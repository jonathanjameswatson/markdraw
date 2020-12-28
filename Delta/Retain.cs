namespace Markdraw.Delta
{
  public class Retain : LengthOp
  {
    private Format _format;

    public Retain(int length) : base(length)
    {
      this._format = null;
    }

    public Retain(int length, Format format) : base(length)
    {
      this._format = format;
    }
  }
}