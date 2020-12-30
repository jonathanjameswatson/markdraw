namespace Markdraw.Delta
{
  public class DividerInsert : EmbedInsert
  {
    public override bool Equals(object obj)
    {
      return obj is EmbedInsert x;
    }

    public override int GetHashCode()
    {
      return 0;
    }
  }
}