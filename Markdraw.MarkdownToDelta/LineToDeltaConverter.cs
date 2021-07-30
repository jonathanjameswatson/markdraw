using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using Markdraw.Delta;

namespace Markdraw.MarkdownToDelta
{
  public static class LineToDeltaConverter
  {
    private static readonly Regex IndentRegex = new(
      @"^(?<indent>(?<whitespace>\s+)|(?<quotes>>)|(?<horizontalrule>(?<hrstart>\*|\-|_)(?:\s{0,3}\k<hrstart>){2}\s*$)|(?<bullet>[\*\-+](?=\s))|(?<number>\d{1,9}[.)](?=\s)))?(?<text>.*)$",
      RegexOptions.Compiled
    );
    private static readonly Regex HeaderRegex = new(
      @"^(?<header>#{1,6}(?=\s))?\s*(?<text>.*)$",
      RegexOptions.Compiled
    );
    private static readonly Regex LinkAndImageRegex = new(
      @"(?<!\\)(?<escapedbackslashes>(?:\\\\)+)?(?<image>!?)\[(?<text>[^\[]*?)(?<!\\)(?:\\\\)*\]\((?<url>.*?(?<!\\)(?:\\\\)*)\)",
      RegexOptions.Compiled
    );
    private static readonly Regex NonCapturingLinkAndImageRegex = new(
      @"(?<!\\)(?<escapedbackslashes>(?:\\\\)+)?(?:!?)\[(?:[^\[]*?)(?<!\\)(?:\\\\)*\]\((?:.*?(?<!\\)(?:\\\\)*)\)",
      RegexOptions.Compiled
    );
    private static readonly Regex AsciiPunctuationRegex = new(
      @"[!""#$%&'()*+,\-.\/:;<=>?@[\\\]^_`{|}~]",
      RegexOptions.Compiled
    );

    public static Ops Parse(string line)
    {
      var ops = new Ops();
      var header = 0;
      var indents = new List<Indent>();

      var text = line;
      var indentMatch = IndentRegex.Match(text);

      int? codeMatch = null;

      while (indentMatch.Groups["indent"].Success)
      {
        var indentType = Indent.Empty(0);

        if (indentMatch.Groups["whitespace"].Success)
        {
          var length = indentMatch.Groups["whitespace"].Length;
          if (length % 4 == 0)
          {
            indentType = Indent.Code;
            codeMatch = length;
          }
          else
          {
            indentType = Indent.Empty(length);
          }
        }
        else if (indentMatch.Groups["quotes"].Success)
        {
          indentType = Indent.Quote;
        }
        else if (indentMatch.Groups["horizontalrule"].Success)
        {
          return ops.Insert(new DividerInsert()).Insert(new LineInsert(new LineFormat {
            Indents = indents.ToImmutableList()
          }));
        }
        else if (indentMatch.Groups["bullet"].Success)
        {
          indentType = Indent.Bullet;
        }
        else if (indentMatch.Groups["number"].Success)
        {
          indentType = Indent.Number(indentMatch.Groups["number"].Length);
        }

        if (indentType is not null)
        {
          indents.Add(indentType);
        }
        text = indentMatch.Groups["text"].Value;

        if (codeMatch is not null)
        {
          // text = text.Insert(0, String.Concat(Enumerable.Repeat("    ", (int)codeMatch / 4 - 1)));
          break;
        }

        indentMatch = IndentRegex.Match(text);
      }

      if (codeMatch is null)
      {
        var headerMatch = HeaderRegex.Match(text);

        if (headerMatch.Groups["header"].Success)
        {
          header = headerMatch.Groups["header"].Value.Length;
        }

        text = headerMatch.Groups["text"].Value;
      }

      var textOps = LinksAndImagesWithTextToDelta(text);

      ops.InsertMany(textOps);

      ops.Insert(new LineInsert(new LineFormat {
        Indents = indents.ToImmutableList(), Header = header
      }));

      return ops;
    }

