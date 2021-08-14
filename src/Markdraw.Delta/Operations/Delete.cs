namespace Markdraw.Delta.Operations
{
  public record Delete : LengthOp
  {
    public Delete(int length) : base(length) {}

    public override string ToString()
    {
      return $"[DELETE {Length}]";
    }
  }
}
