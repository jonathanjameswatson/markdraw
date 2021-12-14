using Markdraw.Delta.Operations.Inserts;

namespace Markdraw.Tree;

public class QuoteContainer : BlockContainer
{
  private QuoteContainer(DeltaTree? deltaTree, int i) : base(deltaTree, i) {}

  public QuoteContainer(List<TreeNode> elementsInside, DeltaTree? deltaTree = null, int i = 0) : base(elementsInside,
    deltaTree, i) {}

  protected override string Tag => "blockquote";

  public new static QuoteContainer CreateInstance(int depth, IEnumerable<Insert> document, DeltaTree? deltaTree, int i)
  {
    var container = new QuoteContainer(deltaTree, i);
    container.Initialise(depth, document, i);
    return container;
  }
}
