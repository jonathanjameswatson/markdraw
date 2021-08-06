namespace Markdraw.Delta.Links
{
  public abstract record Link
  {
    public abstract Link Merge(Link other);
  };

}
