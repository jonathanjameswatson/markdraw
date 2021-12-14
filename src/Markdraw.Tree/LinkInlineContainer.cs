using Markdraw.Delta.Operations.Inserts.Inlines;
using Markdraw.Helpers;

namespace Markdraw.Tree;

public class LinkInlineContainer : InlineContainer
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
      var escapedTitle = EscapeHelpers.Escape(Title);
      if (EmailHelpers.IsEmail(Url))
      {
        escapedUrl = $"mailto:{escapedUrl}";
      }
      var titleString = Title == "" ? "" : $@" title=""{escapedTitle}""";
      return $@"<a href=""{escapedUrl}""{titleString}>";
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
