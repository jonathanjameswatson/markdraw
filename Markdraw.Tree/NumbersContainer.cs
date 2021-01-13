using System.Collections.Generic;
using Markdraw.Delta;

namespace Markdraw.Tree
{
  public class NumbersContainer : Container
  {
    public override string OpeningTag { get => @"<ol>"; }
    public override string InsideOpeningTag { get => @"<li>"; }
    public override string InsideClosingTag { get => @"</li>"; }
    public override string ClosingTag { get => @"</ol>"; }
    public override bool WrapAllInside { get => true; }

    public NumbersContainer(int depth, Ops ops) : base(depth, ops) { }

    public NumbersContainer(int depth, Ops ops, DeltaTree deltaTree) : base(depth, ops, deltaTree) { }

    public NumbersContainer(List<TreeNode> elementsInside) : base(elementsInside) { }

    public NumbersContainer(List<TreeNode> elementsInside, DeltaTree deltaTree) : base(elementsInside, deltaTree) { }
  }
}
