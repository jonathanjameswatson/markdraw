using System.Collections.Generic;
using Markdraw.Delta;

namespace Markdraw.Tree
{
  public class NumbersContainer : ListContainer
  {

    private NumbersContainer(DeltaTree deltaTree = null, int i = 0, bool loose = false) : base(deltaTree, i, loose) {}
    public static NumbersContainer CreateInstance(int depth, Ops ops, DeltaTree deltaTree = null, int i = 0, bool loose = false) {
      var container = new NumbersContainer(deltaTree, i, loose);

      return Initialise(depth, ops, i, container);
    }
    public NumbersContainer(List<TreeNode> elementsInside, DeltaTree deltaTree = null, int i = 0, bool loose = false) : base(elementsInside, deltaTree, i, loose) {}

    protected override string Tag => "ol";
    protected override bool WrapAllInside => true;
  }
}
