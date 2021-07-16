using System;
using System.Linq;
using Markdraw.Tree;
using NUglify;
using NUglify.Html;

namespace Markdraw.Parser
{
  public class Parser
  {
    public static String Parse(string markdown)
    {
      String html = DeltaTree.Parse(markdown).ToString();
      return Inner(Prettify(html));
    }

    public static String Prettify(string html)
    {
      var settings = HtmlSettings.Pretty();
      settings.IsFragmentOnly = true;
      settings.MinifyCss = false;
      settings.MinifyCssAttributes = false;
      settings.MinifyJs = false;
      return Uglify.Html(html, settings).ToString();
    }

    public static String Inner(string html)
    {
      return String.Join("\n", html.Split(new[] { "\n" },
        StringSplitOptions.None).Skip(1).SkipLast(1).Select(s => s.Substring(2)));
    }
  }
}
