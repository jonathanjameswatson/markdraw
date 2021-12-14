using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace MarkdrawBrowser.Shared;

public partial class Button : ComponentBase
{
  [Parameter]
  public RenderFragment ChildContent { get; set; }

  [Parameter]
  public bool Disabled { get; set; }

  [Parameter]
  public EventCallback<MouseEventArgs> EnabledClickCallback { get; set; }

  private async void Invoke()
  {
    if (Disabled == false)
    {
      await EnabledClickCallback.InvokeAsync();
    }
  }
}