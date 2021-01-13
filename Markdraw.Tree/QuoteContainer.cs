using System.Collections.Generic;
using Markdraw.Delta;

namespace Markdraw.Tree
{
  public class QuoteContainer : Container
  {
    public override string OpeningTag { get => @"<blockquote>"; }
    public override string ClosingTag { get => @"</blockquote>"; }

    public QuoteContainer(int depth, Ops ops) : base(depth, ops) { }

    public QuoteContainer(int depth, Ops ops, DeltaTree deltaTree) : base(depth, ops, deltaTree) { }

    public QuoteContainer(List<TreeNode> elementsInside) : base(elementsInside) { }

    public QuoteContainer(List<TreeNode> elementsInside, DeltaTree deltaTree) : base(elementsInside, deltaTree) { }
  }
}
