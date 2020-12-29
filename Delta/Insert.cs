namespace Markdraw.Delta
{
  public abstract class Insert : IOp
  {
    public int Length
    {
      get => 1;
    }

    public void SetFormat(Format format) { }

    public (int, bool) Subtract(int amount)
    {
      return (1, true);
    }
  }
}