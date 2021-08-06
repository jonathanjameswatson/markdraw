using System;
using Markdraw.Delta.Formats;
using Markdraw.Delta.Links;

namespace Markdraw.Delta.Operations.Inserts
{
  public class TextInsert : Insert
  {

    public TextInsert(string text, TextFormat format)
    {
      if (text.Length == 0)
      {
        throw new ArgumentException("TextInsert cannot be empty");
      }

      Text = text;
      Format = format;
    }

    public TextInsert(string text) : this(text, new TextFormat()) {}
    public override int Length => Text.Length;

    public string Text { get; private set; }

    public TextFormat Format { get; private set; }

    public override void SetFormat(Format format)
    {
      if (format is TextFormat textFormat)
      {
        Format = Format.Merge(textFormat);
      }
    }

    public override (int, bool) Subtract(int amount)
    {
      var n = Length;
      if (amount >= n)
      {
        return (n, true);
      }
      Text = Text[..(n - amount)];
      return (amount, false);
    }

    public TextInsert Merge(TextInsert before)
    {
      if (Format.Equals(before.Format))
      {
        before.Text += Text;
        return before;
      }
      return null;
    }

    public TextInsert Merge(TextInsert middle, TextInsert before)
    {
      if (Format.Equals(middle.Format))
      {
        before.Text += middle.Text + Text;
        return before;
      }
      return null;
    }

    public bool DeleteUpTo(int position)
    {
      Text = Text.Substring(position, Length - position);
      return Length == 0;
    }

    public TextInsert SplitAt(int position)
    {
      var startText = Text[..position];
      var endText = Text[position..];
      Text = startText;
      return new TextInsert(endText, Format);
    }

    public override bool Equals(object obj)
    {
      return obj is TextInsert x && x.Text == Text && x.Format.Equals(Format);
    }

    public override int GetHashCode()
    {
      return (Text, Format).GetHashCode();
    }

    public override string ToString()
    {
      var trimmed = Text.TrimStart();
      var bold = Format.Bold == true ? $"**{trimmed}**" : trimmed;
      var italic = Format.Italic == true ? $"*{bold}*" : bold;
      if (Format.Link is not ExistentLink(var url, var title)) return italic;
      var titleString = (title ?? "") == "" ? "" : $@" ""{title}""";
      return $"[{italic}]({url}{titleString})";
    }
  }
}
