namespace Markdraw.Delta {
  public class TextInsert : Insert {
    private string _text;
    private TextFormat _format;

    public new void setFormat(Format format)
    {
      if (format is TextFormat textFormat) {
        this._format = textFormat;
      }
    }
  }
}