namespace Markdraw.Delta.Operations.Inserts
{
  public record InlineHtmlInsert(string Content = "") : InlineInsert, IHtmlInsert
  {

    public override string ToString()
    {
      return Content;
    }

  }
}
