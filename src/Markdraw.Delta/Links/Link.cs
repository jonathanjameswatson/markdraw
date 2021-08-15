namespace Markdraw.Delta.Links
{
  public abstract record Link
  {
    public abstract Link Merge(Link other);

    public override string ToString()
    {
      return "<Link>";
    }
  };

}
