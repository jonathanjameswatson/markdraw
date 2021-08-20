using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MarkdrawBrowser.Shared
{
  public partial class Editor : ComponentBase
  {
    [Parameter]
    public string Content { get; set; }

    [Inject]
    private IJSRuntime Js { get; set; }

    private ElementReference _editor;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
      if (firstRender)
      {
        await Js.InvokeVoidAsync("setUp", _editor);
      }

      await Js.InvokeVoidAsync("renderMarkdown", _editor, Content);
    }
  }
}
