using System.Text;

namespace Markdraw.Delta
{
  public class LineInsert : Insert
  {
    public LineFormat Format { get; private set; }

    public LineInsert(LineFormat format)
    {
      Format = format;
    }

    public LineInsert() : this(new LineFormat()) { }

    public override void SetFormat(Format format)
    {
      switch (format)
      {
        case LineFormat lineFormat:
          Format = Format.Merge(lineFormat);
          break;
        case ModifyingLineFormat modifyingLineFormat:
          Format = Format.Modify(modifyingLineFormat);
          break;
      }
    }

    public override bool Equals(object obj)
    {
      return obj is LineInsert x && x.Format.Equals(Format);
    }

    public override int GetHashCode()
    {
      return Format.GetHashCode();
    }

    public override string ToString()
    {
      return @"[\n]";
    }

    public string LineInsertString()
    {
      var stringBuilder = new StringBuilder();

      foreach (var indent in Format.Indents)
      {
        switch (indent.Type)
        {
          case IndentType.Bullet:
            stringBuilder.Append('-');
            break;
          case IndentType.Quote:
            stringBuilder.Append('>');
            break;
          case IndentType.Number:
            stringBuilder.Append("1.");
            break;
          case IndentType.Code:
            stringBuilder.Append("    ");
            break;
          default:
          {
            for (int i = 0; i < indent.Length; i++)
            {
              stringBuilder.Append(' ');
            }
            break;
          }
        }
      }

      if (Format.Header != 0)
      {
        for (int i = 0; i < Format.Header; i++)
        {
          stringBuilder.Append('#');
        }
        stringBuilder.Append(' ');
      }

      return stringBuilder.ToString();
    }
  }
}