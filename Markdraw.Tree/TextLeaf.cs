using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Markdraw.Delta;
using Markdraw.Delta.Links;
using Markdraw.Delta.Operations.Inserts;

namespace Markdraw.Tree
{
  public class TextLeaf : Leaf
  {

    private List<TextInsert> _correspondingInserts;

    public TextLeaf(List<TextInsert> correspondingInserts, int header) : this(correspondingInserts, header == 0 ? "p" : $"h{header}") {}

    public TextLeaf(List<TextInsert> correspondingInserts, string tag = "p", DeltaTree deltaTree = null, int i = 0) : base(deltaTree, i)
    {
      CorrespondingInserts = correspondingInserts;
      Tag = tag;
    }

    protected override Insert CorrespondingInsert => CorrespondingInserts?[0];
    private List<TextInsert> CorrespondingInserts
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

    private (string, int) AddCode(List<TextInsert> textInserts, int start)
    {
      var stringBuilder = new StringBuilder();
      var open = false;
      var i = start;
      var buffer = new StringBuilder();

      if (textInserts.Count > 0 && textInserts[0].Format.Code == true)
      {
        open = true;
      }

      string CodeString(bool code, string text)
      {
        if (!AddSpans) return code ? $@"<code>{text}</code>" : text;
        var tag = code ? "code" : "span";
        return $@"<{tag}>{text}</{tag}>";
      }

      foreach (var textInsert in textInserts)
      {
        Debug.Assert(textInsert.Format.Code != null, "textInsert.Format.Code != null");
        var code = (bool)textInsert.Format.Code;

        if (open != code)
        {
          stringBuilder.Append(CodeString(open, buffer.ToString()));
          open = code;
          i += buffer.Length;
          buffer.Clear();
        }

        buffer.Append(textInsert.Text);
      }

      if (buffer.Length <= 0) return (stringBuilder.ToString(), i);
      stringBuilder.Append(CodeString(open, buffer.ToString()));
      i += buffer.Length;

      return (stringBuilder.ToString(), i);
    }

    private (string, int) AddBold(List<TextInsert> textInserts, int start)
    {
      var stringBuilder = new StringBuilder();
      var open = false;
      var buffer = new List<TextInsert>();
      var i = start;

      foreach (var textInsert in textInserts)
      {
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
            buffer = new List<TextInsert>();
            break;
          }
          case true when !bold:
            var (text2, newI2) = AddCode(buffer, i);
            stringBuilder.Append($@"{text2}</em>");
            i = newI2;
            open = false;
            buffer = new List<TextInsert>();
            break;
        }

        buffer.Add(textInsert);
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

    private (string, int) AddItalics(List<TextInsert> textInserts, int start)
    {
      var stringBuilder = new StringBuilder();
      var open = false;
      var buffer = new List<TextInsert>();
      var i = start;

      foreach (var textInsert in textInserts)
      {
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
            buffer = new List<TextInsert>();
            break;
          }
          case true when !italic:
            var (text2, newI2) = AddBold(buffer, i);
            stringBuilder.Append($@"{text2}</em>");
            i = newI2;
            open = false;
            buffer = new List<TextInsert>();
            break;
        }

        buffer.Add(textInsert);
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

    private string AddLinks(List<TextInsert> textInserts, int start)
    {
      var stringBuilder = new StringBuilder();
      Link openLink = new NonExistentLink();
      var buffer = new List<TextInsert>();
      var i = start;

      string LinkString(ExistentLink link)
      {
        var (url, title) = link;
        var titleString = title == "" ? "" : $@" title=""{title}""";
        return $@"<a href=""{url}""{titleString}>";
      }

      foreach (var textInsert in textInserts)
      {
        var link = textInsert.Format.Link;

        if (link is ExistentLink existentLink)
        {
          if (openLink is NonExistentLink)
          {
            var (text1, newI1) = AddItalics(buffer, i);
            stringBuilder.Append($@"{text1}{LinkString(existentLink)}");
            i = newI1;
            openLink = existentLink;
            buffer = new List<TextInsert>();
          }
          else if (openLink != link)
          {
            var (text2, newI2) = AddItalics(buffer, i);
            stringBuilder.Append($@"{text2}</a>{LinkString(existentLink)}");
            i = newI2;
            openLink = existentLink;
            buffer = new List<TextInsert>();
          }
        }
        else if (openLink is ExistentLink && link is NonExistentLink)
        {
          var (text3, newI3) = AddItalics(buffer, i);
          stringBuilder.Append($@"{text3}</a>");
          i = newI3;
          openLink = link;
          buffer = new List<TextInsert>();
        }

        buffer.Add(textInsert);
      }

      var (text, _) = AddItalics(buffer, i);
      stringBuilder.Append(text);

      if (openLink is ExistentLink)
      {
        stringBuilder.Append(@"</a>");
      }

      return stringBuilder.ToString();
    }

    public override string ToString()
    {
      return $@"<{Tag}>{AddLinks(CorrespondingInserts, I)}</{Tag}>";
    }
  }
}
