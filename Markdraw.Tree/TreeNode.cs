namespace Markdraw.Tree
{
  public abstract class TreeNode
  {
    public DeltaTree ParentTree { get; }

    protected TreeNode(DeltaTree deltaTree)
    {
      ParentTree = deltaTree;
    }
  }
}
