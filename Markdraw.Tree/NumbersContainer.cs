using System.Collections.Generic;
using Markdraw.Delta;

namespace Markdraw.Tree
{
  public class NumbersContainer : Container
  {
    public NumbersContainer(int depth, Ops ops) : base(depth, ops) { }

    public NumbersContainer(List<TreeNode> elementsInside) : base(elementsInside) { }
  }
}
