using Markdraw.Delta.OperationSequences;
using Markdraw.MarkdownToDelta;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MarkdrawBrowser.Pages;

public partial class Parser : ComponentBase
{
  private const string Original = "# Parser\n\nUse this page to see how Markdraw handles Markdown.";
  private Document _deltaOutput = GetDelta(Original);
  private string _highlightedHtmlOutput;
  private string _htmlOutput = GetHtml(Original);

  private string _input = Original;

  [Inject]
  private IJSRuntime Js { get; set; }

  private string Input
  {
    get => _input;
    set
    {
      _input = value;
      _htmlOutput = GetHtml(value);
      _highlightedHtmlOutput = HighlightHtml(_htmlOutput);
      _deltaOutput = GetDelta(value);
    }
  }

  protected override void OnInitialized()
  {
    _highlightedHtmlOutput = HighlightHtml(_htmlOutput);
  }

  private static string GetHtml(string input)
  {
    return Markdraw.Parser.Parser.Parse(input);
  }

  private string HighlightHtml(string html)
  {
    return ((IJSInProcessRuntime)Js).Invoke<string>("window.highlightHtml", html);
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

  private static Document GetDelta(string input)
  {
    return MarkdownToDeltaConverter.Parse(input);
  }
}