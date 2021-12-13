namespace Markdraw.Delta.Operations
{
  public record Delete(int Length) : LengthOp(Length)
  {
    public override string ToString()
    {
      return $"[Delete {Length}]";
    }
  }
}
