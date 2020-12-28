namespace Markdraw.Delta
{
  public abstract class Insert : Op
  {
    public int length
    {
      get => 1;
    }

    public void setFormat(Format format) { }

    public (int, bool) subtract(int amount)
    {
      return (1, true);
    }
  }
}