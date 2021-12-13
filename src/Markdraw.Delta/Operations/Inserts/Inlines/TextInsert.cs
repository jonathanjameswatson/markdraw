using System.Diagnostics.CodeAnalysis;
using Markdraw.Delta.Formats;

namespace Markdraw.Delta.Operations.Inserts.Inlines
{
  public record TextInsert : InlineInsert
  {

    private readonly string _text;

    public TextInsert(string text, [NotNull] InlineFormat format) : base(format)
    {
      Text = text;
    }

    public TextInsert(string text) : this(text, new InlineFormat())
    {
      Text = text;
    }

    public override int Length => Text.Length;
    [NotNull]
    public string Text
    {
      get => _text;
      init
      {
        if (value.Equals(""))
        {
          throw new ArgumentOutOfRangeException(nameof(value), "Text must be a non-empty string.");
        }
        _text = value;
      }
    }

    public TextInsert Merge(TextInsert before)
    {
      if (!Format.Equals(before.Format)) return null;
      return before with {
        Text = before.Text + Text
      };
    }

    public TextInsert Merge(TextInsert middle, TextInsert before)
    {
      if (!Format.Equals(middle.Format)) return null;
      return before with {
        Text = before.Text + middle.Text + Text
      };
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
      return (this with {
        Text = startText
      }, new TextInsert(endText, Format));
    }

    public override string ToString()
    {
      return Format.Wrap(Text);
    }
  }
}
