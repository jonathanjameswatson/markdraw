using System;

namespace Markdraw.Delta
{
    public class LineFormat : Format {
      public bool quote = false;
      public bool bullet = false;
      public int header
      {
        get { return header; }
        set { header = Math.Min(6, Math.Max(0, value)); }
      }
      public bool ordered = false;

      public LineFormat(bool quote, bool bullet, int header, bool ordered)
      {
        this.quote = quote;
        this.bullet = bullet;
        this.header = header;
        this.ordered = ordered;
      }
    }
}