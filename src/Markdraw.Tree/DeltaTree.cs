using Markdraw.Delta.OperationSequences;
using Markdraw.MarkdownToDelta;
using Markdraw.Tree.TreeNodes.Containers.BlockContainers;

namespace Markdraw.Tree;

public class DeltaTree
{
  private Document _delta;
  public DeltaTree(string markdown = "") : this(MarkdownToDeltaConverter.Parse(markdown)) {}

  public DeltaTree(Document document)
  {
    _delta = document;
    Root = BlockContainer.CreateInstance(0, _delta, this);
    AddSpans = true;
  }
  public Document Delta
  {
    get => _delta;
    set
    {
      _delta = value;
      Root = BlockContainer.CreateInstance(0, _delta, this);
    }
  }

  public BlockContainer Root { get; private set; }

  public bool AddSpans { get; set; }

  public void SetWithMarkdown(string markdown)
  {
    Delta = MarkdownToDeltaConverter.Parse(markdown);
  }

  public static BlockContainer Parse(string markdown)
  {
    var ops = MarkdownToDeltaConverter.Parse(markdown);
    return BlockContainer.CreateInstance(0, ops);
  }

  public static BlockContainer Parse(Document document)
  {
    return BlockContainer.CreateInstance(0, document);
  }
}
