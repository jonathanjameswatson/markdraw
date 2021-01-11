using System.Text;
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

    public List<TextInsert> CorrespondingInserts { get; set; }
    public string Tag { get; set; }

    public TextLeaf(List<TextInsert> correspondingInserts, int header)
    {
      CorrespondingInserts = correspondingInserts;
      Tag = header == 0 ? "p" : $"h{header}";
    }

    public TextLeaf(List<TextInsert> correspondingInserts) : this(correspondingInserts, 0) { }

    private static string AddBold(List<TextInsert> textInserts)
    {
      var stringBuilder = new StringBuilder();
      bool open = false;

      foreach (var textInsert in textInserts)
      {
        bool bold = (bool)textInsert.Format.Bold;

        if (!open && bold)
        {
          stringBuilder.Append(@"<strong>");
          open = true;
        }

        if (open && !bold)
        {
          stringBuilder.Append(@"</strong>");
          open = false;
        }

        stringBuilder.Append(textInsert.Text);
      }

      if (open)
      {
        stringBuilder.Append(@"</strong>");
      }

      return stringBuilder.ToString();
    }

    private static string AddItalics(List<TextInsert> textInserts)
    {
      var stringBuilder = new StringBuilder();
      bool open = false;
      var buffer = new List<TextInsert>();

      foreach (var textInsert in textInserts)
      {
        bool italic = (bool)textInsert.Format.Italic;

        if (!open && italic)
        {
          stringBuilder.Append($@"{AddBold(buffer)}<em>");
          open = true;
          buffer = new List<TextInsert>();
        }
        else if (open && !italic)
        {
          stringBuilder.Append($@"{AddBold(buffer)}</em>");
          open = false;
          buffer = new List<TextInsert>();
        }

        buffer.Add(textInsert);
      }

      if (open)
      {
        stringBuilder.Append($@"{AddBold(buffer)}</em>");
      }
      else
      {
        stringBuilder.Append(AddBold(buffer));
      }

      return stringBuilder.ToString();
    }

    private static string AddLinks(List<TextInsert> textInserts)
    {
      var stringBuilder = new StringBuilder();
      string openLink = "";
      var buffer = new List<TextInsert>();

      foreach (var textInsert in textInserts)
      {
        string link = textInsert.Format.Link;

        if (link != "")
        {
          if (openLink == "")
          {
            stringBuilder.Append($@"{AddItalics(buffer)}<a href=""{link}"">");
            openLink = link;
          }
          else if (openLink != link)
          {
            stringBuilder.Append($@"{AddItalics(buffer)}</a><a href=""{link}"">");
            openLink = link;
          }

          buffer = new List<TextInsert>();
        }
        else if (openLink != "" && link == "")
        {
          stringBuilder.Append($@"{AddItalics(buffer)}</a>");
          buffer = new List<TextInsert>();
        }

        buffer.Add(textInsert);
      }

      if (openLink != "")
      {
        stringBuilder.Append($@"{AddItalics(buffer)}</a>");
      }
      else
      {
        stringBuilder.Append(AddItalics(buffer));
      }

      return stringBuilder.ToString();
    }

    public override string ToString()
    {
      return $@"<{Tag}>{AddLinks(CorrespondingInserts)}</{Tag}>";
    }
  }
}
