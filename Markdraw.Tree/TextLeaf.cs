using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Markdraw.Delta;

namespace Markdraw.Tree
{
  public class TextLeaf : Leaf
  {
    public override Insert CorrespondingInsert
    {
      get => CorrespondingInserts?[0];
    }

    private List<TextInsert> _correspondingInserts;
    public List<TextInsert> CorrespondingInserts
    {
      get => _correspondingInserts;
      set
      {
        _length = value.Aggregate(0, (acc, textInsert) => acc + textInsert.Length);
        _correspondingInserts = value;
      }
    }

    public bool AddSpans { get => ParentTree is not null && ParentTree.AddSpans; }

    public string Tag { get; set; }

    public TextLeaf(List<TextInsert> correspondingInserts, string tag) : this(correspondingInserts, tag, null, 0) { }

    public TextLeaf(List<TextInsert> correspondingInserts, int header) : this(correspondingInserts, header == 0 ? "p" : $"h{header}", null, 0) { }

    public TextLeaf(List<TextInsert> correspondingInserts, string tag, DeltaTree deltaTree, int i) : base(deltaTree, i)
    {
      CorrespondingInserts = correspondingInserts;
      Tag = tag;
    }

    public TextLeaf(List<TextInsert> correspondingInserts) : this(correspondingInserts, "p") { }

    private (string, int) AddBold(List<TextInsert> textInserts, int start)
    {
      var stringBuilder = new StringBuilder();
      bool open = false;
      int i = start;
      var buffer = new StringBuilder();

      if (textInserts.Count > 0 && textInserts[0].Format.Bold == true)
      {
        open = true;
      }

      Func<bool, string, int, string> BoldString = (bold, text, i) =>
      {
        if (AddSpans)
        {
          string tag = bold ? "strong" : "span";
          return $@"<{tag}>{text}</{tag}>";
        }

        if (bold)
        {
          return $@"<strong>{text}</strong>";
        }

        return text;
      };

      foreach (var textInsert in textInserts)
      {
        bool bold = (bool)textInsert.Format.Bold;

        if (open != bold)
        {
          stringBuilder.Append(BoldString(open, buffer.ToString(), i));
          open = bold;
          i += buffer.Length;
          buffer.Clear();
        }

        buffer.Append(textInsert.Text);
      }

      if (buffer.Length > 0)
      {
        stringBuilder.Append(BoldString(open, buffer.ToString(), i));
        i += buffer.Length;
      }

      return (stringBuilder.ToString(), i);
    }

    private (string, int) AddItalics(List<TextInsert> textInserts, int start)
    {
      var stringBuilder = new StringBuilder();
      bool open = false;
      var buffer = new List<TextInsert>();
      int i = start;

      foreach (var textInsert in textInserts)
      {
        bool italic = (bool)textInsert.Format.Italic;

        if (!open && italic)
        {
          (string text1, int newI1) = AddBold(buffer, i);

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
          (string text2, int newI2) = AddBold(buffer, i);
          stringBuilder.Append($@"{text2}</em>");
          i = newI2;
          open = false;
          buffer = new List<TextInsert>();
        }

        buffer.Add(textInsert);
      }

      (string text, int newI) = AddBold(buffer, i);
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
      string openLink = "";
      var buffer = new List<TextInsert>();
      int i = start;

      Func<string, int, string> LinkString = (link, i) =>
      {
        return $@"<a href=""{link}"">";
      };

      foreach (var textInsert in textInserts)
      {
        string link = textInsert.Format.Link;

        if (link != "")
        {
          if (openLink == "")
          {
            (string text1, int newI1) = AddItalics(buffer, i);
            stringBuilder.Append($@"{text1}{LinkString(link, i)}");
            i = newI1;
            openLink = link;
            buffer = new List<TextInsert>();
          }
          else if (openLink != link)
          {
            (string text2, int newI2) = AddItalics(buffer, i);
            stringBuilder.Append($@"{text2}</a>{LinkString(link, i)}");
            i = newI2;
            openLink = link;
            buffer = new List<TextInsert>();
          }
        }
        else if (openLink != "" && link == "")
        {
          (string text3, int newI3) = AddItalics(buffer, i);
          stringBuilder.Append($@"{text3}</a>");
          i = newI3;
          openLink = "";
          buffer = new List<TextInsert>();
        }

        buffer.Add(textInsert);
      }

      (string text, int newI) = AddItalics(buffer, i);
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
