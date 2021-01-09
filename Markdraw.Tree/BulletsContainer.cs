using System.Collections.Generic;
using Markdraw.Delta;

namespace Markdraw.Tree
{
  public class BulletsContainer : Container
  {
    public BulletsContainer(int depth, Ops ops) : base(depth, ops) { }

    public BulletsContainer(List<TreeNode> elementsInside) : base(elementsInside) { }
  }
}
