namespace Markdraw.Tree
{
  public abstract class TreeNode
  {
    public DeltaTree ParentTree { get; set; }
    protected int _i;
    public int I { get => _i; }
    protected int _length;
    public int Length { get => _length; }

    protected TreeNode(DeltaTree deltaTree, int i)
    {
      ParentTree = deltaTree;
      _i = i;
    }
  }
}
