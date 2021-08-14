namespace Markdraw.Delta.Operations
{
  public abstract record Op
  {
    public abstract int Length { get; init; }
  }
}
