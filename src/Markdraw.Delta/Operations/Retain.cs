using Markdraw.Delta.Formats;

namespace Markdraw.Delta.Operations
{
  public record Retain : LengthOp
  {

    public Retain(int length) : base(length)
    {
      Format = null;
    }

    public Retain(int length, Format format) : base(length)
    {
      Format = format;
    }

    public Format Format { get; init; }

    public override string ToString()
    {
      return $"[Retain {Length}]";
    }
  }
}
