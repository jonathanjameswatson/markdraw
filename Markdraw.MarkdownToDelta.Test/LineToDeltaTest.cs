using System.Collections.Generic;
using System.Collections.Immutable;
using Markdraw.Delta;
using Xunit;

namespace Markdraw.MarkdownToDelta.Test
{
  public class LineToDeltaTest
  {
    [Fact]
    public void EmptyString_IsJustLine()
    {
      LineToDeltaConverter
        .Parse("")
        .Is(
          new Ops().Insert(new LineInsert())
        );
    }

    [Fact]
    public void A_IsTheLetterA()
    {
      LineToDeltaConverter
        .Parse("A")
        .Is(
          new Ops()
            .Insert("A").Insert(new LineInsert())
        );
    }

    [Fact]
    public void BoldA_IsTheLetterABold()
    {
      LineToDeltaConverter
        .Parse("**A**")
        .Is(
          new Ops()
            .Insert("A", TextFormat.BoldPreset)
            .Insert(new LineInsert())
        );

      LineToDeltaConverter
        .Parse("__A__")
        .Is(
          new Ops()
            .Insert("A", TextFormat.BoldPreset)
            .Insert(new LineInsert())
        );
    }

    [Fact]
    public void ItalicA_IsTheLetterAItalic()
    {
      LineToDeltaConverter
        .Parse("*A*")
        .Is(
          new Ops()
            .Insert("A", TextFormat.ItalicPreset)
            .Insert(new LineInsert())
        );

      LineToDeltaConverter
        .Parse("_A_")
        .Is(
          new Ops()
            .Insert("A", TextFormat.ItalicPreset)
            .Insert(new LineInsert())
        );
    }

    [Fact]
    public void BoldItalicA_IsTheLetterABoldItalic()
    {
      LineToDeltaConverter
        .Parse("***A***")
        .Is(
          new Ops()
            .Insert("A", new TextFormat { Bold = true, Italic = true })
            .Insert(new LineInsert())
        );

      LineToDeltaConverter
        .Parse("___A___")
        .Is(
          new Ops()
            .Insert("A", new TextFormat { Bold = true, Italic = true })
            .Insert(new LineInsert())
        );
    }

    [Fact]
    public void BoldAThenItalicB_IsCorrect()
    {
      LineToDeltaConverter
        .Parse("**A** *B*")
        .Is(
          new Ops()
            .Insert("A", TextFormat.BoldPreset)
            .Insert(" ")
            .Insert("B", TextFormat.ItalicPreset)
            .Insert(new LineInsert())
        );

      LineToDeltaConverter
        .Parse("__A__ _B_")
        .Is(
          new Ops()
            .Insert("A", TextFormat.BoldPreset)
            .Insert(" ")
            .Insert("B", TextFormat.ItalicPreset)
            .Insert(new LineInsert())
        );
    }

    [Fact]
    public void IntrawordEmphasis_WorksWithAsterisks()
    {
      LineToDeltaConverter
        .Parse("A*B*C")
        .Is(
          new Ops()
            .Insert("A")
            .Insert("B", TextFormat.ItalicPreset)
            .Insert("C")
            .Insert(new LineInsert())
        );
    }

    [Fact]
    public void IntrawordEmphasis_DoesntWorksWithUnderscores()
    {
      LineToDeltaConverter
        .Parse("A_B_C")
        .Is(
          new Ops()
            .Insert("A_B_C")
            .Insert(new LineInsert())
        );
    }

    [Fact]
    public void EmphasisWithExtraAsterisk_HasAsteriskOutside()
    {
      LineToDeltaConverter
        .Parse("**A*")
        .Is(
          new Ops()
            .Insert("*")
            .Insert("A", TextFormat.ItalicPreset)
            .Insert(new LineInsert())
        );

      LineToDeltaConverter
        .Parse("*A**")
        .Is(
          new Ops()
            .Insert("A", TextFormat.ItalicPreset)
            .Insert("*")
            .Insert(new LineInsert())
        );
    }

    [Fact]
    public void ItalicInsideItalic_IsItalic()
    {
      LineToDeltaConverter
        .Parse("*(*A*)*")
        .Is(
          new Ops()
            .Insert("(A)", TextFormat.ItalicPreset)
            .Insert(new LineInsert())
        );
    }

    [Fact]
    public void BoldInsideBold_IsBold()
    {
      LineToDeltaConverter
        .Parse("**(**A**)**")
        .Is(
          new Ops()
            .Insert("(A)", TextFormat.BoldPreset)
            .Insert(new LineInsert())
        );

      LineToDeltaConverter
        .Parse("******A******")
        .Is(
          new Ops()
            .Insert("A", TextFormat.BoldPreset)
            .Insert(new LineInsert())
        );
    }

