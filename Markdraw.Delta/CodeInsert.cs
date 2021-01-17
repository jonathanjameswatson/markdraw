namespace Markdraw.Delta
{
  public class CodeInsert : EmbedInsert
  {
    public string Text { get; set; }
    public string Tag { get; set; }

    public CodeInsert(string text = "", string tag = "")
    {
      Text = text;
      Tag = tag;
    }

    public override bool Equals(object obj)
    {
      return obj is CodeInsert x && x.Text == Text && x.Tag == Tag;
    }

    public override int GetHashCode()
    {
      return (Text, Tag).GetHashCode();
    }

    public override string ToString()
    {
      return $"\n```{Tag}\n{Text}\n```\n";
    }
  }
}