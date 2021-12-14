using System.Text;
using Markdraw.Delta.Formats;

namespace Markdraw.Delta.Operations.Inserts;

public record LineInsert(LineFormat Format) : Insert
{
  public LineInsert() : this(new LineFormat()) {}

  public override LineInsert? SetFormat(IFormatModifier formatModifier)
  {
    if (formatModifier is not IFormatModifier<LineFormat> lineFormatModifier) return null;
    var newFormat = lineFormatModifier.Modify(Format);
    if (newFormat is null) return null;
    return new LineInsert {
      Format = newFormat
    };
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
      stringBuilder.Append(indent);
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