using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
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
    public Ops DelimiterOps;
    public int DelimiterOpsIndex;
    public DelimiterType DelimiterChar;
    public int Number = 1;
    public Dictionary<EmphasisType, OpenerOrCloserType> OpenerOrCloser;

    public Delimiter Previous;
    public Delimiter Next;

    private static Regex whitespaceRegex = new Regex(@"\s", RegexOptions.Compiled);
    private static Regex punctuationRegex = new Regex(@"[^\w\s]", RegexOptions.Compiled);

    bool precedingWhitespace;
    bool followingWhitespace;
    bool precedingPunctuation;
    bool followingPunctuation;

    bool leftFlanking;
    bool rightFlanking;

    public Delimiter(Ops delimiterOps, int delimiterOpsIndex, char delimiterType, int number, char? preceding, char? following, Delimiter previous)
    {
      DelimiterOps = delimiterOps;
      DelimiterOpsIndex = delimiterOpsIndex;
      DelimiterChar = delimiterType == '_' ? DelimiterType.Underscore : DelimiterType.Asterisk;
      Number = number;
      Previous = previous;
      Next = null;

      precedingWhitespace = preceding is null ? true : whitespaceRegex.IsMatch(((string)preceding.ToString()));
      followingWhitespace = following is null ? true : whitespaceRegex.IsMatch(((string)following.ToString()));
      precedingPunctuation = preceding is null ? false : punctuationRegex.IsMatch(((string)preceding.ToString()));
      followingPunctuation = following is null ? false : punctuationRegex.IsMatch(((string)following.ToString()));

      leftFlanking = false;
      rightFlanking = false;

      if (!followingWhitespace && (
        !followingPunctuation || (followingPunctuation && (precedingPunctuation || precedingWhitespace))
       ))
      {
        leftFlanking = true;
      }

      if (!precedingWhitespace && (
        !precedingPunctuation || (precedingPunctuation && (followingPunctuation || followingWhitespace))
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
        if (leftFlanking && (!rightFlanking || (rightFlanking && precedingPunctuation)))
        {
          OpenerOrCloser[EmphasisType.Italic] = OpenerOrCloser[EmphasisType.Italic] | OpenerOrCloserType.Opener;
          if (Number > 1)
          {
            OpenerOrCloser[EmphasisType.Bold] = OpenerOrCloser[EmphasisType.Bold] | OpenerOrCloserType.Opener;
          }
        }
        if (rightFlanking && (!leftFlanking || (leftFlanking && precedingPunctuation)))
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
      int n = emphasis == EmphasisType.Bold ? 2 : 1;
      Number -= n;
      DelimiterOps.Transform(new Ops().Delete(n));
      SetOpenerOrCloser();
    }
  }
}