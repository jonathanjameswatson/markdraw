using Markdraw.Delta.Indents;
using Markdraw.Delta.Operations.Inserts;

namespace Markdraw.Tree.TreeNodes.Containers.BlockContainers;

public abstract class ListContainer : BlockContainer
{
  protected ListContainer(DeltaTree? deltaTree = null, int i = 0, bool loose = true) : base(deltaTree, i)
  {
    Loose = loose;
  }

  protected ListContainer(List<TreeNode> elementsInside, DeltaTree? deltaTree = null, int i = 0, bool loose = true) :
    base(elementsInside, deltaTree, i)
  {
    Loose = loose;
  }

  public bool Loose { get; set; }
  protected override bool LooseInlines => Loose;

  protected override Indent NextBranchMarker(Indent indent)
  {
    return Indent.Continue;
  }

  protected override BlockContainer CreateChildContainer(Indent indent, IEnumerable<Insert> document, int depth, int i)
  {
    return ListItemContainer.CreateInstance(depth, document, ParentTree, i, Loose);
  }
}
