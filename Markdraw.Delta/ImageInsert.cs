namespace Markdraw.Delta
{
  public class ImageInsert : EmbedInsert
  {
    public string Url { get; set; }
    public string Alt { get; set; }

    public ImageInsert()
    {
      Url = "";
      Alt = "";
    }

    public ImageInsert(string url, string alt)
    {
      Url = url;
      Alt = alt;
    }

    public override bool Equals(object obj)
    {
      return obj is ImageInsert x && x.Url == Url;
    }

    public override int GetHashCode()
    {
      return Url.GetHashCode();
    }

    public override string ToString()
    {
      return "![{Alt}]({Url})";
    }
  }
}