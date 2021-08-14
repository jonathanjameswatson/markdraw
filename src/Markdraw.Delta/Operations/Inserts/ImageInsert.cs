namespace Markdraw.Delta.Operations.Inserts
{
  public record ImageInsert(string Url, string Alt = "", string Title = "") : InlineInsert
  {

    public override string ToString()
    {
      return $"![{Alt}]({Url})";
    }

  }
}
