using System.Text;
using Markdraw.Delta.Links;

namespace Markdraw.Delta.Formats
{
  public record InlineFormat : Format
  {

    public static readonly InlineFormat BoldPreset = new() {
      Bold = true
    };
    public static readonly InlineFormat ItalicPreset = new() {
      Italic = true
    };
    public bool? Bold { get; init; } = false;
    public bool? Italic { get; init; } = false;
    public Link Link { get; init; } = new NonExistentLink();
    public bool? Code { get; init; } = false;

    public InlineFormat Merge(InlineFormat other)
    {
      return new InlineFormat {
        Bold = other.Bold ?? Bold, Italic = other.Italic ?? Italic, Link = Link.Merge(other.Link), Code = other.Code ?? Code
      };
    }

    public string Wrap(string text)
    {
      var trimmed = text.TrimStart();
      var bold = Bold == true ? $"**{trimmed}**" : trimmed;
      var italic = Italic == true ? $"*{bold}*" : bold;
      if (Link is not ExistentLink(var url, var title)) return italic;
      var titleString = (title ?? "") == "" ? "" : $@" ""{title}""";
      return $"[{italic}]({url}{titleString})";
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
