namespace Markdraw.Delta.Operations.Inserts
{
  public class DividerInsert : Insert
  {
    public override bool Equals(object obj)
    {
      return obj is DividerInsert;
    }

    public override int GetHashCode()
    {
      return 0;
    }

    public override string ToString()
    {
      return "\n***\n";
    }
  }
}
