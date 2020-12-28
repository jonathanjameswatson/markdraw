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
  }
}