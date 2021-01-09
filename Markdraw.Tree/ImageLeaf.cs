using Markdraw.Delta;

namespace Markdraw.Tree
{
  public class ImageLeaf : Leaf
  {
    private ImageInsert _correspondingInsert;
    public override ImageInsert CorrespondingInsert { get => _correspondingInsert; }

    public ImageLeaf(ImageInsert imageInsert)
    {
      _correspondingInsert = imageInsert;
    }
  }
}
