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
    public string Link { get; init; } = "";
    public bool? Code { get; init; } = false;

    public TextFormat Merge(TextFormat other)
    {
      return new TextFormat {
        Bold = other.Bold ?? Bold, Italic = other.Italic ?? Italic, Link = other.Link ?? Link, Code = other.Code ?? Code
      };
    }
  }
}
