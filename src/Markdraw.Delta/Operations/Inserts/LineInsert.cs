using System.Text;
using Markdraw.Delta.Formats;
using Markdraw.Delta.Indents;

namespace Markdraw.Delta.Operations.Inserts;

public record IndentState(int LastLength, int? LastNumber);

public record LineInsert(LineFormat Format) : IInsert
{
  public LineInsert() : this(new LineFormat()) {}

  IInsert? IInsert.SetFormat(IFormatModifier formatModifier)
  {
    return SetFormat(formatModifier);
  }

  public LineInsert? SetFormat(IFormatModifier formatModifier)
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

  public string LineInsertString(List<IndentState> indentStates, int indentStateCount)
  {
    var stringBuilder = new StringBuilder();

    var i = 0;
    foreach (var indent in Format.Indents)
    {
      int length;
      int? number = null;

      switch (indent)
      {
        case ContinueIndent:
          length = i < indentStateCount ? indentStates[i].LastLength : 1;
          for (var j = 0; j < length; j++)
          {
            stringBuilder.Append(' ');
          }
          break;
        default:
          string indentString;
          if (indent is NumberIndent numberIndent)
          {
            var lastNumber = i < indentStateCount ? indentStates[i].LastNumber : null;
            number = numberIndent.GetMarkdownNumber(lastNumber);
            indentString = $"{number}.";
          }
          else
          {
            indentString = indent.GetMarkdown();
          }
          length = indentString.Length;
          stringBuilder.Append(indentString);
          break;
      }

      if (indentStates.Count <= i)
      {
        indentStates.Add(new IndentState(length, number));
      }
      else
      {
        indentStates[i] = new IndentState(length, number);
      }

      stringBuilder.Append(' ');
      i += 1;
    }

    if (Format.Header == 0) return stringBuilder.ToString();

    for (var j = 0; j < Format.Header; j++)
    {
      stringBuilder.Append('#');
    }
    stringBuilder.Append(' ');

    return stringBuilder.ToString();
  }
}
