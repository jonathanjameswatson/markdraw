using System;
using System.Collections.Generic;
using Markdraw.Delta;

namespace Markdraw.MarkdownToDelta
{
  public static class MarkdownToDeltaConverter
  {
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
