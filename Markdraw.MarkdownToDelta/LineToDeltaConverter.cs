using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Markdraw.Delta;

namespace Markdraw.MarkdownToDelta
{
  public static class LineToDeltaConverter
  {
    private static readonly Regex indentRegex = new Regex(
      @"^(?<indent>(?<code>\s{4})|(?<quotes>>)|(?<horizontalrule>(?<hrstart>\*|\-)(?:\s{0,3}\k<hrstart>){2}\s*$)|(?<bullet>[\*-](?=\s))|(?<number>\d+.(?=\s)))?(?<whitespace>(?!\s{4})\s{0,3})?(?<text>.*)$",
      RegexOptions.Compiled
    );
    private static readonly Regex headerRegex = new Regex(
      @"^(?<header>#{1,6}(?=\s))?\s*(?<text>.*)$",
      RegexOptions.Compiled
    );
    private static readonly Regex linkAndImageRegex = new Regex(
      @"(?<!\\)(?<escapedbackslashes>(?:\\\\)+)?(?<image>!?)\[(?<text>[^\[]*?)(?<!\\)(?:\\\\)*\]\((?<url>.*?(?<!\\)(?:\\\\)*)\)",
      RegexOptions.Compiled
    );
    private static readonly Regex nonCapturingLinkAndImageRegex = new Regex(
      @"(?<!\\)(?<escapedbackslashes>(?:\\\\)+)?(?:!?)\[(?:[^\[]*?)(?<!\\)(?:\\\\)*\]\((?:.*?(?<!\\)(?:\\\\)*)\)",
      RegexOptions.Compiled
    );
    private static readonly Regex asciiPunctuationRegex = new Regex(
      @"[!""#$%&'()*+,\-.\/:;<=>?@[\\\]^_`{|}~]",
      RegexOptions.Compiled
    );

    public static Ops Parse(string line)
    {
      var ops = new Ops();
      var lineFormat = new LineFormat();

      string text = line;
      var indentMatch = indentRegex.Match(text);

      bool codeMatch = false;

      while (indentMatch.Groups["indent"].Success)
      {
        var indentType = Indent.Code;

        if (indentMatch.Groups["code"].Success)
        {
          codeMatch = true;
        }
        else if (indentMatch.Groups["quotes"].Success)
        {
          indentType = Indent.Quote;
        }
        else if (indentMatch.Groups["horizontalrule"].Success)
        {
          ops.Insert(new DividerInsert());
          break;
        }
        else if (indentMatch.Groups["bullet"].Success)
        {
          indentType = Indent.Bullet;
        }
        else if (indentMatch.Groups["number"].Success)
        {
          indentType = Indent.Number;
        }

        lineFormat.Indents.Add(indentType);
        text = indentMatch.Groups["text"].Value;

        if (codeMatch)
        {
          text = text.Insert(0, indentMatch.Groups["whitespace"].Value);
          break;
        }

        indentMatch = indentRegex.Match(text);
      }

      if (!codeMatch)
      {
        var headerMatch = headerRegex.Match(text);

        if (headerMatch.Groups["header"].Success)
        {
          lineFormat.Header = headerMatch.Groups["header"].Value.Length;
        }

        text = headerMatch.Groups["text"].Value;
      }

      var textOps = LinksAndImagesWithTextToDelta(text);

      ops.InsertMany(textOps);

      ops.Insert(new LineInsert(lineFormat));

      return ops;
    }

    private static Ops LinksAndImagesWithTextToDelta(string text)
    {
      var ops = new Ops();
      var matches = linkAndImageRegex.Matches(text);
      var nonMatches = nonCapturingLinkAndImageRegex.Split(text);
      int i = 0;

      foreach (string nonMatch in nonMatches)
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

            int characters = linksOps.Characters;
            if (characters > 0)
            {
              linksOps.Transform(
                new Ops().Retain(
                  characters,
                  new TextFormat(null, null, match.Groups["url"].Value)
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

      int i = -1;
      int opStart = 0;
      int delimiterStart = 0;
      bool cancelled = false;
      char? preceding = null;
      int consecutiveDelimiters = 0;
      int previousConsecutiveDelimiters = 0;
      char delimiterType = '*';

      foreach (char x in text)
      {
        i += 1;
        previousConsecutiveDelimiters = consecutiveDelimiters;

        if (!cancelled && (
          (consecutiveDelimiters == 0 && (x == '*' || x == '_'))
          || (consecutiveDelimiters > 0 && x == delimiterType)
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
          string textToInsert = text.Substring(opStart, delimiterStart - opStart);
          if (textToInsert != "")
          {
            var textOps = new Ops().Insert(textToInsert);
            opsList.Add(textOps);
          }

          var delimiterOps = new Ops().Insert(
            text.Substring(delimiterStart, previousConsecutiveDelimiters)
          );
          opsList.Add(delimiterOps);
          delimiterStack = (new Delimiter(
            delimiterOps,
            opsList.Count - 1,
            delimiterType,
            previousConsecutiveDelimiters,
            preceding,
            x,
            delimiterStack
          ));
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
        string textToInsert = text.Substring(opStart, text.Length - opStart);
        if (textToInsert != "")
        {
          var textOps = new Ops().Insert(textToInsert);
          opsList.Add(textOps);
        }
      }
      else
      {
        string textToInsert = text.Substring(opStart, delimiterStart - opStart);
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
      foreach (var emphasis in new List<EmphasisType>() { EmphasisType.Italic, EmphasisType.Bold })
      {
        foreach (var delimiter in new List<DelimiterType>()
        {
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
        bool found = false;
        while (opener is not null && !currentOpenerBottoms.Contains(opener))
        {
          if ((
              (
                possibleEmphasis.Contains(EmphasisType.Bold) &&
                opener.OpenerOrCloser[EmphasisType.Bold].HasFlag(OpenerOrCloserType.Opener)
              ) || (
                possibleEmphasis.Contains(EmphasisType.Italic) &&
                opener.OpenerOrCloser[EmphasisType.Italic].HasFlag(OpenerOrCloserType.Opener)
              )
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
            newFormat = new TextFormat(true, null, null);
          }
          else
          {
            emphasis = EmphasisType.Italic;
            newFormat = new TextFormat(null, true, null);
          }

          currentDelimiter.RemoveCharacters(emphasis);
          opener.RemoveCharacters(emphasis);

          for (int j = opener.DelimiterOpsIndex + 1; j < currentDelimiter.DelimiterOpsIndex; j++)
          {
            var toFormat = opsList[j];
            int characters = toFormat.Characters;
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
