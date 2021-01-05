using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Markdraw.Delta;

namespace Markdraw.MarkdownToDelta
{
  public static class MarkdownToDeltaConverter
  {
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
          var inserts = LineToDeltaConverter.Parse(lineOrFenced.Contents);

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
