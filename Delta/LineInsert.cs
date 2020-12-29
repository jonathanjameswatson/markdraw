namespace Markdraw.Delta
{
  public class LineInsert : Insert
  {
    private LineFormat _format;

    public LineInsert(LineFormat format)
    {
      this._format = format;
    }

    public LineInsert() : this(new LineFormat()) { }

    public new void SetFormat(Format format)
    {
      if (format is LineFormat lineFormat)
      {
        this._format = lineFormat;
      }
    }
  }
}