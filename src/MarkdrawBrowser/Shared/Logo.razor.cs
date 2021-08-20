using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MarkdrawBrowser.Shared
{
  public partial class Logo : ComponentBase
  {
    [Parameter]
    public Func<ElementReference> GetHoverElement { get; set; }

    [Inject]
    private IJSRuntime Js { get; set; }

    private ElementReference _wheel;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
      if (GetHoverElement is null)
      {
        throw new InvalidOperationException("GetHoverElement must not be null.");
      }

      if (firstRender)
      {
        await Js.InvokeVoidAsync("setupLogo", GetHoverElement(), _wheel);
      }
    }
  }
}
