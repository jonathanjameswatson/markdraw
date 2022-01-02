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

  public override int Length => Text.Length;

  ISplittableInsert? ISplittableInsert.Merge(ISplittableInsert before)
  {
    return before switch {
      TextInsert beforeTextInsert => Merge(beforeTextInsert),
      _ => null
    };
  }

  ISplittableInsert? ISplittableInsert.Merge(ISplittableInsert middle, ISplittableInsert before)
  {
    return (middle, before) switch {
      (TextInsert middleTextInsert, TextInsert beforeTextInsert) => Merge(middleTextInsert, beforeTextInsert),
      _ => null
    };
  }

  ISplittableInsert? ISplittableInsert.DeleteUpTo(int position)
  {
    return DeleteUpTo(position);
  }

  (ISplittableInsert, ISplittableInsert)? ISplittableInsert.SplitAt(int position)
  {
    return SplitAt(position);
  }

  public void Deconstruct(out string text, out InlineFormat format)
  {
    text = _text;
    format = Format;
  }

  public ISplittableInsert? Merge(TextInsert before)
  {
    var (beforeText, beforeFormat) = before;
    if (!Format.Equals(beforeFormat)) return null;
    return before with {
      Text = beforeText + Text
    };
  }

  public TextInsert? Merge(TextInsert middle, TextInsert before)
  {
    var (middleText, middleFormat) = middle;
    var (beforeText, beforeFormat) = before;
    if (!Format.Equals(middleFormat) || !Format.Equals(beforeFormat)) return null;
    return before with {
      Text = beforeText + middleText + Text
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

  public (TextInsert, TextInsert)? SplitAt(int position)
  {
    if (position == 0 || position >= Length) return null;
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
