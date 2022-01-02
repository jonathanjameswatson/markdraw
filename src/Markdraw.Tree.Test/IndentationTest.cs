using System.Collections.Immutable;
using Markdraw.Delta.Formats;
using Markdraw.Delta.Indents;
using Markdraw.Delta.Operations.Inserts;
using Markdraw.Delta.Operations.Inserts.Inlines;
using Markdraw.Delta.OperationSequences;
using Markdraw.Tree.TreeNodes;
using Markdraw.Tree.TreeNodes.Containers.BlockContainers;
using Markdraw.Tree.TreeNodes.Containers.InlineContainers;
using Markdraw.Tree.TreeNodes.Leaves;
using Xunit;

namespace Markdraw.Tree.Test;

public class IndentationTest
{
  [Fact]
  public void EmptyOps_IsEmptyContainer()
  {
    DeltaTree.Parse(new Document()).Is(new BlockContainer(new List<TreeNode>()));
  }

  [Fact]
  public void OneLine_IsOneLine()
  {
    DeltaTree.Parse(new Document().Insert("A")).Is(new BlockContainer(new List<TreeNode> {
      new OuterInlineContainer(new List<TreeNode> {
        new InlineLeaf(new TextInsert("A"))
      })
    }));
  }

  [Fact]
  public void TwoLines_AreTwoLines()
  {
    DeltaTree.Parse(new Document().Insert("A").Insert(new LineInsert()).Insert("B")).Is(new BlockContainer(
      new List<TreeNode> {
        new OuterInlineContainer(new List<TreeNode> {
          new InlineLeaf(new TextInsert("A"))
        }),
        new OuterInlineContainer(new List<TreeNode> {
          new InlineLeaf(new TextInsert("B"))
        })
      }));
  }

  [Fact]
  public void Quote_IsQuoted()
  {
    DeltaTree.Parse(new Document().Insert("A").Insert(new LineInsert(LineFormat.QuotePreset))).Is(new BlockContainer(
      new List<TreeNode> {
        new QuoteContainer(new List<TreeNode> {
          new OuterInlineContainer(new List<TreeNode> {
            new InlineLeaf(new TextInsert("A"))
          })
        })
      }));
  }

  [Fact]
  public void Bullets_AreBulleted()
  {
    DeltaTree.Parse(new Document().Insert("A").Insert(new LineInsert(LineFormat.BulletPreset))).Is(new BlockContainer(
      new List<TreeNode> {
        new BulletsContainer(new List<TreeNode> {
          new ListItemContainer(new List<TreeNode> {
            new OuterInlineContainer(new List<TreeNode> {
              new InlineLeaf(new TextInsert("A"))
            })
          })
        })
      }));
  }

  [Fact]
  public void Numbers_AreNumbered()
  {
    DeltaTree.Parse(new Document().Insert("A").Insert(new LineInsert(LineFormat.NumberPreset))).Is(new BlockContainer(
      new List<TreeNode> {
        new NumbersContainer(new List<TreeNode> {
          new ListItemContainer(new List<TreeNode> {
            new OuterInlineContainer(new List<TreeNode> {
              new InlineLeaf(new TextInsert("A"))
            })
          })
        })
      }));
  }

  [Fact]
  public void MultipleIndents_WorkAdjacent()
  {
    DeltaTree.Parse(new Document().Insert("A").Insert(new LineInsert(LineFormat.NumberPreset)).Insert("B")
      .Insert(new LineInsert(LineFormat.BulletPreset)).Insert("C").Insert(new LineInsert())
      .Insert(new LineInsert(LineFormat.QuotePreset))).Is(new BlockContainer(new List<TreeNode> {
      new NumbersContainer(new List<TreeNode> {
        new ListItemContainer(new List<TreeNode> {
          new OuterInlineContainer(new List<TreeNode> {
            new InlineLeaf(new TextInsert("A"))
          })
        })
      }),
      new BulletsContainer(new List<TreeNode> {
        new ListItemContainer(new List<TreeNode> {
          new OuterInlineContainer(new List<TreeNode> {
            new InlineLeaf(new TextInsert("B"))
          })
        })
      }),
      new OuterInlineContainer(new List<TreeNode> {
        new InlineLeaf(new TextInsert("C"))
      }),
      new QuoteContainer(new List<TreeNode> {
        new OuterInlineContainer(new List<TreeNode>())
      })
    }));
  }

