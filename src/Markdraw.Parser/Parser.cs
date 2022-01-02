using Markdraw.MarkdownToDelta;
using Markdraw.Tree;
using NUglify;
using NUglify.Html;

namespace Markdraw.Parser;

public static class Parser
{
  public static string Parse(string markdown)
  {
    var html = DeltaTree.Parse(markdown).InnerHtml;
    return Prettify(html);
  }

  public static string DoubleParse(string markdown)
  {
    var delta = MarkdownToDeltaConverter.Parse(markdown);
    return Parse(delta.ToString());
  }

  public static string Prettify(string html)
  {
    var settings = HtmlSettings.Pretty();
    settings.IsFragmentOnly = true;
    settings.MinifyCss = false;
    settings.MinifyCssAttributes = false;
    settings.MinifyJs = false;
    settings.AttributeQuoteChar = '"';
    return Uglify.Html(html, settings).ToString();
  }
}
