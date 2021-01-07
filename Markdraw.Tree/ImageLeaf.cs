using System;
using Markdraw.Delta;

namespace Markdraw.Tree
{
  public class ImageLeaf : Leaf
  {
    public override ImageInsert CorrespondingInsert;

    public ImageLeaf(ImageInsert imageInsert) {
      CorrespondingInsert = imageInsert;
    }
  }
}
