using System;

namespace Markdraw.Delta
{
  public class LineFormat : Format
  {
    public bool? Quote = false;
    public bool? Bullet = false;
    public bool? Ordered = false;

    private int? _header = 0;
    public int? Header
    {
      get { return _header; }
      set { _header = value is null ? null : Math.Clamp((int)value, 0, 6); }
    }

    public static LineFormat QuotePreset = new LineFormat(true, false, false, 0);
    public static LineFormat BulletPreset = new LineFormat(false, true, false, 0);
    public static LineFormat OrderedPreset = new LineFormat(false, false, true, 0);

    public LineFormat(bool? quote, bool? bullet, bool? ordered, int? header)
    {
      Quote = quote;
      Bullet = bullet;
      Ordered = ordered;
      Header = header;
    }

    public LineFormat() { }

    public override bool Equals(object obj)
    {
      return (obj is LineFormat lineFormat
              && Quote == lineFormat.Quote
              && Bullet == lineFormat.Bullet
              && Ordered == lineFormat.Ordered
              && Header == lineFormat.Header
             );
    }

    public override int GetHashCode()
    {
      return (Quote, Bullet, Ordered, Header).GetHashCode();
    }
  }
}