using System.Collections.Generic;
using Xunit;

namespace Markdraw.Delta.Test
{
  public class TransformTest
  {
    [Fact]
    public void RetainAndInsert_Appends()
    {
      new Ops()
        .Insert("A")
        .Transform(new Ops().Retain(1).Insert("B"))
        .Is(new Ops().Insert("AB"));

      new Ops()
        .Insert("A")
        .Transform(new Ops().Retain(1).Insert("B", TextFormat.BoldPreset))
        .Is(new Ops().Insert("A").Insert("B", TextFormat.BoldPreset));
    }

    [Fact]
    public void Retain_FormatsText()
    {
      var turnBold = new TextFormat(true, null, null);

      new Ops()
        .Insert("A")
        .Transform(new Ops().Retain(1, turnBold))
        .Is(new Ops().Insert("A", TextFormat.BoldPreset));

      new Ops()
        .Insert("AA")
        .Transform(new Ops().Retain(1, turnBold))
        .Is(new Ops().Insert("A", TextFormat.BoldPreset).Insert("A"));

      new Ops()
        .Insert("AA")
        .Transform(new Ops().Retain(1).Retain(1, turnBold))
        .Is(new Ops().Insert("A").Insert("A", TextFormat.BoldPreset));

      new Ops()
        .Insert("A")
        .Insert("A", (TextFormat)TextFormat.BoldPreset.Clone())
        .Transform(new Ops().Retain(1, turnBold))
        .Is(new Ops().Insert("AA", TextFormat.BoldPreset));

      new Ops()
        .Insert("A")
        .Insert("A", (TextFormat)TextFormat.BoldPreset.Clone())
        .Transform(new Ops().Retain(2, turnBold))
        .Is(new Ops().Insert("AA", TextFormat.BoldPreset));

      new Ops()
        .Insert("A", (TextFormat)TextFormat.BoldPreset.Clone())
        .Insert("A", (TextFormat)TextFormat.ItalicPreset.Clone())
        .Transform(new Ops().Retain(2, turnBold))
        .Is(new Ops()
          .Insert("A", TextFormat.BoldPreset)
          .Insert("A", new TextFormat(true, true, ""))
        );

      new Ops()
      .Insert("AA", (TextFormat)TextFormat.BoldPreset.Clone())
      .Insert("AA", (TextFormat)TextFormat.ItalicPreset.Clone())
      .Transform(new Ops().Retain(1).Retain(2, TextFormat.BoldPreset))
      .Is(new Ops()
        .Insert("AAA", TextFormat.BoldPreset)
        .Insert("A", TextFormat.ItalicPreset)
      );

      new Ops()
      .Insert("AAA")
      .Transform(new Ops().Retain(1).Retain(1, TextFormat.BoldPreset))
      .Is(new Ops()
        .Insert("A")
        .Insert("A", TextFormat.BoldPreset)
        .Insert("A")
      );
    }

    [Fact]
    public void Retain_FormatsLines()
    {
      var turnQuote = new LineFormat(new List<Indent>() { Indent.Quote, Indent.Empty(1) }, null);

      new Ops()
        .Insert(new LineInsert())
        .Transform(new Ops().Retain(1, turnQuote))
        .Is(new Ops().Insert(new LineInsert(LineFormat.QuotePreset)));

      new Ops()
        .Insert("A")
        .Insert(new LineInsert())
        .Insert("A")
        .Transform(new Ops().Retain(3, turnQuote))
        .Is(new Ops()
          .Insert("A")
          .Insert(new LineInsert(LineFormat.QuotePreset))
          .Insert("A")
        );
    }

    [Fact]
    public void Delete_DeletesText()
    {
      new Ops()
        .Insert("A")
        .Transform(new Ops().Delete(1))
        .Is(new Ops());

      new Ops()
        .Insert("AA")
        .Transform(new Ops().Delete(1))
        .Is(new Ops().Insert("A"));

      new Ops()
        .Insert("AA")
        .Transform(new Ops().Retain(1).Delete(1))
        .Is(new Ops().Insert("A"));

      new Ops()
        .Insert("AAA")
        .Transform(new Ops().Retain(1).Delete(1))
        .Is(new Ops().Insert("AA"));
    }

    [Fact]
    public void Insert_InsertsInText()
    {
      new Ops()
        .Insert("AA")
        .Transform(new Ops().Retain(1).Insert("A"))
        .Is(new Ops().Insert("AAA"));

      new Ops()
        .Insert("AA")
        .Transform(new Ops().Retain(1).Insert("A", TextFormat.BoldPreset))
        .Is(new Ops().Insert("A").Insert("A", TextFormat.BoldPreset).Insert("A"));

      new Ops()
        .Insert("AB", new TextFormat(false, false, "C"))
        .Transform(new Ops().Retain(1).Insert("D"))
        .Is(
          new Ops()
            .Insert("A", new TextFormat(false, false, "C"))
            .Insert("D")
            .Insert("B", new TextFormat(false, false, "C")));
    }
  }
}
