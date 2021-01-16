using System.Collections.Generic;
using Markdraw.Delta;

namespace Markdraw.Tree
{
  public class BulletsContainer : Container
  {
    public override string OpeningTag
    {
      get => ParentTree is not null && ParentTree.HasI ? $"<ul i={I}>" : "<ul>";
    }
    public override string ClosingTag { get => @"</ul>"; }
    public override bool WrapAllInside { get => true; }

    public BulletsContainer(int depth, Ops ops) : base(depth, ops) { }

    public BulletsContainer(int depth, Ops ops, DeltaTree deltaTree, int i) : base(depth, ops, deltaTree, i) { }

    public BulletsContainer(List<TreeNode> elementsInside) : base(elementsInside) { }

    public BulletsContainer(List<TreeNode> elementsInside, DeltaTree deltaTree, int i) : base(elementsInside, deltaTree, i) { }
  }
}
