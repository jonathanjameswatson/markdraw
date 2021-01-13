using System.Collections.Generic;
using Markdraw.Delta;

namespace Markdraw.Tree
{
  public class BulletsContainer : Container
  {
    public override string OpeningTag { get => @"<ul>"; }
    public override string InsideOpeningTag { get => @"<li>"; }
    public override string InsideClosingTag { get => @"</li>"; }
    public override string ClosingTag { get => @"</ul>"; }
    public override bool WrapAllInside { get => true; }

    public BulletsContainer(int depth, Ops ops) : base(depth, ops) { }

    public BulletsContainer(int depth, Ops ops, DeltaTree deltaTree) : base(depth, ops, deltaTree) { }

    public BulletsContainer(List<TreeNode> elementsInside) : base(elementsInside) { }

    public BulletsContainer(List<TreeNode> elementsInside, DeltaTree deltaTree) : base(elementsInside, deltaTree) { }
  }
}
