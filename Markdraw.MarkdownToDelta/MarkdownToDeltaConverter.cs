using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Markdraw.Delta;

namespace Markdraw.MarkdownToDelta
{
  public static class MarkdownToDeltaConverter
  {
    private static readonly Regex lineFormatRegex = new Regex(@"^(?:(?<code>\s{4,})|(?<quotes>(?:>\s?)+)|(?<horizontalrule>(\*|\-)(?:\s*\k<horizontalrule>){2}\s*$)|(?<bullets>\*\s)|(?<ordered>\d+.\s))*(?<headers>#{1,6}\s*)?(?<text>.*?)$", RegexOptions.Compiled);
    private static readonly Regex linkAndImageRegex = new Regex(@"(?<image>(?:(?<!\\)(?:\\\\)*!(?=\[))?)(?<!\\)(?:\\\\)*\[(?<text>[^\[]*?)(?<!\\)(?:\\\\)*\]\((?<url>.*?(?<!\\)(?:\\\\)*)\)", RegexOptions.Compiled);

    public static List<Insert> LineToInserts(string line)
    {
      throw new NotImplementedException();
    }

    public static Ops Parse(string markdown)
    {
      var linesAndFences = LineSplitter.Split(markdown);

      var ops = new Ops();

      foreach (LineOrFenced lineOrFenced in linesAndFences)
      {
        if (lineOrFenced.Fenced)
        {
          ops.Insert(new CodeInsert(lineOrFenced.Contents, lineOrFenced.InfoString)); // format infostring?
        }
        else
        {
          var inserts = LineToInserts(lineOrFenced.Contents);

          foreach (Insert insert in inserts)
          {
            ops.Insert(insert);
          }
        }
      }

      return ops;
    }
  }
}
