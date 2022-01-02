namespace Markdraw.Delta.Indents;

public record ContinueIndent : Indent
{
  public override string ToString()
  {
    return "||•||";
  }

  public override string GetMarkdown()
  {
    return " ";
  }
}
