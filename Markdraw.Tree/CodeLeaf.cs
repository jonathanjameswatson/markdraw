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
      string language = (CorrespondingInsert.Tag == "" || CorrespondingInsert.Tag is null)
        ? "none" : CorrespondingInsert.Tag;
      return $@"<pre class=""language-{language}"" contenteditable=""false""><code class=""language-{language}"">{CorrespondingInsert.Text}</code></pre>";
    }
  }
}
