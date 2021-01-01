using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Markdraw.Delta;

namespace Markdraw.MarkdownToDelta
{
  public static class MarkdownToDeltaConverter
  {
    private static readonly Regex lineFormatRegex = new Regex(@"^\s*(?:(?<quotes>(?:> ?)+)|(?<bullets>\* )|(?<ordered>\d+. )|(?<horizontalrule>(\*|\-) *\5 *\5 *$))*(?<headers>#+ )?(?<text>.*?)$", RegexOptions.Compiled | RegexOptions.Multiline);
    private static readonly Regex linkAndImageRegex = new Regex(@"(?<image>(?:(?<!\\)(?:\\\\)*!(?=\[))?)(?<!\\)(?:\\\\)*\[(?<text>[^\[]*?)(?<!\\)(?:\\\\)*\]\((?<url>.*?(?<!\\)(?:\\\\)*)\)", RegexOptions.Compiled | RegexOptions.Multiline);


    public static List<string> SplitIntoLines(string markdown)
    {
      throw new NotImplementedException();
    }

    public static List<Insert> LineToInserts(string line)
    {
      throw new NotImplementedException();
    }

    public static Ops Parse(string markdown)
    {
      var lines = SplitIntoLines(markdown);

      var ops = new Ops();

      foreach (string line in lines)
      {
        var inserts = LineToInserts(line);

        foreach (Insert insert in inserts)
        {
          ops.Insert(insert);
        }
      }

      return ops;
    }
  }
}