    private static Ops LinksAndImagesWithTextToDelta(string text)
    {
      var ops = new Ops();
      var matches = LinkAndImageRegex.Matches(text);
      var nonMatches = NonCapturingLinkAndImageRegex.Split(text);
      var i = 0;

      foreach (var nonMatch in nonMatches)
      {
        ops.InsertMany(TextToDelta(nonMatch));

        if (i < matches.Count)
        {
          var match = matches[i];

          if (match.Groups["image"].Value != "")
          {
            ops.Insert(new ImageInsert(match.Groups["url"].Value, match.Groups["text"].Value));
          }
          else
          {
            var linksOps = TextToDelta(match.Groups["text"].Value);

            var characters = linksOps.Characters;
            if (characters > 0)
            {
              linksOps.Transform(
                new Ops().Retain(
                  characters,
                  new TextFormat {
                    Bold = null, Italic = null, Link = match.Groups["url"].Value
                  }
                )
              );
            }

            ops.InsertMany(linksOps);
          }
        }

        i += 1;
      }

      return ops;
    }

    private static Ops TextToDelta(string text)
    {
      var opsList = new List<Ops>();
      Delimiter delimiterStack = null;

      var i = -1;
      var opStart = 0;
      var delimiterStart = 0;
      var cancelled = false;
      char? preceding = null;
      var consecutiveDelimiters = 0;
      var previousConsecutiveDelimiters = 0;
      var delimiterType = '*';

      foreach (var x in text)
      {
        i += 1;
        previousConsecutiveDelimiters = consecutiveDelimiters;

        if (!cancelled && (
          consecutiveDelimiters == 0 && (x == '*' || x == '_')
          || consecutiveDelimiters > 0 && x == delimiterType
        ))
        {
          consecutiveDelimiters += 1;
          delimiterType = x;
        }
        else
        {
          consecutiveDelimiters = 0;
        }

        if (consecutiveDelimiters == 1)
        {
          delimiterStart = i;
        }

        if (consecutiveDelimiters == 0 && previousConsecutiveDelimiters != 0)
        {
          var textToInsert = text.Substring(opStart, delimiterStart - opStart);
          if (textToInsert != "")
          {
            var textOps = new Ops().Insert(textToInsert);
            opsList.Add(textOps);
          }

          var delimiterOps = new Ops().Insert(
            text.Substring(delimiterStart, previousConsecutiveDelimiters)
          );
          opsList.Add(delimiterOps);
          delimiterStack = new Delimiter(
            delimiterOps,
            opsList.Count - 1,
            delimiterType,
            previousConsecutiveDelimiters,
            preceding,
            x,
            delimiterStack
          );
          if (delimiterStack.Previous is not null)
          {
            delimiterStack.Previous.Next = delimiterStack;
          }
          opStart = delimiterStart + previousConsecutiveDelimiters;
        }

        if (consecutiveDelimiters == 0)
        {
          preceding = x;
        }

        if (cancelled)
        {
          cancelled = false;
        }
        else if (x == '\\')
        {
          cancelled = true;
        }
      }

      if (consecutiveDelimiters == 0)
      {
        var textToInsert = text.Substring(opStart, text.Length - opStart);
        if (textToInsert != "")
        {
          var textOps = new Ops().Insert(textToInsert);
          opsList.Add(textOps);
        }
      }
      else
      {
        var textToInsert = text.Substring(opStart, delimiterStart - opStart);
        if (textToInsert != "")
        {
          var textOps = new Ops().Insert(textToInsert);
          opsList.Add(textOps);
        }

        var delimiterOps = new Ops().Insert(
          text.Substring(delimiterStart, consecutiveDelimiters)
        );
        opsList.Add(delimiterOps);
        delimiterStack = new Delimiter(
          delimiterOps,
          opsList.Count - 1,
          delimiterType,
          consecutiveDelimiters,
          preceding,
          null,
          delimiterStack
        );
        if (delimiterStack.Previous is not null)
        {
          delimiterStack.Previous.Next = delimiterStack;
        }
      }

      var openersBottom = new Dictionary<ValueTuple<EmphasisType, DelimiterType>, Delimiter>();
      foreach (var emphasis in new List<EmphasisType> {
        EmphasisType.Italic, EmphasisType.Bold
      })
      {
        foreach (var delimiter in new List<DelimiterType> {
          DelimiterType.Asterisk, DelimiterType.Underscore
        })
        {
          openersBottom.Add((emphasis, delimiter), null);
        }
      }

      var currentDelimiter = delimiterStack;

      while (currentDelimiter?.Previous is not null)
      {
        currentDelimiter = currentDelimiter.Previous;
      }

      while (currentDelimiter is not null)
      {
        if (!currentDelimiter.OpenerOrCloser[EmphasisType.Italic].HasFlag(OpenerOrCloserType.Closer)
          && !currentDelimiter.OpenerOrCloser[EmphasisType.Bold].HasFlag(OpenerOrCloserType.Closer))
        {
          currentDelimiter = currentDelimiter.Next;
          continue;
        }

        var possibleEmphasis = new List<EmphasisType>();
        if (currentDelimiter.OpenerOrCloser[EmphasisType.Bold].HasFlag(OpenerOrCloserType.Closer))
        {
          possibleEmphasis.Add(EmphasisType.Bold);
        }
        if (currentDelimiter.OpenerOrCloser[EmphasisType.Italic].HasFlag(OpenerOrCloserType.Closer))
        {
          possibleEmphasis.Add(EmphasisType.Italic);
        }

        var currentOpenerBottoms = possibleEmphasis.Select(
          emphasis => openersBottom[(emphasis, currentDelimiter.DelimiterChar)]
        );

        var opener = currentDelimiter.Previous;
        var found = false;
        while (opener is not null && !currentOpenerBottoms.Contains(opener))
        {
          if ((
              possibleEmphasis.Contains(EmphasisType.Bold) &&
              opener.OpenerOrCloser[EmphasisType.Bold].HasFlag(OpenerOrCloserType.Opener) || possibleEmphasis.Contains(EmphasisType.Italic) &&
              opener.OpenerOrCloser[EmphasisType.Italic].HasFlag(OpenerOrCloserType.Opener)
            ) && opener.DelimiterChar == currentDelimiter.DelimiterChar
          )
          {
            found = true;
            break;
          }
          opener = opener.Previous;
        }

        if (found)
        {
          EmphasisType emphasis;
          TextFormat newFormat;
          if (opener.OpenerOrCloser[EmphasisType.Bold].HasFlag(OpenerOrCloserType.Opener)
            && currentDelimiter.OpenerOrCloser[EmphasisType.Bold].HasFlag(OpenerOrCloserType.Closer)
          )
          {
            emphasis = EmphasisType.Bold;
            newFormat = new TextFormat {
              Bold = true, Italic = null, Link = null
            };
          }
          else
          {
            emphasis = EmphasisType.Italic;
            newFormat = new TextFormat {
              Bold = null, Italic = true, Link = null
            };
          }

          currentDelimiter.RemoveCharacters(emphasis);
          opener.RemoveCharacters(emphasis);

          for (var j = opener.DelimiterOpsIndex + 1; j < currentDelimiter.DelimiterOpsIndex; j++)
          {
            var toFormat = opsList[j];
            var characters = toFormat.Characters;
            if (characters > 0)
            {
              toFormat.Transform(
                new Ops().Retain(characters, newFormat)
              );
            }
          }

          currentDelimiter.Previous = opener;
          opener.Next = currentDelimiter;

          possibleEmphasis = new List<EmphasisType>();

          if (currentDelimiter.OpenerOrCloser[EmphasisType.Bold].HasFlag(OpenerOrCloserType.Closer))
          {
            possibleEmphasis.Add(EmphasisType.Bold);
          }
          if (currentDelimiter.OpenerOrCloser[EmphasisType.Italic].HasFlag(OpenerOrCloserType.Closer))
          {
            possibleEmphasis.Add(EmphasisType.Italic);
          }

          currentOpenerBottoms = possibleEmphasis.Select(
            emphasis => openersBottom[(emphasis, currentDelimiter.DelimiterChar)]
          );

          if (currentDelimiter.DelimiterOps.Characters == 0)
          {
            currentDelimiter = currentDelimiter.Next;
          }
        }
        else
        {
          foreach (var emphasis in possibleEmphasis)
          {
            openersBottom[(emphasis, currentDelimiter.DelimiterChar)] = currentDelimiter.Previous;
          }

          if (!currentDelimiter.OpenerOrCloser[EmphasisType.Italic].HasFlag(OpenerOrCloserType.Opener)
            && !currentDelimiter.OpenerOrCloser[EmphasisType.Bold].HasFlag(OpenerOrCloserType.Opener))
          {
            currentDelimiter.Previous.Next = currentDelimiter.Next;
          }

          currentDelimiter = currentDelimiter.Next;
        }
      }

      var allOps = new Ops();
      opsList.Reverse();
      foreach (var ops in opsList)
      {
        allOps.Transform(ops);
      }

      return allOps;
    }
  }
}
