using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MarkdrawBrowser.Shared;

public partial class Editor : ComponentBase
{
  private ElementReference _editor;

  [Parameter]
  public string Content { get; set; } = "";

  protected override async Task OnAfterRenderAsync(bool firstRender)
  {
    if (firstRender)
    {
      await _js.InvokeVoidAsync("setUp", _editor);
    }

    await _js.InvokeVoidAsync("renderMarkdown", _editor, Content);
  }
}
