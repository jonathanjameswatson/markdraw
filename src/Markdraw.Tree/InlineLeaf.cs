using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Markdraw.Delta.Links;
using Markdraw.Delta.Operations.Inserts;
using Markdraw.Delta.Operations.Inserts.Inlines;
using Markdraw.Helpers;

namespace Markdraw.Tree
{
  public class InlineLeaf : Leaf
  {

    private List<InlineInsert> _correspondingInserts;
    private bool _loose;

    public InlineLeaf(List<InlineInsert> correspondingInserts, int header, bool loose = true) : this(correspondingInserts, header == 0 ? "p" : $"h{header}", null, 0, loose) {}

    public InlineLeaf(List<InlineInsert> correspondingInserts, string tag = "p", DeltaTree deltaTree = null, int i = 0, bool loose = true) : base(deltaTree, i)
    {
      CorrespondingInserts = correspondingInserts;
      Tag = tag;
      _loose = loose;
    }

    protected override Insert CorrespondingInsert => CorrespondingInserts?[0];
    private List<InlineInsert> CorrespondingInserts
    {
      get => _correspondingInserts;
      set
      {
        Length = value.Sum(insert => insert.Length);
        _correspondingInserts = value;
      }
    }

    private bool AddSpans => ParentTree is not null && ParentTree.AddSpans;

    private string Tag { get; set; }

    private static string GetContents(InlineInsert inlineInsert)
    {
      return inlineInsert switch {
        ImageInsert imageInsert => ImageTag(imageInsert),
        InlineHtmlInsert { Content: var content } => content,
        TextInsert { Text: var text } => EscapeHelpers.Escape(text),
        _ => throw new ArgumentOutOfRangeException(nameof(inlineInsert))
      };
    }

    private (string, int) AddCode(List<InlineInsert> inlineInserts, int start)
    {
      var stringBuilder = new StringBuilder();
      var open = false;
      var i = start;
      var buffer = new StringBuilder();

      if (inlineInserts.Count > 0 && inlineInserts[0].Format.Code == true)
      {
        open = true;
      }

      string CodeString(bool code, string text)
      {
        if (!AddSpans) return code ? $@"<code>{text}</code>" : text;
        var tag = code ? "code" : "span";
        return $@"<{tag}>{text}</{tag}>";
      }

      foreach (var inlineInsert in inlineInserts)
      {
        Debug.Assert(inlineInsert.Format.Code != null, "textInsert.Format.Code != null");
        var code = (bool)inlineInsert.Format.Code;

        if (open != code)
        {
          stringBuilder.Append(CodeString(open, buffer.ToString()));
          open = code;
          i += buffer.Length;
          buffer.Clear();
        }

        buffer.Append(GetContents(inlineInsert));
      }

      if (buffer.Length <= 0) return (stringBuilder.ToString(), i);
      stringBuilder.Append(CodeString(open, buffer.ToString()));
      i += buffer.Length;

      return (stringBuilder.ToString(), i);
    }

    private (string, int) AddBold(List<InlineInsert> inlineInserts, int start)
    {
      var stringBuilder = new StringBuilder();
      var open = false;
      var buffer = new List<InlineInsert>();
      var i = start;

      foreach (var inlineInsert in inlineInserts)
      {
        Debug.Assert(inlineInsert.Format.Bold != null, "textInsert.Format.Bold != null");
        var bold = (bool)inlineInsert.Format.Bold;

        switch (open)
        {
          case false when bold:
            var (text1, newI1) = AddCode(buffer, i);

            if (ParentTree is not null && ParentTree.AddSpans)
            {
              stringBuilder.Append($@"{text1}<strong i=""{i}"">");
            }
            else
            {
              stringBuilder.Append($@"{text1}<strong>");
            }

            i = newI1;
            open = true;
            buffer = new List<InlineInsert>();
            break;
          case true when !bold:
            var (text2, newI2) = AddCode(buffer, i);
            stringBuilder.Append($@"{text2}</strong>");
            i = newI2;
            open = false;
            buffer = new List<InlineInsert>();
            break;
        }

        buffer.Add(inlineInsert);
      }

      var (text, newI) = AddCode(buffer, i);
      stringBuilder.Append(text);
      i = newI;

      if (open)
      {
        stringBuilder.Append(@"</strong>");
      }

      return (stringBuilder.ToString(), i);
    }

