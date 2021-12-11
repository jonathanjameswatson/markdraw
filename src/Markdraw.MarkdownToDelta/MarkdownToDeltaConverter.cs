using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using Markdig;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using Markdraw.Delta.Formats;
using Markdraw.Delta.Indents;
using Markdraw.Delta.Operations.Inserts;
using Markdraw.Delta.Operations.Inserts.Inlines;
using Markdraw.Delta.OperationSequences;
using Markdraw.Delta.Styles;
using Markdraw.Helpers;

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
      var newIndentSequences = indentSequences;
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
                newListSequence = SequenceHelpers.RepeatAfterFirst<ListIndent>(newIndent, newIndent with {
                  Start = 0
                });
              }
              else
              {
                var newIndent = Indent.Bullet(true, loose);
                newListSequence = SequenceHelpers.RepeatAfterFirst<ListIndent>(newIndent, newIndent with {
                  Start = false
                });
              }
              break;
            case ListItemBlock:
              Debug.Assert(listSequence != null, nameof(listSequence) + " != null");
              listSequence.MoveNext();
              newIndentSequences = newIndentSequences.Add(SequenceHelpers.RepeatAfterFirst<Indent>(listSequence.Current, listSequence.Current.NextIndent));
              break;
            case MarkdownDocument:
              break;
            case QuoteBlock:
              newIndentSequences = newIndentSequences.Add(SequenceHelpers.RepeatAfterFirst<Indent>(Indent.Quote, Indent.Quote with {
                Start = false
              }));
              break;
            default:
              throw new ArgumentOutOfRangeException(nameof(block));

          }

          if (containerBlock.Count == 0)
          {
            document.Insert(new LineInsert(new LineFormat {
              Indents = SequenceHelpers.YieldFromSequences(newIndentSequences).ToImmutableList(), Header = 0
            }));
          }
          foreach (var child in containerBlock)
          {
            Write(document, child, newIndentSequences, newListSequence);
          }


          break;
        case LeafBlock leafBlock:
          var indents = SequenceHelpers.YieldFromSequences(newIndentSequences).ToImmutableList();
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
              InsertInline(document, headingBlock.Inline);
              header = headingBlock.Level;
              break;
            case HtmlBlock htmlBlock:
              document.Insert(new BlockHtmlInsert(htmlBlock.Lines.ToString()));
              break;
            case LinkReferenceDefinition:
              return;
            case ParagraphBlock paragraphBlock:
              InsertInline(document, paragraphBlock.Inline);
              break;
            case ThematicBreakBlock:
              document.Insert(new DividerInsert());
              break;
            default:
              throw new ArgumentOutOfRangeException(nameof(block));

          }

          document.Insert(new LineInsert(new LineFormat {
            Indents = indents, Header = header
          }));

          break;
      }
    }

    private static void InsertInline(Document document, Inline inline, InlineFormat format = null)
    {
      var newFormat = format ?? new InlineFormat();
      switch (inline)
      {
        case ContainerInline containerInline:
          switch (containerInline)
          {
            case DelimiterInline delimiterInline:
              var literal = delimiterInline.ToLiteral();
              document.Insert(new TextInsert(literal, newFormat));
              break;
            case EmphasisInline emphasisInline:
              newFormat = emphasisInline.DelimiterCount switch {
                (< 2) => newFormat with {
                  Styles = newFormat.Styles.Add(Style.Italic)
                },
                _ => newFormat with {
                  Styles = newFormat.Styles.Add(Style.Bold)
                }
              };
              break;
            case LinkInline linkInline:
              if (linkInline.IsImage)
              {
                var altText = GetText(linkInline);
                document.Insert(new ImageInsert(linkInline.Url, altText, linkInline.Title ?? "") with {
                  Format = newFormat
                });
                return;
              }

              newFormat = newFormat with {
                Styles = newFormat.Styles.Add(Style.Link(linkInline.Url, linkInline.Title ?? ""))
              };
              break;
          }

          foreach (var child in containerInline)
          {
            InsertInline(document, child, newFormat);
          }
          break;
        case LeafInline leafInline:
          InlineInsert newInsert = leafInline switch {
            AutolinkInline autolinkInline => new TextInsert(autolinkInline.Url),
            CodeInline codeInline => new TextInsert(codeInline.Content),
            HtmlEntityInline htmlEntityInline => new InlineHtmlInsert(htmlEntityInline.Original.ToString()),
            HtmlInline htmlInline => new InlineHtmlInsert(htmlInline.Tag),
            LineBreakInline { IsHard: var hard } => hard ? new InlineHtmlInsert("<br />") : new TextInsert(" "),
            LiteralInline literalInline => new TextInsert(literalInline.Content.ToString()),
            _ => throw new ArgumentOutOfRangeException(nameof(inline))
          };

          newFormat = leafInline switch {
            AutolinkInline autolinkInline => newFormat with {
              Styles = newFormat.Styles.Add(Style.Link(autolinkInline.Url))
            },
            CodeInline => newFormat with {
              Code = true
            },
            _ => newFormat
          };

          document.Insert(newInsert with {
            Format = newFormat
          });
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
  }
}
