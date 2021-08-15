namespace Markdraw.Delta.Operations.Inserts
{
  public abstract record InlineInsert : Insert
  {
    public override string ToString()
    {
      return "[InlineInsert]";
    }
  };
}