    [Fact]
    public void ItalicInsideBold_IsItalicInsideBold()
    {
      LineToDeltaConverter
        .Parse("**A *B***")
        .Is(
          new Ops()
            .Insert("A ", TextFormat.BoldPreset)
            .Insert("B", new TextFormat { Bold = true, Italic = true })
            .Insert(new LineInsert())
        );
    }

    [Fact]
    public void BoldInsideItalic_IsBoldInsideItalic()
    {
      LineToDeltaConverter
        .Parse("*A **B***")
        .Is(
          new Ops()
            .Insert("A ", TextFormat.ItalicPreset)
            .Insert("B", new TextFormat { Bold = true, Italic = true })
            .Insert(new LineInsert())
        );
    }

    [Fact]
    public void AsterisksAndUnderscores_DoNotMakeEmphasis()
    {
      LineToDeltaConverter
        .Parse("*A_")
        .Is(
          new Ops()
            .Insert("*A_")
            .Insert(new LineInsert())
        );
    }

    [Fact]
    public void AsteriskAndSpace_GivesBullet()
    {
      LineToDeltaConverter
        .Parse("* A")
        .Is(
          new Ops()
            .Insert("A")
            .Insert(new LineInsert(LineFormat.BulletPreset))
        );
    }

    [Fact]
    public void DashAndSpace_GivesBullet()
    {
      LineToDeltaConverter
        .Parse("- A")
        .Is(
          new Ops()
            .Insert("A")
            .Insert(new LineInsert(LineFormat.BulletPreset))
        );
    }

    [Fact]
    public void NumberAndSpace_GivesBullet()
    {
      LineToDeltaConverter
        .Parse("1. A")
        .Is(
          new Ops()
            .Insert("A")
            .Insert(new LineInsert(LineFormat.NumberPreset))
        );
    }

    [Fact]
    public void GreaterThan_GivesQuote()
    {
      LineToDeltaConverter
        .Parse("> A")
        .Is(
          new Ops()
            .Insert("A")
            .Insert(new LineInsert(LineFormat.QuotePreset))
        );

      LineToDeltaConverter
        .Parse(">A")
        .Is(
          new Ops()
            .Insert("A")
            .Insert(new LineInsert(
              new LineFormat { Indents = ImmutableList.Create(Indent.Quote) }
            ))
        );
    }

    [Fact]
    public void FourSpaces_GivesCode()
    {
      LineToDeltaConverter
        .Parse("    A")
        .Is(
          new Ops()
            .Insert("A")
            .Insert(new LineInsert(LineFormat.CodePreset))
        );
    }

    [Fact]
    public void MultipleIndents_AreMixed()
    {
      LineToDeltaConverter
        .Parse("> 1. * A")
        .Is(
          new Ops()
            .Insert("A")
            .Insert(
              new LineInsert(
                new LineFormat {
                  Indents =  ImmutableList.Create(
                    Indent.Quote,
                    Indent.Empty(1),
                    Indent.Number(2),
                    Indent.Empty(1),
                    Indent.Bullet,
                    Indent.Empty(1)
                  )
                }
              ))
          );

    LineToDeltaConverter
        .Parse("-  >  1.  A")
        .Is(
          new Ops()
            .Insert("A")
            .Insert(new LineInsert (
              new LineFormat {
                Indents = ImmutableList.Create(
                  Indent.Bullet,
                  Indent.Empty(2),
                  Indent.Quote,
                  Indent.Empty(2),
                  Indent.Number(2),
                  Indent.Empty(2)
                )
              }
            ))
          );

      LineToDeltaConverter
        .Parse(">>> A")
        .Is(
          new Ops()
            .Insert("A")
            .Insert(new LineInsert(
              new LineFormat {
                Indents = ImmutableList.Create(
                  Indent.Quote,
                  Indent.Quote,
                  Indent.Quote,
                  Indent.Empty(1)
                )
              }
            ))
        );
    }

    [Fact]
    public void MultipleIndentsWithCode_StopsWithCode()
    {
      LineToDeltaConverter
        .Parse("> 1.    * A")
        .Is(
          new Ops()
            .Insert("* A")
            .Insert(new LineInsert(
              new LineFormat {
                Indents = ImmutableList.Create(Indent.Quote, Indent.Empty(1), Indent.Number(2), Indent.Code)
              }
            ))
        );
    }

