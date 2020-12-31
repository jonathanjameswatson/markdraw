namespace Markdraw.Delta
{
  public class Retain : LengthOp
  {
    private Format _format;
    public Format Format { get => _format; }

    public Retain(int length) : base(length)
    {
      _format = null;
    }

    public Retain(int length, Format format) : base(length)
    {
      _format = format;
    }

    public override bool Equals(object obj)
    {
      return obj is Retain x && x.Length == Length && x._format.Equals(_format);
    }

    public override int GetHashCode()
    {
      return (Length, _format).GetHashCode();
    }
  }
}