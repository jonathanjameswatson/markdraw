using System;
using Markdraw.Delta.Formats;
using Markdraw.Delta.Links;

namespace Markdraw.Delta.Operations.Inserts
{
  public record TextInsert : InlineInsert
  {

    public TextInsert(string text, TextFormat format)
    {
      Text = text;
      Format = format;
    }

    public TextInsert(string text) : this(text, new TextFormat()) {}
    protected override int InsertLength => Text.Length;

    private readonly string _text;
    public string Text
    {
      get => _text;
      init
      {
        if (value is null or "")
        {
          throw new ArgumentOutOfRangeException(nameof(value), "Text must be a non-empty string.");
        }
        _text = value;
      }
    }

    public TextFormat Format { get; init; }

    public override TextInsert SetFormat(Format format)
    {
      if (format is TextFormat textFormat)
      {
        return this with {
          Format = Format.Merge(textFormat)
        };
      }
      return null;
    }

    public TextInsert Merge(TextInsert before)
    {
      if (!Format.Equals(before.Format)) return null;
      return before with { Text = before.Text + Text };
    }

    public TextInsert Merge(TextInsert middle, TextInsert before)
    {
      if (!Format.Equals(middle.Format)) return null;
      return before with { Text = before.Text + middle.Text + Text };
    }

    public TextInsert DeleteUpTo(int position)
    {
      var newText = Text.Substring(position, Length - position);
      if (newText.Length == 0) return null;
      return this with {
        Text = newText
      };
    }

    public (TextInsert, TextInsert) SplitAt(int position)
    {
      var startText = Text[..position];
      var endText = Text[position..];
      return (this with { Text = startText }, new TextInsert(endText, Format));
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