    [Fact]
    public void Headers_AreCounted()
    {
      LineToDeltaConverter
        .Parse("# A")
        .Is(
          new Ops()
            .Insert("A")
            .Insert(new LineInsert(
              new LineFormat { Header = 1 })
            )
        );

      LineToDeltaConverter
        .Parse("###### A")
        .Is(
          new Ops()
            .Insert("A")
            .Insert(new LineInsert(
              new LineFormat { Header = 6 })
            )
        );

      LineToDeltaConverter
        .Parse("####### A")
        .Is(
          new Ops()
            .Insert("####### A")
            .Insert(new LineInsert())
        );
    }

    [Fact]
    public void Headers_ComeAfterInsertsExceptCode()
    {
      LineToDeltaConverter
        .Parse("> # A")
        .Is(
          new Ops()
            .Insert("A")
            .Insert(new LineInsert(
              new LineFormat {
                Indents = ImmutableList.Create(Indent.Quote, Indent.Empty(1)),
                Header = 1
              }
            ))
        );
      
      LineToDeltaConverter
        .Parse("* # A")
        .Is(
          new Ops()
            .Insert("A")
            .Insert(new LineInsert(
              new LineFormat {
                Indents = ImmutableList.Create(Indent.Bullet, Indent.Empty(1)),
                Header = 1
              }
            ))
        );

      LineToDeltaConverter
        .Parse("1. # A")
        .Is(
          new Ops()
            .Insert("A")
            .Insert(new LineInsert(
              new LineFormat {
                Indents = ImmutableList.Create(Indent.Number(2), Indent.Empty(1)),
                Header = 1
              }
            ))
        );

      LineToDeltaConverter
        .Parse("    # A")
        .Is(
          new Ops()
            .Insert("# A")
            .Insert(new LineInsert(LineFormat.CodePreset))
        );
    }

    [Fact]
    public void Headers_DoNotComeBeforeIndents()
    {
      LineToDeltaConverter
        .Parse("# > A")
        .Is(
          new Ops()
            .Insert("> A")
            .Insert(new LineInsert(
              new LineFormat { Header = 1 })
            )
        );
    }

    [Fact]
    public void Links_AreFound()
    {
      LineToDeltaConverter
        .Parse("A[B](C)D")
        .Is(
          new Ops()
            .Insert("A")
            .Insert("B", new TextFormat { Link = "C" })
            .Insert("D")
            .Insert(new LineInsert())
        );

      LineToDeltaConverter
        .Parse("A[B](C)D[E](F)G")
        .Is(
          new Ops()
            .Insert("A")
            .Insert("B", new TextFormat { Link = "C" })
            .Insert("D")
            .Insert("E", new TextFormat { Link = "F" })
            .Insert("G")
            .Insert(new LineInsert())
        );
    }

    [Fact]
    public void Images_AreFound()
    {
      LineToDeltaConverter
        .Parse("A![B](C)D")
        .Is(
          new Ops()
            .Insert("A")
            .Insert(new ImageInsert("C", "B"))
            .Insert("D")
            .Insert(new LineInsert())
        );
    }

    [Fact]
    public void ImagesAndLinks_AreFound()
    {
      LineToDeltaConverter
        .Parse("A![B](C)D[E](F)")
        .Is(
          new Ops()
            .Insert("A")
            .Insert(new ImageInsert("C", "C"))
            .Insert("D")
            .Insert("E", new TextFormat { Link = "F" })
            .Insert(new LineInsert())
        );
    }

    [Fact]
    public void Links_CanHaveEmphasis()
    {
      LineToDeltaConverter
        .Parse("[***A***](B)")
        .Is(
          new Ops()
            .Insert("A", new TextFormat { Bold = true, Italic = true, Link = "B" })
            .Insert(new LineInsert())
        );
    }

    [Fact(Skip = "Not implemented")]
    public void OnlyAsciiPunctuation_IsCancelled()
    {
      LineToDeltaConverter
        .Parse(@"\!\""\#\$\%\&\'\(\)\*\+\,\-\.\/\:\;\<\=\>\?\@\[\\\]\^\_\`\{\|\}\~")
        .Is(
          new Ops()
            .Insert(@"!""#$%&'()*+,-./:;<=>?@[\]^_`{|}~")
            .Insert(new LineInsert())
        );

      LineToDeltaConverter
        .Parse(@"\→\A\a\ \3\φ\«")
        .Is(
          new Ops()
            .Insert(@"\→\A\a\ \3\φ\«")
            .Insert(new LineInsert())
        );
    }
  }
}
