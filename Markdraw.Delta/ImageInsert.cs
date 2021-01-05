namespace Markdraw.Delta
{
  public class ImageInsert : EmbedInsert
  {
    private string _url = "";
    private string _alt = "";

    public ImageInsert() { }

    public ImageInsert(string url, string alt)
    {
      _url = url;
      _alt = alt;
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