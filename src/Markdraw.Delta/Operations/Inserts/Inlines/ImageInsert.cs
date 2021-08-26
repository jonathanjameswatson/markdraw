namespace Markdraw.Delta.Operations.Inserts.Inlines
{
  public record ImageInsert(string Url, string Alt = "", string Title = "") : InlineInsert
  {

    public override string ToString()
    {
      return Format.Wrap($"![{Alt}]({Url})");
    }

  }
}
