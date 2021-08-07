using System.Collections.Generic;
using Markdraw.Delta;

namespace Markdraw.Tree
{
  public abstract class ListContainer : Container
  {

    protected ListContainer(DeltaTree deltaTree = null, int i = 0, bool loose = true) : base(deltaTree, i)
    {
      Loose = loose;
    }

    protected ListContainer(List<TreeNode> elementsInside, DeltaTree deltaTree = null, int i = 0, bool loose = true) : base(elementsInside, deltaTree, i)
    {
      Loose = loose;
    }
  }
}
