using System.Text;
using Markdraw.Delta.Operations.Inserts;
using Markdraw.Helpers;

namespace Markdraw.Tree.TreeNodes.Leaves;

public class CodeLeaf : Leaf
{
  public CodeLeaf(CodeInsert codeInsert, DeltaTree? deltaTree = null, int i = 0) : base(deltaTree, i)
  {
    CorrespondingInsert = codeInsert;
  }

  protected override CodeInsert CorrespondingInsert { get; }

  public override string ToString()
  {
    // var language = CorrespondingInsert.Tag is "" or null
    //   ? "none" : CorrespondingInsert.Tag;
    var stringBuilder = new StringBuilder("<pre><code");
    if (!CorrespondingInsert.Tag.Equals("", StringComparison.Ordinal))
    {
      stringBuilder.Append(@" class=""language-");
      stringBuilder.Append(CorrespondingInsert.Tag);
      stringBuilder.Append('"');
    }
    stringBuilder.Append('>');
    var text = EscapeHelpers.Escape(CorrespondingInsert.Text);
    stringBuilder.Append(text);
    if (!text.Equals("", StringComparison.Ordinal))
    {
      stringBuilder.Append("&#10;");
    }
    stringBuilder.Append("</code></pre>");
    return stringBuilder.ToString();
    // return $@"<pre class=""language-{language}"" contenteditable=""false""><code class=""language-{language}"">{CorrespondingInsert.Text}</code></pre>";
  }
}
