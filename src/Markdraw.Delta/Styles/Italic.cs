namespace Markdraw.Delta.Styles;

public record Italic : Style
{
  public override string Wrap(string contents)
  {
    return $"*{contents}*";
  }

  public override string ToString()
  {
    return "<Italic>";
  }
}