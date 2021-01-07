using System;
using Markdraw.Delta;

namespace Markdraw.Tree
{
  public class DividerLeaf : Leaf
  {
    public override DividerInsert CorrespondingInsert;

    public DividerLeaf(DividerInsert dividerInsert) {
      CorrespondingInsert = dividerInsert;
    }
  }
}
