namespace Markdraw.Delta.Operations.Inserts;

public record CodeInsert(string Text = "", string Tag = "") : IInsert
{
  public override string ToString()
  {
    return $"\n```{Tag}\n{Text}\n```\n";
  }
}