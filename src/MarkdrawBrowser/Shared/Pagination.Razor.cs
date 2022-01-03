using Microsoft.AspNetCore.Components;

namespace MarkdrawBrowser.Shared;

public partial class Pagination : ComponentBase
{
  private int _increment = 1;

  private int _max = 3;

  private int _min = 1;
  private int _page;
  [Parameter]
  public int Page
  {
    get => _page;
    set
    {
      if (_page == value) return;
      _page = Math.Clamp((value - Min) / Increment * Increment + Min, Min, Max);
      PageChanged.InvokeAsync(_page);
    }
  }

  [Parameter]
  public EventCallback<int> PageChanged { get; set; }

  [Parameter]
  public int Min
  {
    get => _min;
    set
    {
      _min = value;
      Page = _page;
    }
  }
  [Parameter]
  public int Max
  {
    get => _max;
    set
    {
      _max = value;
      Page = _page;
    }
  }
  [Parameter]
  public int Increment
  {
    get => _increment;
    set
    {
      _increment = value;
      Page = _page;
    }
  }
}
