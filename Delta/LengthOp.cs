using System;

namespace Markdraw.Delta
{
  public abstract class LengthOp : Op
  {
    public int length
    {
      get => length;
      set
      {
        if (value < 1)
        {
          throw new ArgumentOutOfRangeException("value", "You must operate on at least one character.");
        }
        length = value;
      }
    }

    public LengthOp(int length)
    {
      this.length = length;
    }
  }
}