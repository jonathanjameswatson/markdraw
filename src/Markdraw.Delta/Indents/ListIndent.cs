namespace Markdraw.Delta.Indents
{
  public abstract record ListIndent : Indent
  {
    public bool Loose { get; init; }
  }

}


