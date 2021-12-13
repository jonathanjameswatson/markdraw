using Markdraw.Delta.Formats;

namespace Markdraw.Delta.Operations
{
  public record Retain(IFormatModifier FormatModifier, int Length) : LengthOp(Length)
  {
    public Retain(int length) : this(null, length) {}

    public void Deconstruct(out IFormatModifier formatModifier, out int length)
    {
      formatModifier = FormatModifier;
      length = Length;
    }

    public override string ToString()
    {
      return $"[Retain {Length}]";
    }
  }
}
