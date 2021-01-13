using Markdraw.Delta;

namespace Markdraw.Tree
{
  public class ImageLeaf : Leaf
  {
    private ImageInsert _correspondingInsert;
    public override ImageInsert CorrespondingInsert { get => _correspondingInsert; }

    public ImageLeaf(ImageInsert imageInsert) : this(imageInsert, null, 0) { }

    public ImageLeaf(ImageInsert imageInsert, DeltaTree deltaTree, int i) : base(deltaTree, i)
    {
      _correspondingInsert = imageInsert;
    }

    public override string ToString()
    {
      if (ParentTree is not null && ParentTree.HasI)
      {
        return $@"<img src=""{CorrespondingInsert.Url}"" alt=""{CorrespondingInsert.Alt}"" i=""{I}"" />"; ;
      }
      return $@"<img src=""{CorrespondingInsert.Url}"" alt=""{CorrespondingInsert.Alt}"" />";
    }
  }
}
