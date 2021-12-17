using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Markdig;
using Markdig.Helpers;
using Markdraw.Delta.OperationSequences;
using Markdraw.MarkdownToDelta;
using Markdraw.Tree;
using Markdraw.Tree.TreeNodes.Containers;
using Markdraw.Tree.TreeNodes.Containers.BlockContainers;
using Markdraw.Tree.TreeNodes.Containers.InlineContainers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MarkdrawBrowser.Pages;

public partial class Parser : ComponentBase
{
  private const string Original = "# Parser\n\nUse this page to see how Markdraw handles Markdown.";
  private static readonly Document OriginalDelta = GetDelta(Original);
  private static readonly DeltaTree OriginalDeltaTree = GetDeltaTree(OriginalDelta);
  private static readonly string OriginalMarkdrawHtml = GetMarkdrawHtml(OriginalDeltaTree);
  private static readonly string OriginalMarkdigHtml = GetMarkdigHtml(Original);
  // private static readonly string OriginalMarkdigAstJson = GetMarkdigAstJson(Original);

  private string _markdrawHtml = OriginalMarkdrawHtml;
  private string _markdigHtml = OriginalMarkdigHtml;
  private Document _delta = OriginalDelta;
  private DeltaTree _deltaTree = OriginalDeltaTree;
  // private string _markdigAstJson = OriginalMarkdigAstJson;

  private string _highlightedMarkdrawHtml = OriginalMarkdrawHtml;
  private string _highlightedMarkdigHtml = OriginalMarkdigHtml;
  // private string _highlightedMarkdigAstJson = OriginalMarkdigAstJson;

  private string _input = Original;

  private string Input
  {
    get => _input;
    set
    {
      _input = value;

      _delta = GetDelta(value);

      _deltaTree = GetDeltaTree(_delta);

      _markdrawHtml = GetMarkdrawHtml(_deltaTree);
      _highlightedMarkdrawHtml = HighlightHtml(_markdrawHtml);

      _markdigHtml = GetMarkdigHtml(value);
      _highlightedMarkdigHtml = HighlightHtml(_markdigHtml);

      // _markdigAstJson = GetMarkdigAstJson(value);
      // _highlightedMarkdigAstJson = HighlightJson(_markdigAstJson);
    }
  }

  protected override void OnInitialized()
  {
    _highlightedMarkdrawHtml = HighlightHtml(_markdrawHtml);
    _highlightedMarkdigHtml = HighlightHtml(_markdigHtml);
    // _highlightedMarkdigAstJson = HighlightJson(_markdigAstJson);
  }

  private static DeltaTree GetDeltaTree(Document input)
  {
    return new DeltaTree(input);
  }

  private static string GetMarkdrawHtml(DeltaTree input)
  {
    return Markdraw.Parser.Parser.Prettify(input.ToString() ?? "");
  }

  private static string GetMarkdigHtml(string input)
  {
    return Markdraw.Parser.Parser.Prettify(Markdown.ToHtml(input));
  }

  private static Document GetDelta(string input)
  {
    return MarkdownToDeltaConverter.Parse(input);
  }

  private static string GetMarkdigAstJson(string input)
  {
    return JsonSerializer.Serialize(Markdown.Parse(input), new JsonSerializerOptions {
      ReferenceHandler = ReferenceHandler.IgnoreCycles,
      WriteIndented = true,
      Converters = {
        // new StringSliceJsonConverter()
      },
      DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    });
  }

  private string HighlightHtml(string html)
  {
    return ((IJSInProcessRuntime)_js).Invoke<string>("window.highlightHtml", html);
  }

  private string HighlightJson(string json)
  {
    return ((IJSInProcessRuntime)_js).Invoke<string>("window.highlightJson", json);
  }
}
