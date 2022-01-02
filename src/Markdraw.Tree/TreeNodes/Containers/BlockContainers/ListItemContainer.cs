using Markdraw.Delta.Operations.Inserts;

namespace Markdraw.Tree.TreeNodes.Containers.BlockContainers;

public class ListItemContainer : BlockContainer
{
  private ListItemContainer(DeltaTree? deltaTree, int i, bool loose = true) : base(deltaTree, i)
  {
    Loose = loose;
  }

  public ListItemContainer(List<TreeNode> elementsInside, DeltaTree? deltaTree = null, int i = 0, bool loose = true) :
    base(elementsInside, deltaTree, i)
  {
    Loose = loose;
  }

  protected override string Tag => "li";

  public bool Loose { get; set; }
  protected override bool LooseInlines => Loose;

  public static ListItemContainer CreateInstance(int depth, IEnumerable<IInsert> document, DeltaTree? deltaTree = null,
    int i = 0, bool loose = true)
  {
    var container = new ListItemContainer(deltaTree, i, loose);
    container.Initialise(depth, document, i);
    return container;
  }
}
