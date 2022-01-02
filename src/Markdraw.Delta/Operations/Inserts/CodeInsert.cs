namespace Markdraw.Delta.Operations.Inserts;

public record CodeInsert(string Text = "", string Tag = "") : ISplittableInsert
{
  public int Length => Text.Length;

  ISplittableInsert? ISplittableInsert.Merge(ISplittableInsert before)
  {
    return before switch {
      CodeInsert beforeCodeInsert => Merge(beforeCodeInsert),
      _ => null
    };
  }

  ISplittableInsert? ISplittableInsert.Merge(ISplittableInsert middle, ISplittableInsert before)
  {
    return (middle, before) switch {
      (CodeInsert middleCodeInsert, CodeInsert beforeCodeInsert) => Merge(middleCodeInsert, beforeCodeInsert),
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

  public ISplittableInsert? Merge(CodeInsert before)
  {
    if (!Tag.Equals(before.Tag)) return null;
    return before with {
      Text = before.Text + Text
    };
  }

  public CodeInsert? Merge(CodeInsert middle, CodeInsert before)
  {
    var (middleText, middleTag) = middle;
    var (beforeText, beforeTag) = before;
    if (!Tag.Equals(middleTag) || !Tag.Equals(beforeTag)) return null;
    return before with {
      Text = beforeText + middleText + Text
    };
  }

  public CodeInsert? DeleteUpTo(int position)
  {
    if (position >= Length) return null;
    var newText = Text.Substring(position, Length - position);
    return this with {
      Text = newText
    };
  }

  public (CodeInsert, CodeInsert)? SplitAt(int position)
  {
    if (position == 0 || position >= Length) return null;
    var startText = Text[..position];
    var endText = Text[position..];
    return (this with {
      Text = startText
    }, new CodeInsert(endText, Tag));
  }

  public override string ToString()
  {
    return $"\n```{Tag}\n{Text}\n```\n";
  }
}
