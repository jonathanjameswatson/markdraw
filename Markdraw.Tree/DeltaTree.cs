using Markdraw.Delta;
using Markdraw.MarkdownToDelta;

namespace Markdraw.Tree
{
  public class DeltaTree
  {
    private Ops _delta;

    public DeltaTree(string markdown = "")
    {
      Delta = MarkdownToDeltaConverter.Parse(markdown);
      AddSpans = true;
    }

    public DeltaTree(Ops ops)
    {
      Delta = ops;
      AddSpans = true;
    }
    public Ops Delta
    {
      get => _delta;
      set
      {
        _delta = value;
        Root = new Container(0, Delta, this);
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
      return new Container(0, ops);
    }

    public static Container Parse(Ops ops)
    {
      return new Container(0, ops);
    }
  }
}
