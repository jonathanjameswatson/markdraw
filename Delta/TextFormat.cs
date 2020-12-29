namespace Markdraw.Delta
{
  public class TextFormat : Format
  {
    public bool Bold = false;
    public bool Italic = false;
    public string Link = "";
    public bool Underlined = false;
    public bool Struckthrough = false;

    public TextFormat(bool bold, bool italic, string link, bool underlined, bool struckthrough)
    {
      Bold = bold;
      Italic = italic;
      Link = link;
      Underlined = underlined;
      Struckthrough = struckthrough;
    }

    public TextFormat() { }

    public bool IsSameAs(TextFormat textFormat)
    {
      return (Bold == textFormat.Bold
              && Italic == textFormat.Italic
              && Link == textFormat.Link
              && Underlined == textFormat.Underlined
              && Struckthrough == textFormat.Struckthrough
             );
    }
  }
}