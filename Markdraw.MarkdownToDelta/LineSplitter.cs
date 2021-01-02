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
  }

  public static class LineSplitter
  {
    private static readonly Regex fenceOpenRegex = new Regex(@"^\s{0,3}(?<fencing>(?<fencetype>`|~)\k<fencetype>{2,})\s*?(?<infostring>[^\s`][^`]*)?$", RegexOptions.Compiled);
    private static readonly Regex fenceCloseRegex = new Regex(@"^(?<text>.+?)?(?<fencing>(?<fencetype>`|~)\k<fencetype>{2,})\s*$", RegexOptions.Compiled);

    public static List<LineOrFenced> Split(string markdown)
    {
      var lines = markdown.Split("\n");
      var linesAndFenced = new List<LineOrFenced>();

      LineOrFenced? fenced = null;
      char fenceType = '`';
      int fenceLength = 0;

      foreach (string line in lines)
      {
        if (fenced is null)
        {
          var match = fenceOpenRegex.Match(line);

          if (match is not null)
          {
            Group fencing = match.Groups["fencing"];
            Group infoString = match.Groups["infostring"];
            fenced = new LineOrFenced("", true, infoString.Success ? infoString.Value : null);
            fenceLength = fencing.Value.Length;
            fenceType = fencing.Value[0];
            linesAndFenced.Add((LineOrFenced)fenced);
          }
          else
          {
            linesAndFenced.Add(new LineOrFenced(line));
          }
        }
        else
        {
          var fencedAsFenced = (LineOrFenced)fenced;
          var match = fenceOpenRegex.Match(line);

          if (match is not null && match.Groups["fencing"].Value.Length >= fenceLength && match.Groups["fencing"].Value[0] == fenceType)
          {
            Group text = match.Groups["text"];
            if (text.Success)
            {
              fencedAsFenced.Contents += text.Value;
            }
            fenced = null;
          }
          else
          {
            fencedAsFenced.Contents += line;
          }
        }
      }

      return linesAndFenced;
    }
  }
}