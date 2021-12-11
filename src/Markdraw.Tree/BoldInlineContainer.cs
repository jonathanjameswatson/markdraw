using System.Collections.Generic;
using Markdraw.Delta.Operations.Inserts.Inlines;

namespace Markdraw.Tree
{
  public class BoldInlineContainer : InlineContainer
  {
    private BoldInlineContainer(DeltaTree deltaTree = null, int i = 0) : base(deltaTree, i) {}

    public BoldInlineContainer(List<TreeNode> elementsInside, DeltaTree deltaTree = null, int i = 0) : base(elementsInside, deltaTree, i) {}

    protected override string Tag => "strong";

    public static BoldInlineContainer CreateInstance(int depth, IEnumerable<InlineInsert> document, DeltaTree deltaTree = null, int i = 0)
    {
      var container = new BoldInlineContainer(deltaTree, i);
      container.Initialise(depth, document, i);
      return container;
    }
  }
}
