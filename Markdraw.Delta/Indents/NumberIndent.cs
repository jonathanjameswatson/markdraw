using System;

namespace Markdraw.Delta.Indents
{
  public record NumberIndent : ListIndent
  {
    private readonly int _start;
    public int Start
    {
      get => _start;
      init
      {
        if (value < 1)
        {
          throw new ArgumentOutOfRangeException(nameof(value), "Start must be at least one.");
        }

        _start = value;
      }
    }
  }
}
