namespace Markdraw.Delta
{
  public class TextFormat : Format
  {
    public bool? Bold = false;
    public bool? Italic = false;
    public string Link = "";

    public TextFormat(bool? bold, bool? italic, string link)
    {
      Bold = bold;
      Italic = italic;
      Link = link;
    }

    public static TextFormat BoldPreset = new TextFormat(true, false, "");
    public static TextFormat ItalicPreset = new TextFormat(false, true, "");

    public TextFormat() { }

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
  }
}