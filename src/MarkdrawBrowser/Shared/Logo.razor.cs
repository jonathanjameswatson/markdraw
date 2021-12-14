using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MarkdrawBrowser.Shared;

public partial class Logo : ComponentBase
{
  private ElementReference _wheel;

  [Parameter]
  public Func<ElementReference> GetHoverElement { get; set; }

  [Inject]
  private IJSRuntime Js { get; set; }

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