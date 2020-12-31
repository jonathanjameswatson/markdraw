using System;

namespace Markdraw.Delta
{
  public class TextFormat : Format, ICloneable
  {
    public bool? Bold = false;
    public bool? Italic = false;
    public string Link = "";

    public static TextFormat BoldPreset = new TextFormat(true, false, "");
    public static TextFormat ItalicPreset = new TextFormat(false, true, "");

    public TextFormat(bool? bold, bool? italic, string link)
    {
      Bold = bold;
      Italic = italic;
      Link = link;
    }

    public TextFormat() { }

    public void Merge(TextFormat other)
    {
      Bold = other.Bold is null ? Bold : other.Bold;
      Italic = other.Italic is null ? Italic : other.Italic;
      Link = other.Link is null ? Link : other.Link;
    }

    public override bool Equals(object obj)
    {
      return (obj is TextFormat textFormat
              && Bold == textFormat.Bold
              && Italic == textFormat.Italic
              && Link == textFormat.Link
             );
    }

    public override int GetHashCode()
    {
      return (Bold, Italic, Link).GetHashCode();
    }

    public object Clone()
    {
      return this.MemberwiseClone();
    }
  }
}