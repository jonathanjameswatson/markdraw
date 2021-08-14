namespace Markdraw.Delta.Indents
{
  public record QuoteIndent : Indent
  {
    public override string ToString()
    {
      return ">";
    }
  }
}
