namespace Markdraw.Delta
{
  public class Retain : LengthOp
  {
    private Format _format;

    public Retain(int length) : base(length)
    {
      _format = null;
    }

    public Retain(int length, Format format) : base(length)
    {
      _format = format;
    }
  }
}