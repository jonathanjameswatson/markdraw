namespace Markdraw.Delta
{
  public abstract class Insert : IOp
  {
    public virtual int Length => 1;

    public virtual void SetFormat(Format format) {}

    public virtual (int, bool) Subtract(int amount)
    {
      return (1, true);
    }

    public override string ToString()
    {
      return $"[INSERT {Length}]";
    }
  }
}
