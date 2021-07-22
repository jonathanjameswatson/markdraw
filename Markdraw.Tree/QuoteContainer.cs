using System.Collections.Generic;
using Markdraw.Delta;

namespace Markdraw.Tree
{
  public class QuoteContainer : Container
  {
    public override string Tag { get => "blockquote"; }

    public QuoteContainer(int depth, Ops ops) : base(depth, ops) { }

    public QuoteContainer(int depth, Ops ops, DeltaTree deltaTree, int i) : base(depth, ops, deltaTree, i) { }

    public QuoteContainer(List<TreeNode> elementsInside) : base(elementsInside) { }

    public QuoteContainer(List<TreeNode> elementsInside, DeltaTree deltaTree, int i) : base(elementsInside, deltaTree, i) { }
  }
}
