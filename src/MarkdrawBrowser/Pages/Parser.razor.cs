using Markdraw.MarkdownToDelta;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MarkdrawBrowser.Pages
{
  public partial class Parser : ComponentBase
  {
    [Inject]
    private IJSRuntime Js { get; set; }

    private const string Original = "# Parser\n\nUse this page to see how Markdraw handles Markdown.";

    private string _input = Original;
    private string _htmlOutput = GetHtml(Original);
    private string _deltaOutput = GetDelta(Original);

    private string Input
    {
      get => _input;
      set
      {
        _input = value;
        _htmlOutput = GetHtml(value);
        _deltaOutput = GetDelta(value);
      }
    }

    private static string GetHtml(string input)
    {
      return Markdraw.Parser.Parser.Parse(input);
    }

    /*
    private static string GetMarkdigAst(string input)
    {
      return JsonSerializer.Serialize(Markdown.Parse(input), new JsonSerializerOptions {
        ReferenceHandler = ReferenceHandler.Preserve,
        WriteIndented = true
      });
    }
    */

    private static string GetDelta(string input)
    {
      return MarkdownToDeltaConverter.Parse(input).ToString();
    }
  }
}
