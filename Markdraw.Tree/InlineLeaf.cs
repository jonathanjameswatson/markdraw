using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Markdraw.Delta.Links;
using Markdraw.Delta.Operations.Inserts;
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
        Length = value.Aggregate(0, (acc, textInsert) => acc + textInsert.Length);
        _correspondingInserts = value;
      }
    }

    private bool AddSpans => ParentTree is not null && ParentTree.AddSpans;

    private string Tag { get; set; }

    private (string, int) AddCode(List<InlineInsert> inlineInserts, int start)
    {
      var stringBuilder = new StringBuilder();
      var open = false;
      var i = start;
      var buffer = new StringBuilder();

      if (inlineInserts.Count > 0 && inlineInserts[0] is TextInsert firstTextInsert && firstTextInsert.Format.Code == true)
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
        switch (inlineInsert)
        {
          case ImageInsert imageInsert:
            buffer.Append(ImageTag(imageInsert));
            break;
          case TextInsert textInsert:
            Debug.Assert(textInsert.Format.Code != null, "textInsert.Format.Code != null");
            var code = (bool)textInsert.Format.Code;

            if (open != code)
            {
              stringBuilder.Append(CodeString(open, buffer.ToString()));
              open = code;
              i += buffer.Length;
              buffer.Clear();
            }

            buffer.Append(EscapeHelpers.Escape(textInsert.Text));
            break;
          default:
            throw new ArgumentOutOfRangeException(nameof(inlineInserts));

        }

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
        switch (inlineInsert)
        {
          case ImageInsert:
            break;
          case TextInsert textInsert:
            Debug.Assert(textInsert.Format.Bold != null, "textInsert.Format.Bold != null");
            var bold = (bool)textInsert.Format.Bold;

            switch (open)
            {
              case false when bold:
              {
                var (text1, newI1) = AddCode(buffer, i);

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
              }
              case true when !bold:
                var (text2, newI2) = AddCode(buffer, i);
                stringBuilder.Append($@"{text2}</em>");
                i = newI2;
                open = false;
                buffer = new List<InlineInsert>();
                break;
            }
            break;
          default:
            throw new ArgumentOutOfRangeException(nameof(inlineInserts));

        }

        buffer.Add(inlineInsert);
      }

      var (text, newI) = AddCode(buffer, i);
      stringBuilder.Append(text);
      i = newI;

      if (open)
      {
        stringBuilder.Append(@"</em>");
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
        switch (inlineInsert)
        {
          case ImageInsert:
            break;
          case TextInsert textInsert:
            Debug.Assert(textInsert.Format.Italic != null, "textInsert.Format.Italic != null");
            var italic = (bool)textInsert.Format.Italic;

            switch (open)
            {
              case false when italic:
              {
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
              }
              case true when !italic:
                var (text2, newI2) = AddBold(buffer, i);
                stringBuilder.Append($@"{text2}</em>");
                i = newI2;
                open = false;
                buffer = new List<InlineInsert>();
                break;
            }
            break;
          default:
            throw new ArgumentOutOfRangeException(nameof(inlineInserts));

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
        var titleString = title == "" ? "" : $@" title=""{title}""";
        return $@"<a href=""{escapedUrl}""{titleString}>";
      }

      foreach (var inlineInsert in inlineInserts)
      {
        switch (inlineInsert)
        {
          case ImageInsert:
            break;
          case TextInsert textInsert:
            var link = textInsert.Format.Link;

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
            break;
          default:
            throw new ArgumentOutOfRangeException(nameof(inlineInserts));

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

    private string ImageTag(ImageInsert imageInsert)
    {
      var titleString = imageInsert.Title == "" ? "" : $@"title=""{EscapeHelpers.Escape(imageInsert.Title)}"" ";
      return $@"<img src=""{imageInsert.Url}"" alt=""{imageInsert.Alt}""{titleString}/>";
    }

    public override string ToString()
    {
      var inner = AddLinks(CorrespondingInserts, I);
      return _loose ? $@"<{Tag}>{inner}</{Tag}>" : inner;
    }
  }
}
