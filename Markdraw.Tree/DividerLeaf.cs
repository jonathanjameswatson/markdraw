using Markdraw.Delta;

namespace Markdraw.Tree
{
  public class DividerLeaf : Leaf
  {

    public DividerLeaf(DividerInsert dividerInsert) : this(dividerInsert, null, 0) {}

    public DividerLeaf(DividerInsert dividerInsert, DeltaTree deltaTree, int i) : base(deltaTree, i)
    {
      CorrespondingInsert = dividerInsert;
    }
    public override DividerInsert CorrespondingInsert { get; }

    public override string ToString()
    {
      return @"<hr />";
    }
  }
}
