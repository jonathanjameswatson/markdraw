using Markdraw.Delta.Operations.Inserts;

namespace Markdraw.Tree
{
  public class DividerLeaf : Leaf
  {

    public DividerLeaf(DividerInsert dividerInsert, DeltaTree deltaTree = null, int i = 0) : base(deltaTree, i)
    {
      CorrespondingInsert = dividerInsert;
    }
    protected override DividerInsert CorrespondingInsert { get; }

    public override string ToString()
    {
      return @"<hr />";
    }
  }
}
