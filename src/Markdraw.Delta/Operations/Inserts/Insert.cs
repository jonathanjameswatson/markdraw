using System;
using Markdraw.Delta.Formats;

namespace Markdraw.Delta.Operations.Inserts
{
  public abstract record Insert : Op
  {
    protected virtual int InsertLength => 1;
    public sealed override int Length
    {
      get => InsertLength;
      init => throw new InvalidOperationException("You cannot initialize the length of an insert.");
    }

    public virtual Insert SetFormat(Format format)
    {
      return null;
    }

    public override string ToString()
    {
      return $"[INSERT {Length}]";
    }
  }
}
