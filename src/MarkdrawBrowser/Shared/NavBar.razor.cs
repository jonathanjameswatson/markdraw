using System;
using Microsoft.AspNetCore.Components;

namespace MarkdrawBrowser.Shared
{
  public partial class NavBar : ComponentBase
  {
    [Parameter]
    public string ImportMarkdown
    {
      get => _markdown;
      set
      {
        if (_markdown == value) return;
        _temporaryMarkdown = value;
        _markdown = value;
        ImportMarkdownChanged.InvokeAsync(value);
      }
    }

    [Parameter]
    public EventCallback<string> ImportMarkdownChanged { get; set; }

    [Parameter]
    public Func<string> ExportMarkdown { get; set; }

    private bool _active = false;

    private string _markdown;
    private string _temporaryMarkdown;
    private string _exportMarkdown;

    private ElementReference _navbarLogo;
    private ModalOpen _modal = ModalOpen.Neither;

    private enum ModalOpen
    {
      Neither,
      Import,
      Export
    }

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
      _exportMarkdown = ExportMarkdown();
      _modal = ModalOpen.Export;
    }
  }
}
