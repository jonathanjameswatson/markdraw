using Markdraw.Delta;

namespace Markdraw.Tree
{
  public class ImageLeaf : Leaf
  {

    public ImageLeaf(ImageInsert imageInsert) : this(imageInsert, null, 0) {}

    public ImageLeaf(ImageInsert imageInsert, DeltaTree deltaTree, int i) : base(deltaTree, i)
    {
      CorrespondingInsert = imageInsert;
    }
    public override ImageInsert CorrespondingInsert { get; }

    public override string ToString()
    {
      return $@"<img src=""{CorrespondingInsert.Url}"" alt=""{CorrespondingInsert.Alt}"" />";
    }
  }
}
