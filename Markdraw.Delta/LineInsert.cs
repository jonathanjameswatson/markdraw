using System.Text;

namespace Markdraw.Delta
{
  public class LineInsert : Insert
  {
    private LineFormat _format;
    public LineFormat Format { get => _format; }

    public LineInsert(LineFormat format)
    {
      _format = format;
    }

    public LineInsert() : this(new LineFormat()) { }

    public override void SetFormat(Format format)
    {
      if (format is LineFormat lineFormat)
      {
        _format.Merge(lineFormat);
      }
      else if (format is ModifyingLineFormat modifyingLineFormat)
      {
        _format.Modify(modifyingLineFormat);
      }
    }

    public override bool Equals(object obj)
    {
      return obj is LineInsert x && x._format.Equals(_format);
    }

    public override int GetHashCode()
    {
      return _format.GetHashCode();
    }

    public override string ToString()
    {
      return "\n";
    }

    public string LineInsertString()
    {
      var stringBuilder = new StringBuilder();

      foreach (var indent in Format.Indents)
      {
        if (indent.Type == IndentType.Bullet)
        {
          stringBuilder.Append("-");
        }
        else if (indent.Type == IndentType.Quote)
        {
          stringBuilder.Append(">");
        }
        else if (indent.Type == IndentType.Number)
        {
          stringBuilder.Append("1.");
        }
        else if (indent.Type == IndentType.Code)
        {
          stringBuilder.Append("    ");
        }
        else
        {
          for (int i = 0; i < indent.Length; i++)
          {
            stringBuilder.Append(" ");
          }
        }
      }

      if (Format.Header != 0)
      {
        for (int i = 0; i < Format.Header; i++)
        {
          stringBuilder.Append("#");
        }
        stringBuilder.Append(" ");
      }

      return stringBuilder.ToString();
    }
  }
}