    private (string, int) AddItalics(List<InlineInsert> inlineInserts, int start)
    {
      var stringBuilder = new StringBuilder();
      var open = false;
      var buffer = new List<InlineInsert>();
      var i = start;

      foreach (var inlineInsert in inlineInserts)
      {
        Debug.Assert(inlineInsert.Format.Italic != null, "textInsert.Format.Italic != null");
        var italic = (bool)inlineInsert.Format.Italic;

        switch (open)
        {
          case false when italic:
            var (text1, newI1) = AddBold(buffer, i);

            if (ParentTree is not null && ParentTree.AddSpans)
            {
              stringBuilder.Append($@"{text1}<em i=""{i}"">");
            }
            else
            {
              stringBuilder.Append($@"{text1}<em>");
            }

            i = newI1;
            open = true;
            buffer = new List<InlineInsert>();
            break;
          case true when !italic:
            var (text2, newI2) = AddBold(buffer, i);
            stringBuilder.Append($@"{text2}</em>");
            i = newI2;
            open = false;
            buffer = new List<InlineInsert>();
            break;
        }

        buffer.Add(inlineInsert);
      }

      var (text, newI) = AddBold(buffer, i);
      stringBuilder.Append(text);
      i = newI;

      if (open)
      {
        stringBuilder.Append(@"</em>");
      }

      return (stringBuilder.ToString(), i);
    }

    private string AddLinks(List<InlineInsert> inlineInserts, int start)
    {
      var stringBuilder = new StringBuilder();
      Link openLink = new NonExistentLink();
      var buffer = new List<InlineInsert>();
      var i = start;

      string LinkString(ExistentLink link)
      {
        var (url, title) = link;
        var escapedUrl = EscapeHelpers.EscapeUrl(url);
        var escapedTitle = EscapeHelpers.Escape(title);
        if (EmailHelpers.IsEmail(url))
        {
          escapedUrl = $"mailto:{escapedUrl}";
        }
        var titleString = title == "" ? "" : $@" title=""{escapedTitle}""";
        return $@"<a href=""{escapedUrl}""{titleString}>";
      }

      foreach (var inlineInsert in inlineInserts)
      {
        var link = inlineInsert.Format.Link;

        if (link is ExistentLink existentLink)
        {
          if (openLink is NonExistentLink)
          {
            var (text1, newI1) = AddItalics(buffer, i);
            stringBuilder.Append($@"{text1}{LinkString(existentLink)}");
            i = newI1;
            openLink = existentLink;
            buffer = new List<InlineInsert>();
          }
          else if (openLink != link)
          {
            var (text2, newI2) = AddItalics(buffer, i);
            stringBuilder.Append($@"{text2}</a>{LinkString(existentLink)}");
            i = newI2;
            openLink = existentLink;
            buffer = new List<InlineInsert>();
          }
        }
        else if (openLink is ExistentLink && link is NonExistentLink)
        {
          var (text3, newI3) = AddItalics(buffer, i);
          stringBuilder.Append($@"{text3}</a>");
          i = newI3;
          openLink = link;
          buffer = new List<InlineInsert>();
        }

        buffer.Add(inlineInsert);
      }

      var (text, _) = AddItalics(buffer, i);
      stringBuilder.Append(text);

      if (openLink is ExistentLink)
      {
        stringBuilder.Append(@"</a>");
      }

      return stringBuilder.ToString();
    }

    private static string ImageTag(ImageInsert imageInsert)
    {
      var (url, alt, title) = imageInsert;
      var titleString = title == "" ? "" : $@"title=""{EscapeHelpers.Escape(title)}"" ";
      return $@"<img src=""{url}"" alt=""{alt}""{titleString}/>";
    }

    public override string ToString()
    {
      var inner = AddLinks(CorrespondingInserts, I);
      return _loose ? $@"<{Tag}>{inner}</{Tag}>" : inner;
    }
  }
}
