using System;
using System.Linq;
using Markdraw.Tree;
using NUglify;
using NUglify.Html;

namespace Markdraw.Parser
{
  public class Parser
  {
    public static string Parse(string markdown)
    {
      var html = string.Join(' ', DeltaTree.Parse(markdown).Select(child => child.ToString()));
      return Prettify(html);
    }

    public static string Prettify(string html)
    {
      var settings = HtmlSettings.Pretty();
      settings.IsFragmentOnly = true;
      settings.MinifyCss = false;
      settings.MinifyCssAttributes = false;
      settings.MinifyJs = false;
      return Uglify.Html(html, settings).ToString();
    }
  }
}
