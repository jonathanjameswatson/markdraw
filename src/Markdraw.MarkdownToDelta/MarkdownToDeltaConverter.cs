﻿using System;
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
using Markdraw.Delta.Links;
using Markdraw.Delta.Operations.Inserts;
using Markdraw.Delta.OperationSequences;

// ReSharper disable IteratorNeverReturns

namespace Markdraw.MarkdownToDelta
{
  public static class MarkdownToDeltaConverter
  {
    public static Document Parse(string markdown)
    {
      var abstractSyntaxTree = Markdown.Parse(markdown);
      var ops = new Document();
      Write(ops, abstractSyntaxTree, ImmutableList<IEnumerator<Indent>>.Empty);
      return ops;
    }

    private static void Write(Document document, IMarkdownObject block, ImmutableList<IEnumerator<Indent>> indentSequences, IEnumerator<ListIndent> listSequence = null)
    {
      var newIndents = indentSequences;
      IEnumerator<ListIndent> newListSequence = null;

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
                var newIndent = Indent.Number(int.Parse(listBlock.OrderedStart), loose);
                newListSequence = RepeatAfterFirst<ListIndent>(newIndent, newIndent with { Start = 0 });
              }
              else
              {
                var newIndent = Indent.Bullet(true, loose);
                newListSequence = RepeatAfterFirst<ListIndent>(newIndent, newIndent with { Start = false });
              }
              break;
            case ListItemBlock:
              Debug.Assert(listSequence != null, nameof(listSequence) + " != null");
              listSequence.MoveNext();
              newIndents = newIndents.Add(RepeatAfterFirst<Indent>(listSequence.Current, Indent.Continue));
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
            Write(document, child, newIndents, newListSequence);
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
              document.Insert(new CodeInsert(fencedCodeBlock.Lines.ToString(), fencedCodeBlock.Info));
              break;
            case CodeBlock codeBlock:
              document.Insert(new CodeInsert(codeBlock.Lines.ToString()));
              break;
            case HeadingBlock headingBlock:
              InsertText(document, headingBlock.Inline);
              header = headingBlock.Level;
              break;
            case HtmlBlock htmlBlock:
              document.Insert(new TextInsert(htmlBlock.Lines.ToString(), new TextFormat()));
              break;
            case LinkReferenceDefinition:
              return;
            case ParagraphBlock paragraphBlock:
              InsertText(document, paragraphBlock.Inline);
              break;
            case ThematicBreakBlock:
              document.Insert(new DividerInsert());
              break;
            default:
              throw new ArgumentOutOfRangeException(nameof(block));

          }
          document.Insert(new LineInsert(new LineFormat() {
            Indents = indents, Header = header
          }));
          break;

      }
    }

    private static void InsertText(Document document, Inline inline, TextFormat format = null)
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
                document.Insert(new ImageInsert(linkInline.Url, altText, linkInline.Title ?? ""));
                return;
              }

              newTextFormat = newTextFormat with {
                Link = new ExistentLink(linkInline.Url, linkInline.Title ?? "")
              }; // add Title as well

              break;

          }

          foreach (var child in containerInline)
          {
            InsertText(document, child, newTextFormat);
          }
          break;
        case LeafInline leafInline:
          switch (leafInline)
          {
            case AutolinkInline autolinkInline:
              document.Insert(new TextInsert(autolinkInline.Url, newTextFormat with {
                Link = new ExistentLink(autolinkInline.Url)
              }));
              break;
            case CodeInline codeInline:
              document.Insert(new TextInsert(codeInline.Content, newTextFormat with {
                Code = true
              }));
              break;
            case HtmlEntityInline htmlEntityInline:
              document.Insert(new TextInsert(htmlEntityInline.Transcoded.ToString()));
              break;
            case HtmlInline htmlInline:
              document.Insert(new TextInsert(htmlInline.Tag));
              break;
            case LineBreakInline:
              document.Insert(" ");
              break;
            case LiteralInline literalInline:
              document.Insert(new TextInsert(literalInline.Content.ToString(), newTextFormat));
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