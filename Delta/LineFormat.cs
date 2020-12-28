using System;

namespace Markdraw.Delta
{
  public class LineFormat : Format
  {
    public bool quote = false;
    public bool bullet = false;
    public int header
    {
      get { return header; }
      set { header = Math.Clamp(value, 0, 6); }
    }
    public bool ordered = false;

    public LineFormat(bool quote, bool bullet, int header, bool ordered)
    {
      this.quote = quote;
      this.bullet = bullet;
      this.header = header;
      this.ordered = ordered;
    }

    public LineFormat()
    {
      this.header = 0;
    }
  }
}