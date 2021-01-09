using System.Collections.Generic;
using Markdraw.Delta;

namespace Markdraw.Tree
{
  public class QuoteContainer : Container
  {
    public QuoteContainer(int depth, Ops ops) : base(depth, ops) { }

    public QuoteContainer(List<TreeNode> elementsInside) : base(elementsInside) { }
  }
}
