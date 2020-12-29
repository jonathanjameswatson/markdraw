namespace Markdraw.Delta
{
  public class TextInsert : Insert
  {
    public new int Length
    {
      get => _text.Length;
    }

    private string _text;
    private TextFormat _format;

    public TextInsert(string text, TextFormat format)
    {
      _text = text;
      _format = format;
    }

    public TextInsert(string text) : this(text, new TextFormat()) { }

    public new void SetFormat(Format format)
    {
      if (format is TextFormat textFormat)
      {
        _format = textFormat;
      }
    }

    public new(int, bool) Subtract(int amount)
    {
      int n = _text.Length;
      if (amount >= n)
      {
        return (n, true);
      }
      _text = _text.Substring(0, n - amount);
      return (amount, false);
    }

    public TextInsert Merge(TextInsert before)
    {
      if (_format.IsSameAs(before._format))
      {
        before._text += _text;
        return before;
      }
      else
      {
        return null;
      }
    }

    public TextInsert Merge(TextInsert middle, TextInsert before)
    {
      if (_format.IsSameAs(middle._format))
      {
        before._text += middle._text + _text;
        return before;
      }
      else
      {
        return null;
      }
    }

    public bool DeleteAt(int position, int amount)
    {
      _text = _text.Substring(position, amount);
      return Length == 0;
    }

    public TextInsert SplitAt(int position)
    {
      string startText = _text.Substring(0, position);
      string endText = _text.Substring(position);
      _text = startText;
      return new TextInsert(endText, _format);
    }
  }
}