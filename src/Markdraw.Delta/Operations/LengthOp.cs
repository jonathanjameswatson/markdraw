using System;

namespace Markdraw.Delta.Operations
{
  public abstract record LengthOp : Op
  {
    private readonly int _length;

    protected LengthOp(int length)
    {
      Length = length;
    }

    public override int Length
    {
      get => _length;
      init
      {
        if (value < 1)
        {
          throw new ArgumentOutOfRangeException(nameof(value), "You must operate on at least one character.");
        }
        _length = value;
      }
    }
  }
}
