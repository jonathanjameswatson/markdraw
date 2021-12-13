using Markdraw.Delta.Operations.Inserts.Inlines;

namespace Markdraw.Tree
{
  public class CodeInlineContainer : Container
  {
    private CodeInlineContainer(DeltaTree deltaTree = null, int i = 0) : base(deltaTree, i) {}

    public CodeInlineContainer(List<TreeNode> elementsInside, DeltaTree deltaTree = null, int i = 0) : base(
      elementsInside, deltaTree, i) {}

    protected override string Tag => "code";

    public static CodeInlineContainer CreateInstance(IEnumerable<InlineInsert> document, DeltaTree deltaTree = null,
      int i = 0)
    {
      var container = new CodeInlineContainer(deltaTree, i);
      var length = 0;
      var newI = i;

      foreach (var inlineInsert in document)
      {
        length += inlineInsert.Length;
        newI += inlineInsert.Length;
        container.ElementsInside.Add(new InlineLeaf(inlineInsert, deltaTree, newI));
      }

      container.Length = length;

      return container;
    }
  }
}
