using Markdraw.Delta.Operations.Inserts.Inlines;

namespace Markdraw.Tree
{
  public class OuterInlineContainer : InlineContainer
  {
    private OuterInlineContainer(DeltaTree deltaTree = null, int i = 0, int header = 0, bool loose = true) : base(deltaTree, i)
    {
      Header = header;
      Loose = loose;
    }

    public OuterInlineContainer(List<TreeNode> elementsInside, DeltaTree deltaTree = null, int i = 0, int header = 0, bool loose = true) : base(elementsInside, deltaTree, i)
    {
      Header = header;
      Loose = loose;
    }

    public int Header { get; set; }
    public bool Loose { get; set; }

    protected override string Tag => Header == 0 ? "p" : $"h{Header}";
    protected override string StartingTag => Loose ? $"<{Tag}>" : "";
    protected override string EndingTag => Loose ? $"</{Tag}>" : "";

    public static OuterInlineContainer CreateInstance(int depth, IEnumerable<InlineInsert> document, DeltaTree deltaTree = null, int i = 0, int header = 0, bool loose = false)
    {
      var container = new OuterInlineContainer(deltaTree, i, header, loose);
      container.Initialise(depth, document, i);
      return container;
    }
  }
}
