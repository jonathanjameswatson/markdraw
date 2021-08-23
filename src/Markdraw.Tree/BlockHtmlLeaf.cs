using Markdraw.Delta.Operations.Inserts;

namespace Markdraw.Tree
{
  public class BlockHtmlLeaf : Leaf
  {

    public BlockHtmlLeaf(BlockHtmlInsert blockHtmlInsert, DeltaTree deltaTree = null, int i = 0) : base(deltaTree, i)
    {
      CorrespondingInsert = blockHtmlInsert;
    }

    protected override BlockHtmlInsert CorrespondingInsert { get; }

    public override string ToString()
    {
      return CorrespondingInsert.Content;
    }

  }
}
