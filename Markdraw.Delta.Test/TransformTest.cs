using System.Collections.Immutable;
using Markdraw.Delta.Formats;
using Markdraw.Delta.Indents;
using Markdraw.Delta.Operations.Inserts;
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
      var turnBold = new TextFormat {
        Bold = true, Italic = null, Link = null
      };

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
        .Insert("A", TextFormat.BoldPreset)
        .Transform(new Ops().Retain(1, turnBold))
        .Is(new Ops().Insert("AA", TextFormat.BoldPreset));

      new Ops()
        .Insert("A")
        .Insert("A", TextFormat.BoldPreset)
        .Transform(new Ops().Retain(2, turnBold))
        .Is(new Ops().Insert("AA", TextFormat.BoldPreset));

      new Ops()
        .Insert("A", TextFormat.BoldPreset)
        .Insert("A", TextFormat.ItalicPreset)
        .Transform(new Ops().Retain(2, turnBold))
        .Is(new Ops()
          .Insert("A", TextFormat.BoldPreset)
          .Insert("A", new TextFormat {
            Bold = true, Italic = true
          })
        );

      new Ops()
        .Insert("AA", TextFormat.BoldPreset)
        .Insert("AA", TextFormat.ItalicPreset)
        .Transform(new Ops().Retain(1).Retain(2, turnBold))
        .Is(new Ops()
          .Insert("AA", TextFormat.BoldPreset)
          .Insert("A", new TextFormat {
            Bold = true, Italic = true
          })
          .Insert("A", TextFormat.ItalicPreset)
        );

      new Ops()
        .Insert("AAA")
        .Transform(new Ops().Retain(1).Retain(1, turnBold))
        .Is(new Ops()
          .Insert("A")
          .Insert("A", TextFormat.BoldPreset)
          .Insert("A")
        );
    }

    [Fact]
    public void Retain_FormatsLines()
    {
      var turnQuote = new LineFormat {
        Indents = ImmutableList.Create<Indent>(Indent.Quote), Header = null
      };

      new LineInsert(new LineFormat() {
        Indents = ImmutableList.Create<Indent>(Indent.Quote)
      }).Is(new LineInsert(new LineFormat() {
        Indents = ImmutableList.Create<Indent>(Indent.Quote)
      }));

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
        .Insert("AB", new TextFormat {
          Link = "C"
        })
        .Transform(new Ops().Retain(1).Insert("D"))
        .Is(
          new Ops()
            .Insert("A", new TextFormat {
              Link = "C"
            })
            .Insert("D")
            .Insert("B", new TextFormat {
              Link = "C"
            }));
    }

    [Fact]
    public void Retain_FormatsTextWithNewLine()
    {
      var turnBold = new TextFormat {
        Bold = true, Italic = null, Link = null
      };

      new Ops()
        .Insert("AAA")
        .Insert(new LineInsert())
        .Transform(new Ops().Retain(1).Retain(1, turnBold))
        .Is(new Ops()
          .Insert("A")
          .Insert("A", TextFormat.BoldPreset)
          .Insert("A")
          .Insert(new LineInsert())
        );
    }

    [Fact]
    public void Insert_InsertsWithNewLine()
    {
      new Ops()
        .Insert("AAA")
        .Insert(new LineInsert())
        .Transform(new Ops().Retain(3).Insert("A"))
        .Is(new Ops()
          .Insert("AAAA")
          .Insert(new LineInsert())
        );

      new Ops()
        .Insert("AAA")
        .Insert(new LineInsert())
        .Transform(new Ops().Retain(1).Insert(new LineInsert()).Insert("B"))
        .Is(new Ops()
          .Insert("A")
          .Insert(new LineInsert())
          .Insert("BAA")
          .Insert(new LineInsert())
        );
    }
  }
}
