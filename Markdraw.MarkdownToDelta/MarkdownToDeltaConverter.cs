using Markdraw.Delta;

namespace Markdraw.MarkdownToDelta
{
  public static class MarkdownToDeltaConverter
  {
    public static Ops Parse(string markdown)
    {
      var linesAndFences = LineSplitter.Split(markdown);

      var ops = new Ops();

      foreach (var lineOrFenced in linesAndFences)
      {
        if (lineOrFenced.Fenced)
        {
          ops.Insert(new CodeInsert(lineOrFenced.Contents, lineOrFenced.InfoString));// format infostring?
          // ops.Insert(new LineInsert());
        }
        else
        {
          var inserts = LineToDeltaConverter.Parse(lineOrFenced.Contents);

          ops.InsertMany(inserts);
        }
      }

      return ops;
    }
  }
}
