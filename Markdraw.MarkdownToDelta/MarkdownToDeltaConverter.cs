using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using Markdraw.Delta;
using Markdig;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using Markdraw.Delta.Formats;
using Markdraw.Delta.Indents;
using Markdraw.Delta.Operations.Inserts;

// ReSharper disable IteratorNeverReturns

namespace Markdraw.MarkdownToDelta
{
  public static class MarkdownToDeltaConverter
  {
    public static Ops Parse(string markdown)
    {
      var abstractSyntaxTree = Markdown.Parse(markdown);
      var ops = new Ops();
      Write(ops, abstractSyntaxTree, ImmutableList<IEnumerator<Indent>>.Empty);
      return ops;
    }

    private static void Write(Ops ops, IMarkdownObject block, ImmutableList<IEnumerator<Indent>> indentSequences, ListIndent listType = null)
    {
      var newIndents = indentSequences;
      ListIndent newListType = null;

      switch (block)
      {
        case BlankLineBlock:
          break;
        case ContainerBlock containerBlock:
          switch (containerBlock)
          {
            case LinkReferenceDefinitionGroup:
              return;
            case ListBlock listBlock:
              var loose = listBlock.IsLoose;
              if (listBlock.IsOrdered)
              {
                Debug.Assert(listBlock.OrderedStart != null, "listBlock.OrderedStart != null");
                newListType = Indent.Number(int.Parse(listBlock.OrderedStart), loose);
              }
              else
              {
                newListType = Indent.Bullet(loose);
              }
              break;
            case ListItemBlock:
              newIndents = newIndents.Add(RepeatAfterFirst<Indent>(listType, Indent.Continue));
              break;
            case MarkdownDocument:
              break;
            case QuoteBlock:
              newIndents = newIndents.Add(Repeat<Indent>(Indent.Quote));
              break;
            default:
              throw new ArgumentOutOfRangeException(nameof(block));

          }

          foreach (var child in containerBlock)
          {
            Write(ops, child, newIndents, newListType);
          }
          break;
        case LeafBlock leafBlock:
          var indents = indentSequences.Select(sequence => {
            sequence.MoveNext();
            return sequence.Current;
          }).ToImmutableList();
          var header = 0;

          switch (leafBlock)
          {
            case EmptyBlock:
              return;
            case FencedCodeBlock fencedCodeBlock:
              ops.Insert(new CodeInsert(fencedCodeBlock.Lines.ToString(), fencedCodeBlock.Info));
              break;
            case HeadingBlock headingBlock:
              InsertText(ops, headingBlock.Inline);
              header = headingBlock.Level;
              break;
            case HtmlBlock htmlBlock:
              ops.Insert(new TextInsert(htmlBlock.Lines.ToString(), new TextFormat()));
              break;
            case LinkReferenceDefinition:
              return;
            case ParagraphBlock paragraphBlock:
              InsertText(ops, paragraphBlock.Inline);
              break;
            case ThematicBreakBlock:
              ops.Insert(new DividerInsert());
              break;
            default:
              throw new ArgumentOutOfRangeException(nameof(block));

          }
          ops.Insert(new LineInsert(new LineFormat() {
            Indents = indents, Header = header
          }));
          break;

      }
    }

    private static void InsertText(Ops ops, Inline inline, TextFormat format = null)
    {
      var newTextFormat = format ?? new TextFormat();
      switch (inline)
      {
        case ContainerInline containerInline:
          switch (containerInline)
          {
            case DelimiterInline:
              return;
            case EmphasisInline emphasisInline:
              newTextFormat = emphasisInline.DelimiterCount switch {
                (< 2) => newTextFormat with {
                  Italic = true
                },
                _ => newTextFormat with {
                  Bold = true
                }
              };
              break;
            case LinkInline linkInline:
              if (linkInline.IsImage)
              {
                var altText = GetText(linkInline);
                ops.Insert(new ImageInsert(linkInline.Url, altText, linkInline.Title ?? ""));
                return;
              }

              newTextFormat = newTextFormat with {
                Link = new Link(linkInline.Url, linkInline.Title ?? "")
              }; // add Title as well

              break;

          }

          foreach (var child in containerInline)
          {
            InsertText(ops, child, newTextFormat);
          }
          break;
        case LeafInline leafInline:
          switch (leafInline)
          {
            case AutolinkInline autolinkInline:
              ops.Insert(new TextInsert(autolinkInline.Url, newTextFormat with {
                Link = new Link(autolinkInline.Url)
              }));
              break;
            case CodeInline codeInline:
              ops.Insert(new TextInsert(codeInline.Content, newTextFormat with {
                Code = true
              }));
              break;
            case HtmlEntityInline htmlEntityInline:
              ops.Insert(new TextInsert(htmlEntityInline.Transcoded.ToString()));
              break;
            case HtmlInline htmlInline:
              ops.Insert(new TextInsert(htmlInline.Tag));
              break;
            case LineBreakInline:
              return; // fix later maybe
            case LiteralInline literalInline:
              ops.Insert(new TextInsert(literalInline.Content.ToString(), newTextFormat));
              break;
            default:
              throw new ArgumentOutOfRangeException(nameof(inline));

          }
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(inline));

      }
    }

    private static string GetText(Inline inline)
    {
      return inline switch {
        ContainerInline containerInline => string.Join("", containerInline.Select(GetText)),
        LeafInline leafInline => leafInline switch {
          LiteralInline literalInline => literalInline.Content.ToString(),
          _ => ""
        },
        _ => ""
      };
    }

    private static IEnumerator<T> Repeat<T>(T repeated)
    {
      IEnumerable<T> Enumerable()
      {
        while (true) {
          yield return repeated;
        }
      }

      return Enumerable().GetEnumerator();
    }

    private static IEnumerator<T> RepeatAfterFirst<T>(T first, T repeated)
    {
      IEnumerable<T> Enumerable()
      {
        yield return first;
        while (true) {
          yield return repeated;
        }
      }

      return Enumerable().GetEnumerator();
    }

  }
}
