using System.Text;
using Markdraw.Delta.Formats;

namespace Markdraw.Delta.Operations.Inserts
{
  public record LineInsert : Insert
  {

    public LineInsert(LineFormat format)
    {
      Format = format;
    }

    public LineInsert() : this(new LineFormat()) {}
    public LineFormat Format { get; init; }

    public override LineInsert SetFormat(Format format)
    {
      if (format is not ILineFormatModifier lineFormatModifier) return null;
      var result = lineFormatModifier.Modify(Format);
      if (result is null) return null;
      return new LineInsert {
        Format = result
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
}
