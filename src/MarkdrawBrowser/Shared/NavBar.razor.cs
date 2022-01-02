using Microsoft.AspNetCore.Components;

namespace MarkdrawBrowser.Shared;

public partial class NavBar : ComponentBase
{
  private bool _active = false;

  private string _exportMarkdown = "";

  private string _markdown = "";

  private ModalOpen _modal = ModalOpen.Neither;

  private ElementReference _navbarLogo;
  private string _temporaryMarkdown = "";
  [Parameter]
  public string ImportMarkdown
  {
    get => _markdown;
    set
    {
      if (_markdown.Equals(value, StringComparison.Ordinal)) return;
      _temporaryMarkdown = value;
      _markdown = value;
      ImportMarkdownChanged.InvokeAsync(value);
    }
  }

  [Parameter]
  public EventCallback<string> ImportMarkdownChanged { get; set; }

  [Parameter]
  public Func<string>? ExportMarkdown { get; set; }

  [Parameter]
  public bool ModalsAvailable { get; set; }

  private void ToggleActive()
  {
    _active = !_active;
  }

  private void SetMarkdown()
  {
    ImportMarkdown = _temporaryMarkdown;
    Close();
  }

  private void Close()
  {
    _modal = ModalOpen.Neither;
  }

  private void Import()
  {
    _modal = ModalOpen.Import;
  }

  private void Export()
  {
    if (ExportMarkdown is null) throw new InvalidOperationException("ExportMarkdown is not set.");
    _exportMarkdown = ExportMarkdown();
    _modal = ModalOpen.Export;
  }

  private enum ModalOpen
  {
    Neither,
    Import,
    Export
  }
}
