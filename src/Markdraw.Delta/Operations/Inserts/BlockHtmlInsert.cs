namespace Markdraw.Delta.Operations.Inserts;

public record BlockHtmlInsert(string Content = "") : Insert, IHtmlContent
{
  public override string ToString()
  {
    return $"\n{Content}\n";
  }
}