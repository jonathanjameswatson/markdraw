namespace Markdraw.Delta.Indents
{
  public record QuoteIndent : Indent
  {
    public bool Start { get; init; }

    public override string ToString()
    {
      return ">";
    }
  }
}
