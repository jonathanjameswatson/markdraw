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
  }
}