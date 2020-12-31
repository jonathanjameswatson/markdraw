namespace Markdraw.Delta
{
  public class ImageInsert : EmbedInsert
  {
    private string _url = "";

    public ImageInsert() { }

    public ImageInsert(string url)
    {
      _url = url;
    }

    public override bool Equals(object obj)
    {
      return obj is ImageInsert x && x._url == _url;
    }

    public override int GetHashCode()
    {
      return _url.GetHashCode();
    }
  }
}