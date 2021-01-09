using Markdraw.Delta;

namespace Markdraw.Tree
{
  public class CodeLeaf : Leaf
  {
    public override CodeInsert CorrespondingInsert { get; }

    public CodeLeaf(CodeInsert codeInsert)
    {
      CorrespondingInsert = codeInsert;
    }

    public override string ToString()
    {
      if (CorrespondingInsert.Tag == "" || CorrespondingInsert.Tag is null)
      {
        return $@"<pre><code>{CorrespondingInsert.Text}</code></pre>";
      }
      return $@"<pre><code class=""language-{CorrespondingInsert.Tag}"">{CorrespondingInsert.Text}</code></pre>";
    }
  }
}
