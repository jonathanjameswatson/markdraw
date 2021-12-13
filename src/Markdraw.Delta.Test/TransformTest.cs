using System.Collections.Immutable;
using Markdraw.Delta.Formats;
using Markdraw.Delta.Indents;
using Markdraw.Delta.Operations.Inserts;
using Markdraw.Delta.OperationSequences;
using Markdraw.Delta.Styles;
using Xunit;

namespace Markdraw.Delta.Test
{
  public class TransformTest
  {
    [Fact]
    public void RetainAndInsert_Appends()
    {
      new Document().Insert("A").Transform(new Transformation().Retain(1).Insert("B")).Is(new Document().Insert("AB"));

      new Document().Insert("A").Transform(new Transformation().Retain(1).Insert("B", InlineFormat.BoldPreset))
        .Is(new Document().Insert("A").Insert("B", InlineFormat.BoldPreset));
    }

    [Fact]
    public void Retain_FormatsText()
    {
      var turnBold = new InlineFormatModifier(styles => styles.Add(Style.Bold));

      new Document().Insert("A").Transform(new Transformation().Retain(1, turnBold))
        .Is(new Document().Insert("A", InlineFormat.BoldPreset));

      new Document().Insert("AA").Transform(new Transformation().Retain(1, turnBold))
        .Is(new Document().Insert("A", InlineFormat.BoldPreset).Insert("A"));

      new Document().Insert("AA").Transform(new Transformation().Retain(1).Retain(1, turnBold))
        .Is(new Document().Insert("A").Insert("A", InlineFormat.BoldPreset));

      new Document().Insert("A").Insert("A", InlineFormat.BoldPreset)
        .Transform(new Transformation().Retain(1, turnBold)).Is(new Document().Insert("AA", InlineFormat.BoldPreset));

      new Document().Insert("A").Insert("A", InlineFormat.BoldPreset)
        .Transform(new Transformation().Retain(2, turnBold)).Is(new Document().Insert("A", InlineFormat.BoldPreset)
          .Insert("A", new InlineFormat(ImmutableList.Create<Style>(Style.Bold, Style.Bold))));

      new Document().Insert("A", InlineFormat.BoldPreset).Insert("A", InlineFormat.ItalicPreset)
        .Transform(new Transformation().Retain(2, turnBold)).Is(new Document()
          .Insert("A", new InlineFormat(ImmutableList.Create<Style>(Style.Bold, Style.Bold))).Insert("A",
            new InlineFormat(ImmutableList.Create<Style>(Style.Italic, Style.Bold))));

      new Document().Insert("AA", InlineFormat.BoldPreset).Insert("AA", InlineFormat.ItalicPreset)
        .Transform(new Transformation().Retain(1).Retain(2, turnBold)).Is(new Document()
          .Insert("A", InlineFormat.BoldPreset)
          .Insert("A", new InlineFormat(ImmutableList.Create<Style>(Style.Bold, Style.Bold)))
          .Insert("A", new InlineFormat(ImmutableList.Create<Style>(Style.Italic, Style.Bold)))
          .Insert("A", InlineFormat.ItalicPreset));

      new Document().Insert("AAA").Transform(new Transformation().Retain(1).Retain(1, turnBold))
        .Is(new Document().Insert("A").Insert("A", InlineFormat.BoldPreset).Insert("A"));
    }

    [Fact]
    public void Retain_FormatsLines()
    {
      var turnQuote = new LineFormatModifier(indents => indents.Insert(0, Indent.Quote));

      new LineInsert(new LineFormat {
        Indents = ImmutableList.Create<Indent>(Indent.Quote)
      }).Is(new LineInsert(new LineFormat {
        Indents = ImmutableList.Create<Indent>(Indent.Quote)
      }));

      new Document().Insert(new LineInsert()).Transform(new Transformation().Retain(1, turnQuote))
        .Is(new Document().Insert(new LineInsert(LineFormat.QuotePreset)));

      new Document().Insert("A").Insert(new LineInsert()).Insert("A")
        .Transform(new Transformation().Retain(3, turnQuote))
        .Is(new Document().Insert("A").Insert(new LineInsert(LineFormat.QuotePreset)).Insert("A"));
    }

    [Fact]
    public void Delete_DeletesText()
    {
      new Document().Insert("A").Transform(new Transformation().Delete(1)).Is(new Document());

      new Document().Insert("AA").Transform(new Transformation().Delete(1)).Is(new Document().Insert("A"));

      new Document().Insert("AA").Transform(new Transformation().Retain(1).Delete(1)).Is(new Document().Insert("A"));

      new Document().Insert("AAA").Transform(new Transformation().Retain(1).Delete(1)).Is(new Document().Insert("AA"));
    }

    [Fact]
    public void Insert_InsertsInText()
    {
      new Document().Insert("AA").Transform(new Transformation().Retain(1).Insert("A"))
        .Is(new Document().Insert("AAA"));

      new Document().Insert("AA").Transform(new Transformation().Retain(1).Insert("A", InlineFormat.BoldPreset))
        .Is(new Document().Insert("A").Insert("A", InlineFormat.BoldPreset).Insert("A"));

      new Document().Insert("AB", new InlineFormat(ImmutableList.Create<Style>(Style.Link("C"))))
        .Transform(new Transformation().Retain(1).Insert("D")).Is(new Document()
          .Insert("A", new InlineFormat(ImmutableList.Create<Style>(Style.Link("C")))).Insert("D")
          .Insert("B", new InlineFormat(ImmutableList.Create<Style>(Style.Link("C")))));
    }

    [Fact]
    public void Retain_FormatsTextWithNewLine()
    {
      var turnBold = new InlineFormatModifier(styles => styles.Add(Style.Bold));

      new Document().Insert("AAA").Insert(new LineInsert())
        .Transform(new Transformation().Retain(1).Retain(1, turnBold)).Is(new Document().Insert("A")
          .Insert("A", InlineFormat.BoldPreset).Insert("A").Insert(new LineInsert()));
    }

    [Fact]
    public void Insert_InsertsWithNewLine()
    {
      new Document().Insert("AAA").Insert(new LineInsert()).Transform(new Transformation().Retain(3).Insert("A"))
        .Is(new Document().Insert("AAAA").Insert(new LineInsert()));

      new Document().Insert("AAA").Insert(new LineInsert())
        .Transform(new Transformation().Retain(1).Insert(new LineInsert()).Insert("B")).Is(new Document().Insert("A")
          .Insert(new LineInsert()).Insert("BAA").Insert(new LineInsert()));
    }
  }
}
