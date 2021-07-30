using Markdraw.Delta;

namespace Markdraw.Tree
{
  public class ImageLeaf : Leaf
  {

    public ImageLeaf(ImageInsert imageInsert, DeltaTree deltaTree = null, int i = 0) : base(deltaTree, i)
    {
      CorrespondingInsert = imageInsert;
    }
    protected override ImageInsert CorrespondingInsert { get; }

    public override string ToString()
    {
      return $@"<img src=""{CorrespondingInsert.Url}"" alt=""{CorrespondingInsert.Alt}"" />";
    }
  }
}
