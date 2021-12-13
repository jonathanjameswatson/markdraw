using Markdraw.Delta.Operations.Inserts;

namespace Markdraw.Tree
{
  public abstract class Leaf : TreeNode
  {
    protected Leaf(DeltaTree deltaTree, int i) : base(deltaTree, i)
    {
      Length = 1;
    }
    protected abstract Insert CorrespondingInsert { get; }

    public override bool Equals(object obj)
    {
      return obj is Leaf x && x.CorrespondingInsert.Equals(CorrespondingInsert);
    }

    public override int GetHashCode()
    {
      return CorrespondingInsert.GetHashCode();
    }
  }
}
