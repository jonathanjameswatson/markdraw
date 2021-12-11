using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace MarkdrawBrowser.Shared
{
  public partial class FormatButton : ComponentBase
  {
    [Parameter]
    public string Icon { get; set; } = "";

    [Parameter]
    public bool ScrollFix { get; set; }

    [Parameter]
    public bool Disabled { get; set; }

    [Parameter]
    public EventCallback<MouseEventArgs> EnabledClickCallback { get; set; }
  }
}
