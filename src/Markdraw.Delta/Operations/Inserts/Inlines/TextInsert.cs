using Markdraw.Delta.Formats;

namespace Markdraw.Delta.Operations.Inserts.Inlines;

public record TextInsert : InlineInsert, ISplittableInsert
{
  private readonly string _text = "";

  public TextInsert(string text, InlineFormat format) : base(format)
  {
    Text = text;
  }

  public TextInsert(string text) : this(text, new InlineFormat())
  {
    Text = text;
  }

  public override int Length => Text.Length;
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

  public TextInsert? Merge(TextInsert before)
  {
    if (!Format.Equals(before.Format)) return null;
    return before with {
      Text = before.Text + Text
    };
  }

  ISplittableInsert? ISplittableInsert.Merge(ISplittableInsert before)
  {
    return before switch {
      TextInsert beforeTextInsert => Merge(beforeTextInsert),
      _ => null
    };
  }

  public TextInsert? Merge(TextInsert middle, TextInsert before)
  {
    if (!Format.Equals(middle.Format)) return null;
    return before with {
      Text = before.Text + middle.Text + Text
    };
  }

  ISplittableInsert? ISplittableInsert.Merge(ISplittableInsert middle, ISplittableInsert before)
  {
    return (middle, before) switch {
      (TextInsert middleTextInsert, TextInsert beforeTextInsert) => Merge(middleTextInsert, beforeTextInsert),
      _ => null
    };
  }

  public TextInsert? DeleteUpTo(int position)
  {
    if (position >= Length) return null;
    var newText = Text.Substring(position, Length - position);
    return this with {
      Text = newText
    };
  }

  ISplittableInsert? ISplittableInsert.DeleteUpTo(int position) => DeleteUpTo(position);

  public (TextInsert, TextInsert)? SplitAt(int position)
  {
    if (position == 0 || position >= Length) return null;
    var startText = Text[..position];
    var endText = Text[position..];
    return (this with {
      Text = startText
    }, new TextInsert(endText, Format));
  }

  (ISplittableInsert, ISplittableInsert)? ISplittableInsert.SplitAt(int position) => SplitAt(position);

  public override string ToString()
  {
    return Format.Wrap(Text);
  }
}
