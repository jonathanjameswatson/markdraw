using System;
using Markdraw.Delta;

namespace Markdraw.Tree
{
  public class CodeLeaf : Leaf
  {
    public override CodeInsert CorrespondingInsert;

    public CodeLeaf(CodeInsert codeInsert) {
      CorrespondingInsert = codeInsert;
    }
  }
}
