using System.Collections.Generic;
using Markdraw.Delta;

namespace Markdraw.Tree
{
  public class QuoteContainer : Container
  {

    private QuoteContainer(DeltaTree deltaTree, int i) : base(deltaTree, i) {}
    public new static QuoteContainer CreateInstance(int depth, Ops ops, DeltaTree deltaTree, int i) {
      var container = new QuoteContainer(deltaTree, i);

      return Initialise(depth, ops, i, container);
    }
    public QuoteContainer(List<TreeNode> elementsInside, DeltaTree deltaTree = null, int i = 0) : base(elementsInside, deltaTree, i) {}

    protected override string Tag => "blockquote";
  }
}
