using System.Collections.Generic;
using Markdraw.Delta;
using Markdraw.Delta.Ops;

namespace Markdraw.Tree
{
  public class NumbersContainer : ListContainer
  {

    private NumbersContainer(DeltaTree deltaTree = null, int i = 0, int start = 0, bool loose = false) : base(deltaTree, i, loose)
    {
      Start = start;
    }
    public static NumbersContainer CreateInstance(int depth, Document document, DeltaTree deltaTree = null, int i = 0, int start = 0, bool loose = false) {
      var container = new NumbersContainer(deltaTree, i, start, loose);

      return Initialise(depth, document, i, container);
    }
    public NumbersContainer(List<TreeNode> elementsInside, DeltaTree deltaTree = null, int i = 0, bool loose = false) : base(elementsInside, deltaTree, i, loose) {}

    public int Start { get; set; } = 0;
    protected override string Tag => "ol";
    protected override string StartingTag => Start > 1 ? $@"<ol start=""{Start}"">" : "<ol>";
    protected override bool WrapAllInside => true;
  }
}
