using System.Collections.Generic;
using System.Collections.Immutable;
using Markdraw.Delta;
using Xunit;

namespace Markdraw.Tree.Test
{
  public class IndentationTest
  {
    [Fact]
    public void EmptyOps_IsEmptyContainer()
    {
      DeltaTree
        .Parse(new Ops())
        .Is(new Container(new List<TreeNode>()));
    }

    [Fact]
    public void OneLine_IsOneLine()
    {
      DeltaTree
        .Parse(
          new Ops()
            .Insert("A")
        )
        .Is(
          new Container(new List<TreeNode> {
            new TextLeaf(new List<TextInsert> {
              new("A")
            })
          })
        );
    }

    [Fact]
    public void TwoLines_AreTwoLines()
    {
      DeltaTree
        .Parse(
          new Ops()
            .Insert("A")
            .Insert(new LineInsert())
            .Insert("B")
        )
        .Is(
          new Container(new List<TreeNode> {
            new TextLeaf(new List<TextInsert> {
              new("A")
            }),
            new TextLeaf(new List<TextInsert> {
              new("B")
            })
          })
        );
    }

    [Fact]
    public void Quote_IsQuoted()
    {
      DeltaTree
        .Parse(
          new Ops()
            .Insert("A")
            .Insert(new LineInsert(LineFormat.QuotePreset))
        )
        .Is(
          new Container(new List<TreeNode> {
            new QuoteContainer(new List<TreeNode> {
              new TextLeaf(new List<TextInsert> {
                new("A")
              })
            })
          })
        );
    }

    [Fact]
    public void Bullets_AreBulleted()
    {
      DeltaTree
        .Parse(
          new Ops()
            .Insert("A")
            .Insert(new LineInsert(LineFormat.BulletPreset))
        )
        .Is(
          new Container(new List<TreeNode> {
            new BulletsContainer(new List<TreeNode> {
              new TextLeaf(new List<TextInsert> {
                new("A")
              })
            })
          })
        );
    }

    [Fact]
    public void Numbers_AreNumbered()
    {
      DeltaTree
        .Parse(
          new Ops()
            .Insert("A")
            .Insert(new LineInsert(LineFormat.NumberPreset))
        )
        .Is(
          new Container(new List<TreeNode> {
            new NumbersContainer(new List<TreeNode> {
              new TextLeaf(new List<TextInsert> {
                new("A")
              })
            })
          })
        );
    }

    [Fact]
    public void MultipleIndents_WorkAdjacent()
    {
      DeltaTree
        .Parse(
          new Ops()
            .Insert("A")
            .Insert(new LineInsert(LineFormat.NumberPreset))
            .Insert("B")
            .Insert(new LineInsert(LineFormat.BulletPreset))
            .Insert("C")
            .Insert(new LineInsert())
            .Insert(new LineInsert(LineFormat.QuotePreset))
        )
        .Is(
          new Container(new List<TreeNode> {
            new NumbersContainer(new List<TreeNode> {
              new TextLeaf(new List<TextInsert> {
                new("A")
              })
            }),
            new BulletsContainer(new List<TreeNode> {
              new TextLeaf(new List<TextInsert> {
                new("B")
              })
            }),
            new TextLeaf(new List<TextInsert> {
              new("C")
            }),
            new QuoteContainer(new List<TreeNode>())
          })
        );
    }

    [Fact]
    public void MultipleIndents_WorkInside()
    {
      DeltaTree
        .Parse(
          new Ops()
            .Insert("A")
            .Insert(new LineInsert(LineFormat.NumberPreset))
            .Insert("B")
            .Insert(new LineInsert(
              new LineFormat {
                Indents = ImmutableList.Create(Indent.Number(2), Indent.Bullet)
              }
            ))
            .Insert("C")
            .Insert(new LineInsert(
              new LineFormat {
                Indents = ImmutableList.Create(Indent.Number(2), Indent.Bullet, Indent.Quote)
              }
            ))
            .Insert("D")
            .Insert(new LineInsert(
              new LineFormat {
                Indents = ImmutableList.Create(Indent.Number(2), Indent.Bullet)
              }
            ))
        )
        .Is(
          new Container(new List<TreeNode> {
            new NumbersContainer(new List<TreeNode> {
              new TextLeaf(new List<TextInsert> {
                new("A")
              }),
              new BulletsContainer(new List<TreeNode> {
                new TextLeaf(new List<TextInsert> {
                  new("B")
                }),
                new QuoteContainer(new List<TreeNode> {
                  new TextLeaf(new List<TextInsert> {
                    new("C")
                  })
                }),
                new TextLeaf(new List<TextInsert> {
                  new("D")
                })
              })
            })
          })
        );
    }

    [Fact]
    public void MultipleIndents_WorkOnSameLineAtOnce()
    {
      DeltaTree
        .Parse(
          new Ops()
            .Insert("A")
            .Insert(new LineInsert(
              new LineFormat {
                Indents = ImmutableList.Create(Indent.Number(2), Indent.Bullet, Indent.Quote)
              }
            ))
        )
        .Is(
          new Container(new List<TreeNode> {
            new NumbersContainer(new List<TreeNode> {
              new BulletsContainer(new List<TreeNode> {
                new QuoteContainer(new List<TreeNode> {
                  new TextLeaf(new List<TextInsert> {
                    new("A")
                  })
                })
              })
            })
          })
        );
    }

    [Fact]
    public void MultipleIndents_WorkDroppingDownOnTheSameLineAtOnce()
    {
      DeltaTree
        .Parse(
          new Ops()
            .Insert("A")
            .Insert(new LineInsert(
              new LineFormat {
                Indents = ImmutableList.Create(Indent.Number(2), Indent.Bullet, Indent.Quote)
              }
            ))
            .Insert("B")
            .Insert(new LineInsert(LineFormat.NumberPreset))
        )
        .Is(
          new Container(new List<TreeNode> {
            new NumbersContainer(new List<TreeNode> {
              new BulletsContainer(new List<TreeNode> {
                new QuoteContainer(new List<TreeNode> {
                  new TextLeaf(new List<TextInsert> {
                    new("A")
                  })
                })
              }),
              new TextLeaf(new List<TextInsert> {
                new("B")
              })
            })
          })
        );
    }
  }
}
