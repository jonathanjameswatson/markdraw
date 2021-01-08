using Markdraw.Delta;

namespace Markdraw.Tree
{
  public class CodeLeaf : Leaf
  {
    public override CodeInsert CorrespondingInsert { get; }

    public CodeLeaf(CodeInsert codeInsert)
    {
      CorrespondingInsert = codeInsert;
    }
  }
}
