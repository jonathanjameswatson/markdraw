using System;

namespace Markdraw.Delta
{
  public abstract class AmountOp : Op
  {
    private int _amount
    {
      get => _amount;
      set
      {
        if (value < 1)
        {
          throw new ArgumentOutOfRangeException("value", "You must operate on at least one character.");
        }
        _amount = value;
      }
    }

    public int length
    {
      get => _amount;
    }

    public AmountOp(int amount)
    {
      _amount = amount;
    }
  }
}