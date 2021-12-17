using Markdig;
using Markdig.Syntax;
using Markdraw.Delta.OperationSequences;
using Markdraw.MarkdownToDelta;
using Markdraw.Tree;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MarkdrawBrowser.Pages;

public partial class Parser : ComponentBase
{
  private const string Original = "# Parser\n\nUse this page to see how Markdraw handles Markdown.";
  private static readonly DeltaTree OriginalDeltaTree = GetDeltaTree(GetDelta(Original));
  private static readonly string OriginalMarkdrawHtml = GetMarkdrawHtml(OriginalDeltaTree);
  private static readonly MarkdownDocument OriginalMarkdigAst = GetMarkdigAst(Original);
  private static readonly string OriginalMarkdigHtml = GetMarkdigHtml(OriginalMarkdigAst);

  private string _markdrawHtml = OriginalMarkdrawHtml;
  private string _markdigHtml = OriginalMarkdigHtml;
  private DeltaTree _deltaTree = OriginalDeltaTree;
  private MarkdownDocument _markdigAst = OriginalMarkdigAst;

  private string _highlightedMarkdrawHtml = OriginalMarkdrawHtml;
  private string _highlightedMarkdigHtml = OriginalMarkdigHtml;

  private string _input = Original;

  private string Input
  {
    get => _input;
    set
    {
      _input = value;

      var delta = GetDelta(value);

      _deltaTree = GetDeltaTree(delta);

      _markdigAst = GetMarkdigAst(value);

      _markdrawHtml = GetMarkdrawHtml(_deltaTree);
      _highlightedMarkdrawHtml = HighlightHtml(_markdrawHtml);

      _markdigHtml = GetMarkdigHtml(_markdigAst);
      _highlightedMarkdigHtml = HighlightHtml(_markdigHtml);
    }
  }

  protected override void OnInitialized()
  {
    _highlightedMarkdrawHtml = HighlightHtml(_markdrawHtml);
    _highlightedMarkdigHtml = HighlightHtml(_markdigHtml);

  }

  private static DeltaTree GetDeltaTree(Document input)
  {
    return new DeltaTree(input);
  }

  private static string GetMarkdrawHtml(DeltaTree input)
  {
    return Markdraw.Parser.Parser.Prettify(string.Join(' ', input.Root.Select(child => child.ToString())));
  }

  private static string GetMarkdigHtml(MarkdownDocument input)
  {
    return Markdraw.Parser.Parser.Prettify(input.ToHtml());
  }

  private static Document GetDelta(string input)
  {
    return MarkdownToDeltaConverter.Parse(input);
  }

  private static MarkdownDocument GetMarkdigAst(string input)
  {
    return Markdown.Parse(input);
  }

  private string HighlightHtml(string html)
  {
    return ((IJSInProcessRuntime)_js).Invoke<string>("window.highlightHtml", html);
  }
}
