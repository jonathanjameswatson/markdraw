namespace Markdraw.Delta.Operations.Inserts
{
  public class ImageInsert : EmbedInsert
  {

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
    public string Url { get; set; }
    public string Alt { get; set; }

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
      return $"![{Alt}]({Url})";
    }
  }
}
