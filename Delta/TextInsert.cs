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
  }
}