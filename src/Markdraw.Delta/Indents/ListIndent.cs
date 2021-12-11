namespace Markdraw.Delta.Indents
{
  public abstract record ListIndent(bool Loose) : Indent
  {
    public abstract ListIndent NextIndent { get; }
  }

}
