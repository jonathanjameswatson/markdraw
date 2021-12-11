namespace Markdraw.Delta.Operations.Inserts
{
  public record BlockHtmlInsert(string Content = "") : Insert, IHtmlInsert
  {

    public override string ToString()
    {
      return $"\n{Content}\n";
    }
  }
}
