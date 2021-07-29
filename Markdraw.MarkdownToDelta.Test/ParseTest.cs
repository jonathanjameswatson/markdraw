using System.Collections.Immutable;
using Markdraw.Delta;
using Xunit;

namespace Markdraw.MarkdownToDelta.Test
{
  public class ParseTest
  {
    [Fact]
    public void EmptyString_IsLineInsert()
    {
      MarkdownToDeltaConverter
        .Parse("")
        .Is(
          new Ops()
            .Insert(new LineInsert())
        );
    }

    [Fact]
    public void AString_IsAAndLineInsert()
    {
      MarkdownToDeltaConverter
        .Parse("A")
        .Is(
          new Ops()
            .Insert("A")
            .Insert(new LineInsert())
        );
    }

    [Fact]
    public void Code_IsHandled()
    {
      MarkdownToDeltaConverter
        .Parse("```code\nline1\nline2\n```")
        .Is(
          new Ops()
            .Insert(new CodeInsert("line1\nline2", "code"))
        );
    }

    [Fact]
    public void Combination_IsHandled()
    {
      MarkdownToDeltaConverter
        .Parse("# Test\n```code\nline1\nline2\n```\n\n1. > *Italic* test")
        .Is(
          new Ops()
            .Insert("Test")
            .Insert(new LineInsert(new LineFormat { Header = 1 }))
            .Insert(new CodeInsert("line1\nline2", "code"))
            .Insert(new LineInsert())
            .Insert("Italic", TextFormat.ItalicPreset)
            .Insert(" test")
            .Insert(
              new LineInsert(
                new LineFormat {
                  Indents = ImmutableList.Create(
                    Indent.Number(2),
                    Indent.Empty(1),
                    Indent.Quote,
                    Indent.Empty(1)
                  )}
                )
              )
        );
    }
  }
}
