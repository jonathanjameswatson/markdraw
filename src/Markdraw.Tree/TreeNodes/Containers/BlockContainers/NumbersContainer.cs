using Markdraw.Delta.Operations.Inserts;

namespace Markdraw.Tree.TreeNodes.Containers.BlockContainers;

public class NumbersContainer : ListContainer
{
  private NumbersContainer(DeltaTree? deltaTree = null, int i = 0, int start = 1, bool loose = true) : base(deltaTree,
    i, loose)
  {
    Start = start;
  }

  public NumbersContainer(List<TreeNode> elementsInside, DeltaTree? deltaTree = null, int i = 0, int start = 1,
    bool loose = true) : base(elementsInside, deltaTree, i, loose)
  {
    Start = start;
  }

  public int Start { get; set; }
  protected override string Tag => "ol";
  protected override string StartingTag => Start != 1 ? $@"<ol start=""{Start}"">" : "<ol>";

  public static NumbersContainer CreateInstance(int depth, IEnumerable<IInsert> document, DeltaTree? deltaTree = null,
    int i = 0, int start = 1, bool loose = true)
  {
    var container = new NumbersContainer(deltaTree, i, start, loose);
    container.Initialise(depth, document, i);
    return container;
  }
}
