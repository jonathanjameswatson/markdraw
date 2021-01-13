using Markdraw.Delta;

namespace Markdraw.Tree
{
  public abstract class Leaf : TreeNode
  {
    public abstract Insert CorrespondingInsert { get; }

    protected Leaf(DeltaTree deltaTree, int i) : base(deltaTree, i)
    {
      _length = 1;
    }

    public override bool Equals(object obj)
    {
      return obj is Leaf x && x.CorrespondingInsert.Equals(this.CorrespondingInsert);
    }

    public override int GetHashCode()
    {
      return CorrespondingInsert.GetHashCode();
    }
  }
}
