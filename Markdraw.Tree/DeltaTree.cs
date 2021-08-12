using Markdraw.Delta;
using Markdraw.Delta.Ops;
using Markdraw.MarkdownToDelta;

namespace Markdraw.Tree
{
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
        Root = Container.CreateInstance(0, Delta, this);
      }
    }
    public Container Root { get; private set; }

    public bool AddSpans { get; set; }

    public void SetWithMarkdown(string markdown)
    {
      Delta = MarkdownToDeltaConverter.Parse(markdown);
    }

    public static Container Parse(string markdown)
    {
      var ops = MarkdownToDeltaConverter.Parse(markdown);
      return Container.CreateInstance(0, ops);
    }

    public static Container Parse(Document document)
    {
      return Container.CreateInstance(0, document);
    }
  }
}
