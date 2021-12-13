using Markdraw.Delta.Operations.Inserts;

namespace Markdraw.Tree
{
  public class NumbersContainer : ListContainer
  {
    private NumbersContainer(DeltaTree deltaTree = null, int i = 0, int start = 0, bool loose = false) : base(deltaTree,
      i, loose)
    {
      Start = start;
    }

    public NumbersContainer(List<TreeNode> elementsInside, DeltaTree deltaTree = null, int i = 0, int start = 0,
      bool loose = false) : base(elementsInside, deltaTree, i, loose)
    {
      Start = start;
    }

    public int Start { get; set; }
    protected override string Tag => "ol";
    protected override string StartingTag => Start > 1 ? $@"<ol start=""{Start}"">" : "<ol>";

    public static NumbersContainer CreateInstance(int depth, IEnumerable<Insert> document, DeltaTree deltaTree = null,
      int i = 0, int start = 0, bool loose = false)
    {
      var container = new NumbersContainer(deltaTree, i, start, loose);
      container.Initialise(depth, document, i);
      return container;
    }
  }
}
