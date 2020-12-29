using System;

namespace Markdraw.Delta
{
  public class LineFormat : Format
  {
    public bool Quote = false;
    public bool Bullet = false;
    public int Header
    {
      get { return Header; }
      set { Header = Math.Clamp(value, 0, 6); }
    }
    public bool Ordered = false;

    public LineFormat(bool quote, bool bullet, int header, bool ordered)
    {
      this.Quote = quote;
      this.Bullet = bullet;
      this.Header = header;
      this.Ordered = ordered;
    }

    public LineFormat()
    {
      this.Header = 0;
    }
  }
}