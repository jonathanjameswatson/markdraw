namespace Markdraw.Delta
{
  public class TextInsert : Insert
  {
    public new int length
    {
      get => _text.Length;
    }

    private string _text;
    private TextFormat _format;

    public new void setFormat(Format format)
    {
      if (format is TextFormat textFormat)
      {
        this._format = textFormat;
      }
    }

    public TextInsert(string text, TextFormat format)
    {
      this._text = text;
      this._format = format;
    }

    public TextInsert(string text) : this(text, new TextFormat()) { }
  }
}