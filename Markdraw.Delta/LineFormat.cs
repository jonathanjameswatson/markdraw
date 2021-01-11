using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdraw.Delta
{
  public class LineFormat : Format
  {
    public List<Indent> Indents;

    private int? _header = 0;
    public int? Header
    {
      get { return _header; }
      set { _header = value is null ? null : Math.Clamp((int)value, 0, 6); }
    }

    public static LineFormat QuotePreset = new LineFormat(new List<Indent>() { Indent.Quote, Indent.Empty(1) }, 0);
    public static LineFormat BulletPreset = new LineFormat(new List<Indent>() { Indent.Bullet, Indent.Empty(1) }, 0);
    public static LineFormat NumberPreset = new LineFormat(new List<Indent>() { Indent.Number(2), Indent.Empty(1) }, 0);
    public static LineFormat CodePreset = new LineFormat(new List<Indent>() { Indent.Code }, 0);

    public LineFormat(List<Indent> indents, int? header)
    {
      Indents = indents;
      Header = header;
    }

    public LineFormat()
    {
      Indents = new List<Indent>();
      Header = 0;
    }

    public void Merge(LineFormat other)
    {
      Indents = other.Indents is null ? Indents : other.Indents;
      Header = other.Header is null ? Header : other.Header;
    }

    public override bool Equals(object obj)
    {
      return (obj is LineFormat lineFormat
              && Indents.SequenceEqual(lineFormat.Indents)
              && Header == lineFormat.Header
             );
    }

    public override int GetHashCode()
    {
      return (Indents, Header).GetHashCode();
    }
  }
}