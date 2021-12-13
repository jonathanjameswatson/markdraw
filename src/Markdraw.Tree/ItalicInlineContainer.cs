using Markdraw.Delta.Operations.Inserts.Inlines;

namespace Markdraw.Tree
{
  public class ItalicInlineContainer : InlineContainer
  {
    private ItalicInlineContainer(DeltaTree deltaTree = null, int i = 0) : base(deltaTree, i) {}

    public ItalicInlineContainer(List<TreeNode> elementsInside, DeltaTree deltaTree = null, int i = 0) : base(elementsInside, deltaTree, i) {}

    protected override string Tag => "em";

    public static ItalicInlineContainer CreateInstance(int depth, IEnumerable<InlineInsert> document, DeltaTree deltaTree = null, int i = 0)
    {
      var container = new ItalicInlineContainer(deltaTree, i);
      container.Initialise(depth, document, i);
      return container;
    }
  }
}
