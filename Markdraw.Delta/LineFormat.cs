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
      Quote = quote;
      Bullet = bullet;
      Header = header;
      Ordered = ordered;
    }

    public LineFormat()
    {
      Header = 0;
    }
  }
}