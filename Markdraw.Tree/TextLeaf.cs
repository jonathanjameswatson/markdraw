using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Markdraw.Delta;

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

    private (string, int) AddBold(List<TextInsert> textInserts, int start)
    {
      var stringBuilder = new StringBuilder();
      var open = false;
      var i = start;
      var buffer = new StringBuilder();

      if (textInserts.Count > 0 && textInserts[0].Format.Bold == true)
      {
        open = true;
      }

      string BoldString(bool bold, string text)
      {
        if (!AddSpans) return bold ? $@"<strong>{text}</strong>" : text;
        var tag = bold ? "strong" : "span";
        return $@"<{tag}>{text}</{tag}>";
      }

      foreach (var textInsert in textInserts)
      {
        Debug.Assert(textInsert.Format.Bold != null, "textInsert.Format.Bold != null");
        var bold = (bool)textInsert.Format.Bold;

        if (open != bold)
        {
          stringBuilder.Append(BoldString(open, buffer.ToString()));
          open = bold;
          i += buffer.Length;
          buffer.Clear();
        }

        buffer.Append(textInsert.Text);
      }

      if (buffer.Length > 0)
      {
        stringBuilder.Append(BoldString(open, buffer.ToString()));
        i += buffer.Length;
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

        if (!open && italic)
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
        }
        else if (open && !italic)
        {
          var (text2, newI2) = AddBold(buffer, i);
          stringBuilder.Append($@"{text2}</em>");
          i = newI2;
          open = false;
          buffer = new List<TextInsert>();
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
      var openLink = "";
      var buffer = new List<TextInsert>();
      var i = start;

      string LinkString(string link) => $@"<a href=""{link}"">";

      foreach (var textInsert in textInserts)
      {
        var link = textInsert.Format.Link;

        if (link != "")
        {
          if (openLink == "")
          {
            var (text1, newI1) = AddItalics(buffer, i);
            stringBuilder.Append($@"{text1}{LinkString(link)}");
            i = newI1;
            openLink = link;
            buffer = new List<TextInsert>();
          }
          else if (openLink != link)
          {
            var (text2, newI2) = AddItalics(buffer, i);
            stringBuilder.Append($@"{text2}</a>{LinkString(link)}");
            i = newI2;
            openLink = link;
            buffer = new List<TextInsert>();
          }
        }
        else if (openLink != "" && link == "")
        {
          var (text3, newI3) = AddItalics(buffer, i);
          stringBuilder.Append($@"{text3}</a>");
          i = newI3;
          openLink = "";
          buffer = new List<TextInsert>();
        }

        buffer.Add(textInsert);
      }

      var (text, _) = AddItalics(buffer, i);
      stringBuilder.Append(text);

      if (openLink != "")
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
