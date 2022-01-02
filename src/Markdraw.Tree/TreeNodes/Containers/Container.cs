using System.Collections;
using System.Text;

namespace Markdraw.Tree.TreeNodes.Containers;

public abstract class Container : TreeNode, IEnumerable<TreeNode>
{
  protected Container(DeltaTree? deltaTree = null, int i = 0) : base(deltaTree, i) {}

  protected Container(List<TreeNode> elementsInside, DeltaTree? deltaTree = null, int i = 0) : base(deltaTree, i)
  {
    ElementsInside = elementsInside;
  }

  protected virtual string Tag => "div";
  protected virtual string StartingTag => $"<{Tag}>";
  protected virtual string EndingTag => $"</{Tag}>";

  public List<TreeNode> ElementsInside { get; } = new();

  public string InnerHtml => string.Concat(this);

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
      stringBuilder.Append(child);
    }

    stringBuilder.Append(EndingTag);

    return stringBuilder.ToString();
  }
}
