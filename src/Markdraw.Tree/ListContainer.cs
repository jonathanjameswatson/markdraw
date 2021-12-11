using System.Collections.Generic;

namespace Markdraw.Tree
{
  public abstract class ListContainer : BlockContainer
  {
    protected ListContainer(DeltaTree deltaTree = null, int i = 0, bool loose = true) : base(deltaTree, i)
    {
      Loose = loose;
    }

    protected ListContainer(List<TreeNode> elementsInside, DeltaTree deltaTree = null, int i = 0, bool loose = true) : base(elementsInside, deltaTree, i)
    {
      Loose = loose;
    }

    public bool Loose { get; set; }
    protected override bool LooseInlines => Loose;

    protected sealed override bool WrapAllInside => true;
  }
}
