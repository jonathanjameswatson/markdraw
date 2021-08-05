using Markdraw.Delta.Operations.Inserts;

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
      var altString = CorrespondingInsert.Alt == "" ? "" : $@"alt=""{CorrespondingInsert.Alt}"" ";
      var titleString = CorrespondingInsert.Title == "" ? "" : $@"title=""{CorrespondingInsert.Title}"" ";
      return $@"<img src=""{CorrespondingInsert.Url}"" {altString}{titleString}/>";
    }
  }
}
