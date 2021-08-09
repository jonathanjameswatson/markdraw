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
        if (value < 0)
        {
          throw new ArgumentOutOfRangeException(nameof(value), "Start must be at least zero.");
        }

        _start = value;
      }
    }
  }
}
