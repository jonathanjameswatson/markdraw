using Markdraw.Delta.Operations.Inserts.Inlines;

namespace Markdraw.Tree.TreeNodes.Containers.InlineContainers;

public class OuterInlineContainer : InlineBranchingContainer
{
  private OuterInlineContainer(DeltaTree? deltaTree = null, int i = 0, int header = 0, bool loose = true) :
    base(deltaTree, i)
  {
    Header = header;
    Loose = loose;
  }

  public OuterInlineContainer(List<TreeNode> elementsInside, DeltaTree? deltaTree = null, int i = 0, int header = 0,
    bool loose = true) : base(elementsInside, deltaTree, i)
  {
    Header = header;
    Loose = loose;
  }

  public int Header { get; set; }
  public bool Loose { get; set; }

  protected override string Tag => Header == 0 ? "p" : $"h{Header}";
  private bool ShowTags => Loose && !(Header == 0 && ElementsInside.Count == 0);
  protected override string StartingTag => ShowTags ? $"<{Tag}>" : "";
  protected override string EndingTag => ShowTags ? $"</{Tag}>" : "";

  public static OuterInlineContainer CreateInstance(int depth, IEnumerable<InlineInsert> document,
    DeltaTree? deltaTree = null, int i = 0, int header = 0, bool loose = true)
  {
    var container = new OuterInlineContainer(deltaTree, i, header, loose);
    container.Initialise(depth, document, i);
    return container;
  }
}
