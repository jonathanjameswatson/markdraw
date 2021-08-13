using System.Collections.Generic;
using Markdraw.Delta;
using Markdraw.Delta.OperationSequences;

namespace Markdraw.Tree
{
  public class BulletsContainer : ListContainer
  {

    private BulletsContainer(DeltaTree deltaTree = null, int i = 0, bool loose = false) : base(deltaTree, i, loose) {}
    public static BulletsContainer CreateInstance(int depth, Document document, DeltaTree deltaTree = null, int i = 0, bool loose = false) {
      var container = new BulletsContainer(deltaTree, i, loose);

      return Initialise(depth, document, i, container);
    }
    public BulletsContainer(List<TreeNode> elementsInside, DeltaTree deltaTree = null, int i = 0, bool loose = false) : base(elementsInside, deltaTree, i, loose) {}

    protected override string Tag => "ul";
    protected override bool WrapAllInside => true;
  }
}
