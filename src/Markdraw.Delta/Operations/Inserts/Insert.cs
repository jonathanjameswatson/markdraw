using Markdraw.Delta.Formats;

namespace Markdraw.Delta.Operations.Inserts
{
  public abstract record Insert : Op
  {
    public virtual int Length => 1;

    public virtual Insert? SetFormat(IFormatModifier formatModifier)
    {
      return null;
    }

    public override string ToString()
    {
      return "[Insert]";
    }
  }
}
