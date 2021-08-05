using Markdraw.Delta.Formats;

namespace Markdraw.Delta.Operations
{
  public class Retain : LengthOp
  {

    public Retain(int length) : base(length)
    {
      Format = null;
    }

    public Retain(int length, Format format) : base(length)
    {
      Format = format;
    }

    public Format Format { get; }

    public override bool Equals(object obj)
    {
      return obj is Retain x && x.Length == Length && x.Format.Equals(Format);
    }

    public override int GetHashCode()
    {
      return (Length, Format).GetHashCode();
    }

    public override string ToString()
    {
      return $"[RETAIN {Length}]";
    }
  }
}
