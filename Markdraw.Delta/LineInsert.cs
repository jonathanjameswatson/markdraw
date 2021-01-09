namespace Markdraw.Delta
{
  public class LineInsert : Insert
  {
    private LineFormat _format;
    public LineFormat Format { get => _format; }

    public LineInsert(LineFormat format)
    {
      _format = format;
    }

    public LineInsert() : this(new LineFormat()) { }

    public override void SetFormat(Format format)
    {
      if (format is LineFormat lineFormat)
      {
        _format.Merge(lineFormat);
      }
    }

    public override bool Equals(object obj)
    {
      return obj is LineInsert x && x._format.Equals(_format);
    }

    public override int GetHashCode()
    {
      return _format.GetHashCode();
    }
  }
}