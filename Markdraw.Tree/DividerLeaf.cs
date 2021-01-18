using Markdraw.Delta;

namespace Markdraw.Tree
{
  public class DividerLeaf : Leaf
  {
    private DividerInsert _correspondingInsert;
    public override DividerInsert CorrespondingInsert { get => _correspondingInsert; }

    public DividerLeaf(DividerInsert dividerInsert) : this(dividerInsert, null, 0) { }

    public DividerLeaf(DividerInsert dividerInsert, DeltaTree deltaTree, int i) : base(deltaTree, i)
    {
      _correspondingInsert = dividerInsert;
    }

    public override string ToString()
    {
      if (ParentTree is not null && ParentTree.HasI)
      {
        return $@"<hr i=""{I}"" contenteditable=""false"" />"; ;
      }
      return @"<hr />";
    }
  }
}
