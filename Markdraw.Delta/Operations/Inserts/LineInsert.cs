using System.Text;
using Markdraw.Delta.Formats;
using Markdraw.Delta.Indents;

namespace Markdraw.Delta.Operations.Inserts
{
  public class LineInsert : Insert
  {

    public LineInsert(LineFormat format)
    {
      Format = format;
    }

    public LineInsert() : this(new LineFormat()) {}
    public LineFormat Format { get; private set; }

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
      return $@"[\n {Format}]";
    }

    public string LineInsertString()
    {
      var stringBuilder = new StringBuilder();

      foreach (var indent in Format.Indents)
      {
        switch (indent)
        {
          case BulletIndent:
            stringBuilder.Append('-');
            break;
          case QuoteIndent:
            stringBuilder.Append('>');
            break;
          case NumberIndent:
            stringBuilder.Append("1.");
            break;
          case CodeIndent:
            stringBuilder.Append("    ");
            break;
          case ContinueIndent:
            stringBuilder.Append(" "); //last
            break;
        }
      }

      if (Format.Header == 0) return stringBuilder.ToString();

      for (var i = 0; i < Format.Header; i++)
      {
        stringBuilder.Append('#');
      }
      stringBuilder.Append(' ');

      return stringBuilder.ToString();
    }
  }
}
