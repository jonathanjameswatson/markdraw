using System.Text;
using Markdraw.Delta.Operations.Inserts.Inlines;
using Markdraw.Helpers;

namespace Markdraw.Tree.TreeNodes.Containers.InlineContainers;

public class LinkInlineContainer : InlineBranchingContainer
{
  private LinkInlineContainer(DeltaTree? deltaTree = null, int i = 0, string url = "", string title = "") :
    base(deltaTree, i)
  {
    Url = url;
    Title = title;
  }

  public LinkInlineContainer(List<TreeNode> elementsInside, DeltaTree? deltaTree = null, int i = 0, string url = "",
    string title = "") : base(elementsInside, deltaTree, i)
  {
    Url = url;
    Title = title;
  }

  public string Url { get; set; }
  public string Title { get; set; }
  protected override string Tag => "a";
  protected override string StartingTag
  {
    get
    {
      var escapedUrl = EscapeHelpers.EscapeUrl(Url);
      var stringBuilder = new StringBuilder(@"<a href=""");
      if (EmailHelpers.IsEmail(Url))
      {
        stringBuilder.Append("mailto:");
      }
      stringBuilder.Append(escapedUrl);
      stringBuilder.Append('"');
      if (!Title.Equals("", StringComparison.Ordinal))
      {
        stringBuilder.Append(@" title=""");
        stringBuilder.Append(EscapeHelpers.Escape(Title));
        stringBuilder.Append('"');
      }
      stringBuilder.Append('>');
      return stringBuilder.ToString();
    }
  }

  public static LinkInlineContainer CreateInstance(int depth, IEnumerable<InlineInsert> document,
    DeltaTree? deltaTree = null, int i = 0, string url = "", string title = "")
  {
    var container = new LinkInlineContainer(deltaTree, i, url, title);
    container.Initialise(depth, document, i);
    return container;
  }
}
