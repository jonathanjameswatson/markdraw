using System;
using Markdraw.Delta.Formats;

namespace Markdraw.Delta.Operations.Inserts.Inlines
{
  public record TextInsert : InlineInsert
  {

    public TextInsert(string text, InlineFormat format=null)
    {
      Text = text;
      Format = format ?? new InlineFormat();
    }

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
      return Format.Wrap(Text);
    }
  }
}
