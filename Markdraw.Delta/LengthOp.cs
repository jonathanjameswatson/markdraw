using System;

namespace Markdraw.Delta
{
  public abstract class LengthOp : IOp
  {
    private int _length;

    public LengthOp(int length)
    {
      Length = length;
    }

    public int Length
    {
      get => _length;
      set
      {
        if (value < 1)
        {
          throw new ArgumentOutOfRangeException("value", "You must operate on at least one character.");
        }
        _length = value;
      }
    }
  }
}
