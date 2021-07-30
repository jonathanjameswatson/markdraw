namespace Markdraw.Tree
{
  public abstract class TreeNode
  {
    protected TreeNode(DeltaTree deltaTree, int i)
    {
      ParentTree = deltaTree;
      I = i;
    }
    protected DeltaTree ParentTree { get; set; }
    public int I { get; }

    public int Length { get; protected set; }
  }
}
