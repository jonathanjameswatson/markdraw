using Markdraw.Tree;
using NUglify;
using NUglify.Html;

namespace Markdraw.Parser;

public static class Parser
{
  public static string Parse(string markdown)
  {
    var html = string.Concat(DeltaTree.Parse(markdown).Select(child => child.ToString()));
    return Prettify(html);
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
