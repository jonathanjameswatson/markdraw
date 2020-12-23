namespace Markdraw.Delta {
  public class LineInsert : Insert {
    private LineFormat _format;

    public new void setFormat(Format format)
    {
      if (format is LineFormat lineFormat)
      {
        this._format = lineFormat;
      }
    }
  }
}