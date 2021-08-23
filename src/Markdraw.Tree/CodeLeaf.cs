using Markdraw.Delta.Operations.Inserts;

namespace Markdraw.Tree
{
  public class CodeLeaf : Leaf
  {

    public CodeLeaf(CodeInsert codeInsert, DeltaTree deltaTree = null, int i = 0) : base(deltaTree, i)
    {
      CorrespondingInsert = codeInsert;
    }
    protected override CodeInsert CorrespondingInsert { get; }

    public override string ToString()
    {
      var language = CorrespondingInsert.Tag is "" or null
        ? "none" : CorrespondingInsert.Tag;
      return $@"<pre><code>{CorrespondingInsert.Text}&#10;</code></pre>";
      // return $@"<pre class=""language-{language}"" contenteditable=""false""><code class=""language-{language}"">{CorrespondingInsert.Text}</code></pre>";
    }
  }
}
