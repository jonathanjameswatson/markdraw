namespace Markdraw.Delta.Operations.Inserts
{
  public record CodeInsert(string Text = "", string Tag = "") : Insert
  {

    public override string ToString()
    {
      return $"\n```{Tag}\n{Text}\n```\n";
    }
  }
}
