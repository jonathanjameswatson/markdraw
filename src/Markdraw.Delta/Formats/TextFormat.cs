using System.Text;
using Markdraw.Delta.Links;

namespace Markdraw.Delta.Formats
{
  public record TextFormat : Format
  {

    public static readonly TextFormat BoldPreset = new() {
      Bold = true
    };
    public static readonly TextFormat ItalicPreset = new() {
      Italic = true
    };
    public bool? Bold { get; init; } = false;
    public bool? Italic { get; init; } = false;
    public Link Link { get; init; } = new NonExistentLink();
    public bool? Code { get; init; } = false;

    public TextFormat Merge(TextFormat other)
    {
      return new TextFormat {
        Bold = other.Bold ?? Bold, Italic = other.Italic ?? Italic, Link = Link.Merge(other.Link), Code = other.Code ?? Code
      };
    }

    public override string ToString()
    {
      var stringBuilder = new StringBuilder("{");
      if (Bold == true)
      {
        stringBuilder.Append(" BOLD ");
      }
      if (Italic == true)
      {
        stringBuilder.Append(" ITALIC ");
      }
      if (Link is ExistentLink existentLink)
      {
        stringBuilder.Append($" {existentLink} ");
      }
      if (Code == true)
      {
        stringBuilder.Append(" CODE ");
      }
      stringBuilder.Append('}');
      return stringBuilder.ToString();
    }
  }
}
