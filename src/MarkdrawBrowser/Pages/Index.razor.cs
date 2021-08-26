using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Net.Http;
using System.Threading.Tasks;
using Markdraw.Delta.Formats;
using Markdraw.Delta.Indents;
using Markdraw.Delta.Links;
using Markdraw.Delta.Operations.Inserts;
using Markdraw.Delta.Operations.Inserts.Inlines;
using Markdraw.Delta.OperationSequences;
using Markdraw.Tree;
using MarkdrawBrowser.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MarkdrawBrowser.Pages
{
  public partial class Index : ComponentBase
  {
    private string _markdown = "";
    private string _content = "<p>Loading...</p>";
    private DeltaTree _deltaTree;
    private Editor _editor;

    private ModalOpen _modal = ModalOpen.None;
    private string _modalLink = "";
    private string _modalImageUrl = "";
    private string _modalImageAlt = "";
    private string _modalCodeLanguage = "";
    private string _modalCodeContents = "";

    private DotNetObjectReference<Index> _indexRef;

    [Inject]
    private HttpClient Http { get; set; }
    [Inject]
    private IJSRuntime Js { get; set; }

    private enum ModalOpen
    {
      None,
      Image,
      Link,
      Code
    }

    // ReSharper disable once ClassNeverInstantiated.Local
    private record Cursor(int Start, int End, int NextLine);

    private string Markdown
    {
      get => _markdown;
      set
      {
        _markdown = value;
        _deltaTree.SetWithMarkdown(_markdown);
        _content = _deltaTree.Root.ToString();
      }
    }

    protected override async Task OnInitializedAsync()
    {
      _indexRef = DotNetObjectReference.Create(this);
      await Js.InvokeVoidAsync("setReference", _indexRef);
      _deltaTree = new DeltaTree(_markdown);
      const string url = "https://raw.githubusercontent.com/jonathanjameswatson/markdraw/main/sample.md";
      Markdown = await Http.GetStringAsync(url);
    }

    private async Task<Cursor> GetCursor()
    {
      return await Js.InvokeAsync<Cursor>("getCursor");
    }

    private async Task SetFormat(Format format)
    {
      var (start, i, nextLine) = await GetCursor();
      var transformation = new Transformation();

      if (start > 0)
      {
        transformation.Retain(start);
      }

      var end = format is ILineFormatModifier ? nextLine : i;

      if (end - start != 0)
      {
        transformation.Retain(end - start, format);
      }

      _deltaTree.Delta = _deltaTree.Delta.Transform(transformation);
      _content = _deltaTree.Root.ToString();
    }

    private int InsertElementsWithCursor(Document inserts, Cursor cursorPos)
    {
      var transformation = new Transformation();

      var characters = inserts.Characters;

      if (cursorPos.Start > 0)
      {
        transformation.Retain(cursorPos.Start);
      }

      if (cursorPos.End - cursorPos.Start != 0)
      {
        transformation.Delete(cursorPos.End - cursorPos.Start);
      }

      transformation.InsertMany(inserts);

      _deltaTree.Delta = _deltaTree.Delta.Transform(transformation);
      _content = _deltaTree.Root.ToString();

      return cursorPos.Start + characters;
    }

    private async Task InsertElements(Document inserts)
    {
      var cursorPos = await GetCursor();
      InsertElementsWithCursor(inserts, cursorPos);
    }

    private async Task Bold()
    {
      await SetFormat(new InlineFormat {
        Bold = true, Italic = null, Link = null, Code = null
      });
    }

    private async Task Italic()
    {
      await SetFormat(new InlineFormat {
        Bold = null, Italic = true, Link = null, Code = null
      });
    }

    private async Task Code()
    {
      await SetFormat(new InlineFormat {
        Bold = null, Italic = null, Link = null, Code = true
      });
    }

    private void Link()
    {
      _modal = ModalOpen.Link;
    }

    private async Task Quote()
    {
      await SetFormat(
        new FunctionalLineFormat {
          IndentsFunction = list => list.Insert(0, Indent.Quote)
        }
      );
    }

    private async Task Bullet()
    {
      await SetFormat(
        new FunctionalLineFormat {
          IndentsFunction = list => list.Insert(0, Indent.LooseBullet)
        }
      );
    }

    private async Task Number()
    {
      await SetFormat(
        new FunctionalLineFormat {
          IndentsFunction = list => list.Insert(0, Indent.Number(2))
        });
    }

    private async Task Clear()
    {
      await SetFormat(new InlineFormat());
    }

    private async Task ClearLine()
    {
      await SetFormat(new FunctionalLineFormat {
        IndentsFunction = _ => ImmutableList<Indent>.Empty
      });
    }

    private async Task HeaderUp()
    {
      await SetFormat(new FunctionalLineFormat {
        HeaderFunction = i => Math.Max((i + 6) % 7, 1)
      });
    }

    private async Task HeaderDown()
    {
      await SetFormat(new FunctionalLineFormat {
        HeaderFunction = i => (i == 0 ? i : i + 1) % 7
      });
    }

    private void Image()
    {
      _modal = ModalOpen.Image;
    }

    private async Task Divider()
    {
      await InsertElements(
        new Document()
          .Insert(new LineInsert())
          .Insert(new DividerInsert())
          .Insert(new LineInsert())
      );
    }

    private void CodeBlock()
    {
      _modal = ModalOpen.Code;
    }

    private void Close()
    {
      _modal = ModalOpen.None;
    }

    private async Task SetLink()
    {
      await SetFormat(new InlineFormat {
        Bold = null, Italic = null, Link = new ExistentLink(_modalLink), Code = null
      });
      Close();
    }

    private async Task AddImage()
    {
      await InsertElements(
        new Document()
          .Insert(new ImageInsert(_modalImageUrl, _modalImageAlt))
      );
      Close();
    }

    private async Task AddCode()
    {
      await InsertElements(
        new Document()
          .Insert(new LineInsert())
          .Insert(new CodeInsert(_modalCodeContents, _modalCodeLanguage))
          .Insert(new LineInsert())
      );
      Close();
    }

    [JSInvokable]
    private int InsertText(string text, Cursor cursor)
    {
      var splitText = new List<string>(text.Split("\n"));
      var lastTextFormat = _deltaTree.Delta.GetFirstFormat<InlineFormat>(cursor.Start);
      LineFormat lastLineFormat = null;
      var ops = new Document();

      foreach (var part in splitText.GetRange(0, splitText.Count - 1))
      {
        lastLineFormat ??= _deltaTree.Delta.GetFirstFormat<LineFormat>(cursor.Start);

        if (part.Length != 0)
        {
          ops.Insert(part, lastTextFormat);
        }

        ops.Insert(new LineInsert(lastLineFormat));
      }

      var final = splitText[^1];
      if (final.Length != 0)
      {
        ops.Insert(final, lastTextFormat);
      }

      var i = InsertElementsWithCursor(ops, cursor);

      StateHasChanged();

      return i;
    }

    [JSInvokable]
    private int RemoveText(bool backwards, Cursor cursor)
    {
      var transformation = new Transformation();

      var range = cursor.End - cursor.Start;
      var retainAmount = range == 0 && backwards ? cursor.Start - 1 : cursor.Start;

      if (retainAmount > 0)
      {
        transformation.Retain(retainAmount);
      }

      transformation.Delete(Math.Max(range, 1));

      _deltaTree.Delta = _deltaTree.Delta.Transform(transformation);
      _content = _deltaTree.Root.ToString();
      StateHasChanged();

      return Math.Max(0, retainAmount);
    }

    private string ExportMarkdown()
    {
      return _deltaTree.Delta.ToString();
    }
  }
}
