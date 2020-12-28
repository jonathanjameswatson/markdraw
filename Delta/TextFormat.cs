namespace Markdraw.Delta
{
  public class TextFormat : Format
  {
    public bool bold = false;
    public bool italic = false;
    public string link = "";
    public bool underlined = false;
    public bool struckthrough = false;

    public TextFormat(bool bold, bool italic, string link, bool underlined, bool struckthrough)
    {
      this.bold = bold;
      this.italic = italic;
      this.link = link;
      this.underlined = underlined;
      this.struckthrough = struckthrough;
    }

    public TextFormat() { }

    public bool hasSameAs(TextFormat textFormat)
    {
      return (this.bold == textFormat.bold
              && this.italic == textFormat.italic
              && this.link == textFormat.link
              && this.underlined == textFormat.underlined
              && this.struckthrough == textFormat.struckthrough
             );
    }
  }
}