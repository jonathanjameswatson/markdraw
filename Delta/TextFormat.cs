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
      this.Bold = bold;
      this.Italic = italic;
      this.Link = link;
      this.Underlined = underlined;
      this.Struckthrough = struckthrough;
    }

    public TextFormat() { }

    public bool IsSameAs(TextFormat textFormat)
    {
      return (this.Bold == textFormat.Bold
              && this.Italic == textFormat.Italic
              && this.Link == textFormat.Link
              && this.Underlined == textFormat.Underlined
              && this.Struckthrough == textFormat.Struckthrough
             );
    }
  }
}