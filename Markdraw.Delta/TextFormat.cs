namespace Markdraw.Delta
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

    public TextFormat Merge(TextFormat other)
    {
      return new TextFormat {
        Bold = other.Bold ?? Bold, Italic = other.Italic ?? Italic, Link = other.Link ?? Link
      };
    }
  }
}
