using System;

namespace Markdraw.Delta
{
  public abstract class LengthOp : IOp
  {
    public int Length
    {
      get => Length;
      set
      {
        if (value < 1)
        {
          throw new ArgumentOutOfRangeException("value", "You must operate on at least one character.");
        }
        Length = value;
      }
    }

    public LengthOp(int length)
    {
      Length = length;
    }
  }
}