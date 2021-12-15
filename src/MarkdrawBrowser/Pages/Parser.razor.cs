using System.Text.Json;
using System.Text.Json.Serialization;
using Markdig;
using Markdig.Helpers;
using Markdraw.Delta.OperationSequences;
using Markdraw.MarkdownToDelta;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MarkdrawBrowser.Pages;

internal class StringSliceJsonConverter : JsonConverter<StringSlice>
{
  public override StringSlice Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    throw new InvalidOperationException("Deserialization should not be required.");
  }

  public override void Write(Utf8JsonWriter writer, StringSlice stringSliceValue, JsonSerializerOptions options)
  {
    writer.WriteNullValue();
  }
}

public partial class Parser : ComponentBase
{
  private const string Original = "# Parser\n\nUse this page to see how Markdraw handles Markdown.";
  private static readonly string OriginalMarkdrawHtmlOutput = GetMarkdrawHtml(Original);
  private static readonly string OriginalMarkdigHtmlOutput = GetMarkdigHtml(Original);
  private static readonly string OriginalMarkdigAstOutput = GetMarkdigAst(Original);

  private Document _deltaOutput = GetDelta(Original);
  private string _highlightedMarkdigAstOutput = OriginalMarkdigAstOutput;
  private string _highlightedMarkdigHtmlOutput = OriginalMarkdigHtmlOutput;
  private string _highlightedMarkdrawHtmlOutput = OriginalMarkdrawHtmlOutput;

  private string _input = Original;

  private string _markdigAstOutput = OriginalMarkdigAstOutput;

  private string _markdigHtmlOutput = OriginalMarkdigHtmlOutput;

  private string _markdrawHtmlOutput = OriginalMarkdrawHtmlOutput;

  private string Input
  {
    get => _input;
    set
    {
      _input = value;

      _markdrawHtmlOutput = GetMarkdrawHtml(value);
      _highlightedMarkdrawHtmlOutput = HighlightHtml(_markdrawHtmlOutput);

      _markdigHtmlOutput = GetMarkdigHtml(value);
      _highlightedMarkdigHtmlOutput = HighlightHtml(_markdigHtmlOutput);

      _markdigAstOutput = GetMarkdigAst(value);
      _highlightedMarkdigAstOutput = HighlightJson(_markdigAstOutput);

      _deltaOutput = GetDelta(value);
    }
  }

  protected override void OnInitialized()
  {
    _highlightedMarkdrawHtmlOutput = HighlightHtml(_markdrawHtmlOutput);
    _highlightedMarkdigHtmlOutput = HighlightHtml(_markdigHtmlOutput);
    _highlightedMarkdigAstOutput = HighlightJson(_markdigAstOutput);
  }

  private static string GetMarkdrawHtml(string input)
  {
    return Markdraw.Parser.Parser.Parse(input);
  }

  private static string GetMarkdigHtml(string input)
  {
    return Markdraw.Parser.Parser.Prettify(Markdown.ToHtml(input));
  }

  private string HighlightHtml(string html)
  {
    return ((IJSInProcessRuntime)_js).Invoke<string>("window.highlightHtml", html);
  }

  private string HighlightJson(string json)
  {
    return ((IJSInProcessRuntime)_js).Invoke<string>("window.highlightJson", json);
  }

  private static string GetMarkdigAst(string input)
  {
    return JsonSerializer.Serialize(Markdown.Parse(input), new JsonSerializerOptions {
      ReferenceHandler = ReferenceHandler.IgnoreCycles,
      WriteIndented = true,
      Converters = {
        new StringSliceJsonConverter()
      },
      DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    });
  }

  private static Document GetDelta(string input)
  {
    return MarkdownToDeltaConverter.Parse(input);
  }
}
