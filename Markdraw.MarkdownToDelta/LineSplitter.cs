using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Markdraw.MarkdownToDelta
{
  public struct LineOrFenced
  {
    public string Contents { get; set; }
    public bool Fenced { get; set; }
    public string InfoString { get; set; }

    public LineOrFenced(string contents, bool fenced = false, string infoString = null)
    {
      Contents = contents;
      Fenced = fenced;
      InfoString = infoString;
    }

    public LineOrFenced AddLineToFenced(string line)
    {
      if (Contents == "")
      {
        Contents += line;
      }
      else
      {
        Contents += "\n" + line;
      }

      return this;
    }
  }

  public static class LineSplitter
  {
    private static readonly Regex fenceOpenRegex = new(
      @"^\s{0,3}(?<fencing>(?<fencetype>`|~)\k<fencetype>{2,})\s*?(?<infostring>[^\s`][^`]*)?$",
      RegexOptions.Compiled
    );
    private static readonly Regex fenceCloseRegex = new(
      @"^(?<text>.*?)(?<fencing>(?<fencetype>`|~)\k<fencetype>{2,})\s*$",
      RegexOptions.Compiled
    );

    public static List<LineOrFenced> Split(string markdown)
    {
      var lines = markdown.Split("\n");
      var linesAndFences = new List<LineOrFenced>();

      int? fencedIndex = null;
      var fenceType = '`';
      var fenceLength = 0;

      foreach (var line in lines)
      {
        if (fencedIndex is null)
        {
          var match = fenceOpenRegex.Match(line);

          if (match.Success)
          {
            var fencing = match.Groups["fencing"];
            var infoString = match.Groups["infostring"];
            fencedIndex = linesAndFences.Count;
            fenceLength = fencing.Value.Length;
            fenceType = fencing.Value[0];
            linesAndFences.Add(new LineOrFenced("", true, infoString.Success ? infoString.Value : null));
          }
          else
          {
            linesAndFences.Add(new LineOrFenced(line));
          }
        }
        else
        {
          var fenced = linesAndFences[(int)fencedIndex];
          var match = fenceCloseRegex.Match(line);

          if (match.Success && match.Groups["fencing"].Value.Length >= fenceLength && match.Groups["fencing"].Value[0] == fenceType)
          {
            var text = match.Groups["text"];
            if (text.Value.Length > 0)
            {
              linesAndFences[(int)fencedIndex] = fenced.AddLineToFenced(text.Value);
            }

            fencedIndex = null;
          }
          else
          {
            linesAndFences[(int)fencedIndex] = fenced.AddLineToFenced(line);
          }
        }
      }

      return linesAndFences;
    }
  }
}
