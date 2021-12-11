using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace MarkdrawBrowser.Shared
{
  public partial class Tabs : ComponentBase
  {
    private readonly List<TabPage> _pages = new();

    [Parameter]
    public RenderFragment ChildContent { get; set; }

    public TabPage ActivePage { get; private set; }

    internal void AddPage(TabPage tabPage)
    {
      _pages.Add(tabPage);
      if (_pages.Count == 1)
      {
        ActivePage = tabPage;
      }
      StateHasChanged();
    }
  }
}
