namespace Markdraw.Delta.Operations.Inserts.Inlines
{
  public record InlineHtmlInsert(string Content = "") : InlineInsert, IHtmlInsert
  {

    public override string ToString()
    {
      return Format.Wrap(Content);
    }

  }
}
