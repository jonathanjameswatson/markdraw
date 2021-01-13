using Markdraw.Delta;

namespace Markdraw.Tree
{
  public class ImageLeaf : Leaf
  {
    private ImageInsert _correspondingInsert;
    public override ImageInsert CorrespondingInsert { get => _correspondingInsert; }

    public ImageLeaf(ImageInsert imageInsert) : this(imageInsert, null) { }

    public ImageLeaf(ImageInsert imageInsert, DeltaTree deltaTree) : base(deltaTree)
    {
      _correspondingInsert = imageInsert;
    }

    public override string ToString()
    {
      return $@"<img src=""{CorrespondingInsert.Url}"" alt=""{CorrespondingInsert.Alt}"" />";
    }
  }
}
