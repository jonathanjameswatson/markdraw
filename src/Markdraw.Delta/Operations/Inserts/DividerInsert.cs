namespace Markdraw.Delta.Operations.Inserts;

public record DividerInsert : IInsert
{
  public override string ToString()
  {
    return "\n***\n";
  }
}
