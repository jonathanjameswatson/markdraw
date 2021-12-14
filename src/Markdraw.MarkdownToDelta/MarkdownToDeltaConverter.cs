using System.Collections.Immutable;
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

namespace Markdraw.MarkdownToDelta;

public static class MarkdownToDeltaConverter
{
  public static Document Parse(string markdown)
  {
    var abstractSyntaxTree = Markdown.Parse(markdown);
    var ops = new Document();
    Write(ops, abstractSyntaxTree, ImmutableList<IEnumerator<Indent>>.Empty);
    return ops;
  }

  private static void Write(Document document, IMarkdownObject block,
    ImmutableList<IEnumerator<Indent>> indentSequences, IEnumerator<ListIndent>? listSequence = null)
  {
    var newIndentSequences = indentSequences;
    IEnumerator<ListIndent>? newListSequence = null;

    switch (block)
    {
      case BlankLineBlock:
        break;
      case ContainerBlock containerBlock:
        switch (containerBlock)
        {
          case LinkReferenceDefinitionGroup:
            return;
          case ListBlock { IsLoose: var loose, IsOrdered: var ordered, OrderedStart: var orderedStart }:
            if (ordered)
            {
              var newIndent = Indent.Number(int.Parse(orderedStart ?? "1"), loose);
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
          case ListItemBlock when listSequence is not null:
            listSequence.MoveNext();
            newIndentSequences =
              newIndentSequences.Add(SequenceHelpers.RepeatAfterFirst<Indent>(listSequence.Current,
                listSequence.Current.NextIndent));
            break;
          case ListItemBlock:
            throw new InvalidOperationException(
              "A list item block should not be read while there is no list sequence.");
          case MarkdownDocument:
            break;
          case QuoteBlock:
            newIndentSequences = newIndentSequences.Add(SequenceHelpers.RepeatAfterFirst<Indent>(Indent.Quote,
              Indent.Quote with {
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
          case FencedCodeBlock { Lines: var lines, Info: var info }:
            document.Insert(new CodeInsert(lines.ToString(), info ?? ""));
            break;
          case CodeBlock { Lines: var lines }:
            document.Insert(new CodeInsert(lines.ToString()));
            break;
          case HeadingBlock { Inline: Inline inline, Level: var level }:
            InsertInline(document, inline);
            header = level;
            break;
          case HeadingBlock { Level: var level }:
            document.Insert(new LineInsert(new LineFormat(indents, level)));
            break;
          case HtmlBlock { Lines: var lines }:
            document.Insert(new BlockHtmlInsert(lines.ToString()));
            break;
          case LinkReferenceDefinition:
            return;
          case ParagraphBlock { Inline: Inline inline }:
            InsertInline(document, inline);
            break;
          case ParagraphBlock:
            document.Insert(new LineInsert(new LineFormat(indents)));
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

  private static void InsertInline(Document document, Inline inline, InlineFormat? format = null)
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
          case EmphasisInline { DelimiterCount: < 2 }:
            newFormat = newFormat with {
              Styles = newFormat.Styles.Add(Style.Italic)
            };
            break;
          case EmphasisInline:
            newFormat = newFormat with {
              Styles = newFormat.Styles.Add(Style.Bold)
            };
            break;
          case LinkInline { IsImage: true, Url: var url, Title: var title } linkInline:
            var altText = GetText(linkInline);
            document.Insert(new ImageInsert(url ?? "", altText, title ?? "", newFormat));
            return;
          case LinkInline { Url: var url, Title: var title }:
            newFormat = newFormat with {
              Styles = newFormat.Styles.Add(Style.Link(url ?? "", title ?? ""))
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
          AutolinkInline { Url: var url } => new TextInsert(url),
          CodeInline { Content: var content } => new TextInsert(content),
          HtmlEntityInline { Original: var original } => new InlineHtmlInsert(original.ToString()),
          HtmlInline { Tag: var tag } => new InlineHtmlInsert(tag),
          LineBreakInline { IsHard: true } => new InlineHtmlInsert("<br />"),
          LineBreakInline => new TextInsert(" "),
          LiteralInline { Content: var content } => new TextInsert(content.ToString()),
          _ => throw new ArgumentOutOfRangeException(nameof(inline))
        };

        newFormat = leafInline switch {
          AutolinkInline { Url: var url } => newFormat with {
            Styles = newFormat.Styles.Add(Style.Link(url))
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
