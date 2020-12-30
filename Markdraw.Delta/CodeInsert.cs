namespace Markdraw.Delta
{
  public class CodeInsert : EmbedInsert
  {
    private string _text;
    private string _tag;

    public CodeInsert(string text = "", string tag = "")
    {
      _text = text;
      _tag = tag;
    }

    public override bool Equals(object obj)
    {
      return obj is CodeInsert x && x._text == _text && x._tag == _tag;
    }

    public override int GetHashCode()
    {
      return (_text, _tag).GetHashCode();
    }
  }
}