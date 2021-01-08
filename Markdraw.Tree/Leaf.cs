using Markdraw.Delta;

namespace Markdraw.Tree
{
  public abstract class Leaf : TreeNode
  {
    public abstract Insert CorrespondingInsert { get; }
  }
}
