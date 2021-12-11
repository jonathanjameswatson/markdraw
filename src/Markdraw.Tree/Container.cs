using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Markdraw.Tree
{
  public abstract class Container : TreeNode, IEnumerable<TreeNode>
  {
    protected Container(DeltaTree deltaTree = null, int i = 0) : base(deltaTree, i) {}

    protected Container(List<TreeNode> elementsInside, DeltaTree deltaTree = null, int i = 0) : base(deltaTree, i)
    {
      ElementsInside = elementsInside;
    }

    protected virtual string Tag => "div";
    protected virtual string StartingTag => $"<{Tag}>";
    protected virtual string EndingTag => $"</{Tag}>";
    protected virtual bool WrapAllInside => false;
    protected List<TreeNode> ElementsInside { get; } = new();

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public IEnumerator<TreeNode> GetEnumerator()
    {
      return ElementsInside.GetEnumerator();
    }

    public override string ToString()
    {
      var stringBuilder = new StringBuilder();

      stringBuilder.Append(StartingTag);

      foreach (var child in ElementsInside)
      {
        if (WrapAllInside)
        {
          stringBuilder.Append(@"<li>");
        }

        stringBuilder.Append(child);

        if (WrapAllInside)
        {
          stringBuilder.Append(@"</li>");
        }
      }

      stringBuilder.Append(EndingTag);

      return stringBuilder.ToString();
    }
  }
}
