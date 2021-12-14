using Markdraw.Delta.OperationSequences;
using Markdraw.MarkdownToDelta;

namespace Markdraw.Tree;

public class DeltaTree
{
  private Document _delta;

  public DeltaTree(string markdown = "")
  {
    Delta = MarkdownToDeltaConverter.Parse(markdown);
    AddSpans = true;
  }

  public DeltaTree(Document document)
  {
    Delta = document;
    AddSpans = true;
  }

  public Document Delta
  {
    get => _delta;
    set
    {
      _delta = value;
      Root = BlockContainer.CreateInstance(0, Delta, this);
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