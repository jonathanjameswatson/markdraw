using Markdraw.Delta.Operations.Inserts;

namespace Markdraw.Tree.TreeNodes.Containers.BlockContainers;

public class BulletsContainer : ListContainer
{
  private BulletsContainer(DeltaTree? deltaTree = null, int i = 0, bool loose = true) : base(deltaTree, i, loose) {}

  public BulletsContainer(List<TreeNode> elementsInside, DeltaTree? deltaTree = null, int i = 0, bool loose = true) :
    base(elementsInside, deltaTree, i, loose) {}

  protected override string Tag => "ul";

  public static BulletsContainer CreateInstance(int depth, IEnumerable<IInsert> document, DeltaTree? deltaTree = null,
    int i = 0, bool loose = true)
  {
    var container = new BulletsContainer(deltaTree, i, loose);
    container.Initialise(depth, document, i);
    return container;
  }
}
