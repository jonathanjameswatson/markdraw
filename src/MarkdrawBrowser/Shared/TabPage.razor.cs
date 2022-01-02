using Microsoft.AspNetCore.Components;

namespace MarkdrawBrowser.Shared;

public partial class TabPage : ComponentBase
{
  [CascadingParameter]
  private Tabs? Parent { get; set; }

  [Parameter]
  public RenderFragment? ChildContent { get; set; }

  [Parameter]
  public string? Text { get; set; }

  protected override void OnInitialized()
  {
    if (Parent is null)
    {
      throw new InvalidOperationException("TabPage must exist within a TabControl");
    }

    base.OnInitialized();
    Parent.AddPage(this);
  }
}
