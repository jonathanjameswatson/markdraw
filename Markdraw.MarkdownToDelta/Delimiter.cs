using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Markdraw.Delta;

namespace Markdraw.MarkdownToDelta
{
  [Flags]
  public enum OpenerOrCloserType : byte
  {
    Neither = 0,
    Opener = 1,
    Closer = 2,
    Both = Opener | Closer
  }

  public enum EmphasisType : byte
  {
    Italic = 0,
    Bold = 1
  }

  public enum DelimiterType : byte
  {
    Asterisk = 0,
    Underscore = 1
  }

  public class Delimiter
  {

    private static readonly Regex whitespaceRegex = new(@"\s", RegexOptions.Compiled);
    private static readonly Regex punctuationRegex = new(@"[^\w\s]", RegexOptions.Compiled);
    public DelimiterType DelimiterChar;
    public Ops DelimiterOps;
    public int DelimiterOpsIndex;
    private readonly bool followingPunctuation;
    private readonly bool followingWhitespace;

    private readonly bool leftFlanking;
    public Delimiter Next;
    public int Number = 1;
    public Dictionary<EmphasisType, OpenerOrCloserType> OpenerOrCloser;
    private readonly bool precedingPunctuation;

    private readonly bool precedingWhitespace;

    public Delimiter Previous;
    private readonly bool rightFlanking;

    public Delimiter(Ops delimiterOps, int delimiterOpsIndex, char delimiterType, int number, char? preceding, char? following, Delimiter previous)
    {
      DelimiterOps = delimiterOps;
      DelimiterOpsIndex = delimiterOpsIndex;
      DelimiterChar = delimiterType == '_' ? DelimiterType.Underscore : DelimiterType.Asterisk;
      Number = number;
      Previous = previous;
      Next = null;

      precedingWhitespace = preceding is null ? true : whitespaceRegex.IsMatch(preceding.ToString());
      followingWhitespace = following is null ? true : whitespaceRegex.IsMatch(following.ToString());
      precedingPunctuation = preceding is null ? false : punctuationRegex.IsMatch(preceding.ToString());
      followingPunctuation = following is null ? false : punctuationRegex.IsMatch(following.ToString());

      leftFlanking = false;
      rightFlanking = false;

      if (!followingWhitespace && (
        !followingPunctuation || followingPunctuation && (precedingPunctuation || precedingWhitespace)
      ))
      {
        leftFlanking = true;
      }

      if (!precedingWhitespace && (
        !precedingPunctuation || precedingPunctuation && (followingPunctuation || followingWhitespace)
      ))
      {
        rightFlanking = true;
      }

      SetOpenerOrCloser();
    }

    private void SetOpenerOrCloser()
    {
      OpenerOrCloser = new Dictionary<EmphasisType, OpenerOrCloserType>();
      OpenerOrCloser.Add(EmphasisType.Italic, OpenerOrCloserType.Neither);
      OpenerOrCloser.Add(EmphasisType.Bold, OpenerOrCloserType.Neither);

      if (Number == 0)
      {
        return;
      }

      if (DelimiterChar == DelimiterType.Asterisk)
      {
        if (leftFlanking)
        {
          OpenerOrCloser[EmphasisType.Italic] = OpenerOrCloser[EmphasisType.Italic] | OpenerOrCloserType.Opener;
          if (Number > 1)
          {
            OpenerOrCloser[EmphasisType.Bold] = OpenerOrCloser[EmphasisType.Bold] | OpenerOrCloserType.Opener;
          }
        }
        if (rightFlanking)
        {
          OpenerOrCloser[EmphasisType.Italic] = OpenerOrCloser[EmphasisType.Italic] | OpenerOrCloserType.Closer;
          if (Number > 1)
          {
            OpenerOrCloser[EmphasisType.Bold] = OpenerOrCloser[EmphasisType.Bold] | OpenerOrCloserType.Closer;
          }
        }
      }
      else
      {
        if (leftFlanking && (!rightFlanking || rightFlanking && precedingPunctuation))
        {
          OpenerOrCloser[EmphasisType.Italic] = OpenerOrCloser[EmphasisType.Italic] | OpenerOrCloserType.Opener;
          if (Number > 1)
          {
            OpenerOrCloser[EmphasisType.Bold] = OpenerOrCloser[EmphasisType.Bold] | OpenerOrCloserType.Opener;
          }
        }
        if (rightFlanking && (!leftFlanking || leftFlanking && precedingPunctuation))
        {
          OpenerOrCloser[EmphasisType.Italic] = OpenerOrCloser[EmphasisType.Italic] | OpenerOrCloserType.Closer;
          if (Number > 1)
          {
            OpenerOrCloser[EmphasisType.Bold] = OpenerOrCloser[EmphasisType.Bold] | OpenerOrCloserType.Closer;
          }
        }
      }
    }

    public void RemoveCharacters(EmphasisType emphasis)
    {
      var n = emphasis == EmphasisType.Bold ? 2 : 1;
      Number -= n;
      DelimiterOps.Transform(new Ops().Delete(n));
      SetOpenerOrCloser();
    }
  }
}
