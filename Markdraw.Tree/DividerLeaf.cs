using Markdraw.Delta;

namespace Markdraw.Tree
{
  public class DividerLeaf : Leaf
  {
    private DividerInsert _correspondingInsert;
    public override DividerInsert CorrespondingInsert { get => _correspondingInsert; }

    public DividerLeaf(DividerInsert dividerInsert) : this(dividerInsert, null) { }

    public DividerLeaf(DividerInsert dividerInsert, DeltaTree deltaTree) : base(deltaTree)
    {
      _correspondingInsert = dividerInsert;
    }

    public override string ToString()
    {
      return @"<hr />";
    }
  }
}
