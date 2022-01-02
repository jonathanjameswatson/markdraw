namespace Markdraw.Delta.Indents;

public record NumberIndent : ListIndent
{
  private readonly int _start;

  public NumberIndent(int start = -1, bool loose = true) : base(loose)
  {
    Start = start;
  }

  public int Start
  {
    get => _start;
    init
    {
      if (value < -1)
      {
        throw new ArgumentOutOfRangeException(nameof(value), "Start must be at least -1.");
      }

      _start = value;
    }
  }

  public void Deconstruct(out int start, out bool loose)
  {
    start = Start;
    loose = Loose;
  }

  public override string ToString()
  {
    return $"||{(Start == -1 ? "?" : Start.ToString())}. {(Loose ? "LOOSE" : "TIGHT")}||";
  }

  public int GetMarkdownNumber(int? lastNumber)
  {
    return Start != -1 ? Start : (lastNumber ?? 0) + 1;
  }

  public override string GetMarkdown()
  {
    return $"{GetMarkdownNumber(null)}.";
  }
}
