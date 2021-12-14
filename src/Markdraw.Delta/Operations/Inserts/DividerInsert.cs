namespace Markdraw.Delta.Operations.Inserts;

public record DividerInsert : Insert
{
  public override string ToString()
  {
    return "\n***\n";
  }
}