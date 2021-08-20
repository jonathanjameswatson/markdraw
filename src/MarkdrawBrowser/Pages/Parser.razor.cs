using Microsoft.AspNetCore.Components;

namespace MarkdrawBrowser.Pages
{
  public partial class Parser : ComponentBase
  {
    private const string Original = "# Parser\n\nUse this page to see how Markdraw handles Markdown.";

    private string _input = Original;
    private string _htmlOutput = GetHtml(Original);

    private string Input
    {
      get => _input;
      set
      {
        _input = value;
        _htmlOutput = GetHtml(value);
      }
    }

    private static string GetHtml(string input)
    {
      return Markdraw.Parser.Parser.Parse(input);
    }
  }
}
