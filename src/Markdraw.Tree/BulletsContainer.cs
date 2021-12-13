using Markdraw.Delta.Operations.Inserts;

namespace Markdraw.Tree
{
  public class BulletsContainer : ListContainer
  {
    private BulletsContainer(DeltaTree deltaTree = null, int i = 0, bool loose = false) : base(deltaTree, i, loose) {}

    public BulletsContainer(List<TreeNode> elementsInside, DeltaTree deltaTree = null, int i = 0, bool loose = false) :
      base(elementsInside, deltaTree, i, loose) {}

    protected override string Tag => "ul";

    public static BulletsContainer CreateInstance(int depth, IEnumerable<Insert> document, DeltaTree deltaTree = null,
      int i = 0, bool loose = false)
    {
      var container = new BulletsContainer(deltaTree, i, loose);
      container.Initialise(depth, document, i);
      return container;
    }
  }
}
