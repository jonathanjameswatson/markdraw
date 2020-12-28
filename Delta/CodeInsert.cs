namespace Markdraw.Delta
{
  public class CodeInsert : EmbedInsert
  {
    private string _text;
    private string _tag;

    public CodeInsert(string text = "", string tag = "")
    {
      this._text = text;
      this._tag = tag;
    }
  }
}