  [Fact]
  public void MultipleIndents_DoNotNest()
  {
    DeltaTree.Parse(new Document().Insert("A").Insert(new LineInsert(LineFormat.NumberPreset)).Insert("B").Insert(
      new LineInsert(new LineFormat {
        Indents = ImmutableList.Create<Indent>(Indent.Number(-1), Indent.LooseBullet)
      })).Insert("C").Insert(new LineInsert(new LineFormat {
      Indents = ImmutableList.Create<Indent>(Indent.Number(-1), Indent.Bullet(false), Indent.Quote)
    })).Insert("D").Insert(new LineInsert(new LineFormat {
      Indents = ImmutableList.Create<Indent>(Indent.Number(-1), Indent.Bullet(false))
    }))).Is(new BlockContainer(new List<TreeNode> {
      new NumbersContainer(new List<TreeNode> {
        new ListItemContainer(new List<TreeNode> {
          new OuterInlineContainer(new List<TreeNode> {
            new InlineLeaf(new TextInsert("A"))
          })
        }),
        new ListItemContainer(new List<TreeNode> {
          new BulletsContainer(new List<TreeNode> {
            new ListItemContainer(new List<TreeNode> {
              new OuterInlineContainer(new List<TreeNode> {
                new InlineLeaf(new TextInsert("B"))
              })
            })
          })
        }),
        new ListItemContainer(new List<TreeNode> {
          new BulletsContainer(new List<TreeNode> {
            new ListItemContainer(new List<TreeNode> {
              new QuoteContainer(new List<TreeNode> {
                new OuterInlineContainer(new List<TreeNode> {
                  new InlineLeaf(new TextInsert("C"))
                })
              })
            })
          })
        }),
        new ListItemContainer(new List<TreeNode> {
          new BulletsContainer(new List<TreeNode> {
            new ListItemContainer(new List<TreeNode> {
              new OuterInlineContainer(new List<TreeNode> {
                new InlineLeaf(new TextInsert("D"))
              })
            })
          })
        })
      })
    }));
  }

  [Fact]
  public void MultipleIndents_WorkOnSameLineAtOnce()
  {
    DeltaTree.Parse(new Document().Insert("A").Insert(new LineInsert(new LineFormat {
      Indents = ImmutableList.Create<Indent>(Indent.Number(2), Indent.LooseBullet, Indent.Quote)
    }))).Is(new BlockContainer(new List<TreeNode> {
      new NumbersContainer(new List<TreeNode> {
        new ListItemContainer(new List<TreeNode> {
          new BulletsContainer(new List<TreeNode> {
            new ListItemContainer(new List<TreeNode> {
              new QuoteContainer(new List<TreeNode> {
                new OuterInlineContainer(new List<TreeNode> {
                  new InlineLeaf(new TextInsert("A"))
                })
              })
            })
          })
        })
      })
    }));
  }

  [Fact]
  public void MultipleIndents_WorkDroppingDownOnTheSameLineAtOnce()
  {
    DeltaTree.Parse(new Document().Insert("A")
      .Insert(new LineInsert(
        new LineFormat(ImmutableList.Create<Indent>(Indent.Number(), Indent.LooseBullet, Indent.Quote)))).Insert("B")
      .Insert(new LineInsert(new LineFormat(ImmutableList.Create<Indent>(Indent.Number(-1)))))).Is(new BlockContainer(
      new List<TreeNode> {
        new NumbersContainer(new List<TreeNode> {
          new ListItemContainer(new List<TreeNode> {
            new BulletsContainer(new List<TreeNode> {
              new ListItemContainer(new List<TreeNode> {
                new QuoteContainer(new List<TreeNode> {
                  new OuterInlineContainer(new List<TreeNode> {
                    new InlineLeaf(new TextInsert("A"))
                  })
                })
              })
            })
          }),
          new ListItemContainer(new List<TreeNode> {
            new OuterInlineContainer(new List<TreeNode> {
              new InlineLeaf(new TextInsert("B"))
            })
          })
        })
      }));
  }
}
