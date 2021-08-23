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
      var text = CorrespondingInsert.Text;
      if (text != "")
      {
        text += @"&#10;";
      }
      return $@"<pre><code>{text}</code></pre>";
      // return $@"<pre class=""language-{language}"" contenteditable=""false""><code class=""language-{language}"">{CorrespondingInsert.Text}</code></pre>";
    }
  }
}
