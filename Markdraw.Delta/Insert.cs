namespace Markdraw.Delta
{
  public abstract class Insert : IOp
  {
    public int Length
    {
      get => 1;
    }

    public void SetFormat(Format format) { }

    public virtual (int, bool) Subtract(int amount)
    {
      return (1, true);
    }
  }
}