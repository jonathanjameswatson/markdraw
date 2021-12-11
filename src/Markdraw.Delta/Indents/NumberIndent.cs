using System;

namespace Markdraw.Delta.Indents
{
  public record NumberIndent : ListIndent
  {
    private readonly int _start;

    public NumberIndent(int start = 0, bool loose = false) : base(loose)
    {
      Start = start;
    }
    public int Start
    {
      get => _start;
      init
      {
        if (value < 0)
        {
          throw new ArgumentOutOfRangeException(nameof(value), "Start must be at least zero.");
        }

        _start = value;
      }
    }

    public override NumberIndent NextIndent => this with {
      Start = 0
    };

    public void Deconstruct(out int start, out bool loose)
    {
      start = Start;
      loose = Loose;
    }

    public override string ToString()
    {
      return $"{(Start <= 1 ? 1 : Start)}.";
    }
  }
}
