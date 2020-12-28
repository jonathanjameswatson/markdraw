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

    public TextInsert(string text, TextFormat format)
    {
      this._text = text;
      this._format = format;
    }

    public TextInsert(string text) : this(text, new TextFormat()) { }

    public new void setFormat(Format format)
    {
      if (format is TextFormat textFormat)
      {
        this._format = textFormat;
      }
    }

    public new(int, bool) subtract(int amount)
    {
      int n = this._text.Length;
      if (amount >= n)
      {
        return (n, true);
      }
      _text = _text.Substring(0, n - amount);
      return (amount, false);
    }

    public TextInsert merge(TextInsert before)
    {
      if (this._format.hasSameAs(before._format))
      {
        before._text += this._text;
        return before;
      }
      else
      {
        return null;
      }
    }


  }
}