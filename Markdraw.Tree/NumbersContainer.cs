using System.Collections.Generic;
using Markdraw.Delta;

namespace Markdraw.Tree
{
  public class NumbersContainer : Container
  {
    public override string Tag { get => "ol"; }
    public override bool WrapAllInside { get => true; }

    public NumbersContainer(int depth, Ops ops) : base(depth, ops) { }

    public NumbersContainer(int depth, Ops ops, DeltaTree deltaTree, int i) : base(depth, ops, deltaTree, i) { }

    public NumbersContainer(List<TreeNode> elementsInside) : base(elementsInside) { }

    public NumbersContainer(List<TreeNode> elementsInside, DeltaTree deltaTree, int i) : base(elementsInside, deltaTree, i) { }
  }
}
