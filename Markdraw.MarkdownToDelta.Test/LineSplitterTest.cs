using System.Collections.Generic;
using Xunit;

namespace Markdraw.MarkdownToDelta.Test
{
  public class LineSplitterTest
  {
    [Fact]
    public void EmptyString_IsEmptyList()
    {
      LineSplitter
        .Split("")
        .Is(new List<LineOrFenced>() { new LineOrFenced("") });
    }

    [Fact]
    public void JustOpening_IsEmptyCode()
    {
      LineSplitter
        .Split("```")
        .Is(new List<LineOrFenced>() { new LineOrFenced("", true, null) });
    }

    [Fact]
    public void InfoString_IsFound()
    {
      LineSplitter
        .Split("```python")
        .Is(new List<LineOrFenced>() { new LineOrFenced("", true, "python") });
    }

    [Fact]
    public void JustCodeBlock_IsFilledCode()
    {
      LineSplitter
        .Split("```\nLine 1\nLine 2\n```")
        .Is(new List<LineOrFenced>() { new LineOrFenced("Line 1\nLine 2", true, null) });
    }

    [Fact]
    public void EndingTicks_IsFilledCode_WithoutNewLineBefore()
    {
      LineSplitter
        .Split("```\nLine 1\nLine 2```")
        .Is(new List<LineOrFenced>() { new LineOrFenced("Line 1\nLine 2", true, null) });
    }

    [Fact]
    public void Tildes_IsFilledCode()
    {
      LineSplitter
        .Split("~~~\nLine 1\nLine 2\n~~~")
        .Is(new List<LineOrFenced>() { new LineOrFenced("Line 1\nLine 2", true, null) });

      LineSplitter
        .Split("~~~\nLine 1\nLine 2\n```")
        .IsNot(new List<LineOrFenced>() { new LineOrFenced("Line 1\nLine 2", true, null) });

      LineSplitter
        .Split("~`~\nLine 1\nLine 2\n~`~")
        .IsNot(new List<LineOrFenced>() { new LineOrFenced("Line 1\nLine 2", true, null) });
    }

    [Fact]
    public void JustCodeBlock_IsFilledCode_WithFourTicks()
    {
      LineSplitter
        .Split("````\nLine 1\nLine 2\n````")
        .Is(new List<LineOrFenced>() { new LineOrFenced("Line 1\nLine 2", true, null) });
    }

    [Fact]
    public void NotSameNumberOrTicks_IsNotFilledCode()
    {
      LineSplitter
        .Split("````\nLine 1```\nLine 2\n````")
        .Is(new List<LineOrFenced>() { new LineOrFenced("Line 1```\nLine 2", true, null) });
    }
  }
}
