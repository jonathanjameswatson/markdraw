namespace Markdraw.Delta.Indents;

public record QuoteIndent(bool Start = false) : Indent
{
  public override string ToString()
  {
    return "||>||";
  }

  public override string GetMarkdown()
  {
    return ">";
  }
}
