using System.Collections.Immutable;
using Markdraw.Delta.Formats;
using Markdraw.Delta.Operations.Inserts;
using Markdraw.Delta.Operations.Inserts.Inlines;
using Markdraw.Delta.Styles;
using Markdraw.Tree.TreeNodes;
using Markdraw.Tree.TreeNodes.Containers.BlockContainers;
using Markdraw.Tree.TreeNodes.Containers.InlineContainers;
using Markdraw.Tree.TreeNodes.Leaves;
using Xunit;

namespace Markdraw.Tree.Test;

public class HtmlTest
{
  [Fact]
  public void EmptyContainer_IsDiv()
  {
    new BlockContainer(new List<TreeNode>()).ToString().Is(@"<div></div>");
  }

  [Fact]
  public void OneLine_IsParagraph()
  {
    new BlockContainer(new List<TreeNode> {
      new OuterInlineContainer(new List<TreeNode> {
        new InlineLeaf(new TextInsert("A"))
      })
    }).ToString().Is(@"<div><p>A</p></div>");
  }

  [Fact]
  public void TwoLines_AreTwoParagraph()
  {
    new BlockContainer(new List<TreeNode> {
      new OuterInlineContainer(new List<TreeNode> {
        new InlineLeaf(new TextInsert("A"))
      }),
      new OuterInlineContainer(new List<TreeNode> {
        new InlineLeaf(new TextInsert("B"))
      })
    }).ToString().Is(@"<div><p>A</p><p>B</p></div>");
  }

  [Fact]
  public void Quote_IsBlockQuote()
  {
    new BlockContainer(new List<TreeNode> {
      new QuoteContainer(new List<TreeNode> {
        new OuterInlineContainer(new List<TreeNode> {
          new InlineLeaf(new TextInsert("A"))
        })
      })
    }).ToString().Is(@"<div><blockquote><p>A</p></blockquote></div>");
  }

  [Fact]
  public void Bullets_AreUnorderedList()
  {
    new BlockContainer(new List<TreeNode> {
      new BulletsContainer(new List<TreeNode> {
        new ListItemContainer(new List<TreeNode> {
          new OuterInlineContainer(new List<TreeNode> {
            new InlineLeaf(new TextInsert("A"))
          })
        })
      })
    }).ToString().Is(@"<div><ul><li><p>A</p></li></ul></div>");
  }

  [Fact]
  public void Numbers_AreOrderedList()
  {
    new BlockContainer(new List<TreeNode> {
      new NumbersContainer(new List<TreeNode> {
        new ListItemContainer(new List<TreeNode> {
          new OuterInlineContainer(new List<TreeNode> {
            new InlineLeaf(new TextInsert("A"))
          })
        })
      })
    }).ToString().Is(@"<div><ol><li><p>A</p></li></ol></div>");
  }

  [Fact]
  public void MultipleIndents_AreAdjacent()
  {
    new BlockContainer(new List<TreeNode> {
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
        new QuoteContainer(new List<TreeNode>())
      }).ToString()
      .Is(@"<div><ol><li><p>A</p></li></ol><ul><li><p>B</p></li></ul><p>C</p><blockquote></blockquote></div>");
  }

  [Fact]
  public void MultipleIndents_AreNested()
  {
    new BlockContainer(new List<TreeNode> {
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
    }).ToString().Is(@"<div><ol><li><ul><li><blockquote><p>A</p></blockquote></li></ul></li></ol></div>");
  }

  [Fact]
  public void Dividers_AreConvertedToHtml()
  {
    new BlockContainer(new List<TreeNode> {
      new DividerLeaf(new DividerInsert())
    }).ToString().Is(@"<div><hr /></div>");
  }

  [Fact]
  public void CodeBlocks_AreConvertedToHtml()
  {
    new BlockContainer(new List<TreeNode> {
      new CodeLeaf(new CodeInsert("A", "B"))
    }).ToString().Is(@"<div><pre><code class=""language-B"">A&#10;</code></pre></div>");

    new BlockContainer(new List<TreeNode> {
      new CodeLeaf(new CodeInsert("A"))
    }).ToString().Is(@"<div><pre><code>A&#10;</code></pre></div>");
  }

  [Fact]
  public void Headers_AreConvertedToHtml()
  {
    new BlockContainer(new List<TreeNode> {
      new OuterInlineContainer(new List<TreeNode> {
        new InlineLeaf(new TextInsert("A"))
      }, null, 0, 1)
    }).ToString().Is(@"<div><h1>A</h1></div>");

    new BlockContainer(new List<TreeNode> {
      new OuterInlineContainer(new List<TreeNode> {
        new InlineLeaf(new TextInsert("A"))
      }, null, 0, 2)
    }).ToString().Is(@"<div><h2>A</h2></div>");

    new BlockContainer(new List<TreeNode> {
      new OuterInlineContainer(new List<TreeNode> {
        new InlineLeaf(new TextInsert("A"))
      }, null, 0, 3)
    }).ToString().Is(@"<div><h3>A</h3></div>");

    new BlockContainer(new List<TreeNode> {
      new OuterInlineContainer(new List<TreeNode> {
        new InlineLeaf(new TextInsert("A"))
      }, null, 0, 4)
    }).ToString().Is(@"<div><h4>A</h4></div>");

    new BlockContainer(new List<TreeNode> {
      new OuterInlineContainer(new List<TreeNode> {
        new InlineLeaf(new TextInsert("A"))
      }, null, 0, 5)
    }).ToString().Is(@"<div><h5>A</h5></div>");

    new BlockContainer(new List<TreeNode> {
      new OuterInlineContainer(new List<TreeNode> {
        new InlineLeaf(new TextInsert("A"))
      }, null, 0, 6)
    }).ToString().Is(@"<div><h6>A</h6></div>");
  }

  [Fact]
  public void DoubleLinks_AreConvertedToHtml()
  {
    new BlockContainer(new List<TreeNode> {
      new OuterInlineContainer(new List<TreeNode> {
        new InlineLeaf(new TextInsert("A")),
        new LinkInlineContainer(new List<TreeNode> {
          new InlineLeaf(new TextInsert("B", new InlineFormat(ImmutableList.Create<Style>(Style.Link("C")))))
        }, null, 0, "C"),
        new InlineLeaf(new TextInsert("D"))
      })
    }).ToString().Is(@"<div><p>A<a href=""C"">B</a>D</p></div>");
  }
}
