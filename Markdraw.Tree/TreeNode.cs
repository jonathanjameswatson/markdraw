namespace Markdraw.Tree
{
  public abstract class TreeNode
  {
    protected int _i;
    protected int _length;

    protected TreeNode(DeltaTree deltaTree, int i)
    {
      ParentTree = deltaTree;
      _i = i;
    }
    public DeltaTree ParentTree { get; set; }
    public int I => _i;
    public int Length => _length;
  }
}
