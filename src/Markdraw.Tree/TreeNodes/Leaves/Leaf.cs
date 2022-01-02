using Markdraw.Delta.Operations.Inserts;

namespace Markdraw.Tree.TreeNodes.Leaves;

public abstract class Leaf : TreeNode
{
  protected Leaf(DeltaTree? deltaTree, int i) : base(deltaTree, i) {}
  protected abstract IInsert CorrespondingInsert { get; }

  public override int Length
  {
    get => 1;
    protected set => throw new InvalidOperationException("Length of a leaf cannot be set.");
  }

  public override bool Equals(object? obj)
  {
    return obj is Leaf x && x.CorrespondingInsert.Equals(CorrespondingInsert);
  }

  public override int GetHashCode()
  {
    return CorrespondingInsert.GetHashCode();
  }
}
