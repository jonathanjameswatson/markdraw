using System.Collections.Generic;
using Markdraw.Delta.Formats;
using Markdraw.Delta.Links;
using Markdraw.Delta.Operations.Inserts;
using Xunit;

namespace Markdraw.Tree.Test
{
  public class HtmlTest
  {
    [Fact]
    public void EmptyContainer_IsDiv()
    {
      new Container(new List<TreeNode>())
        .ToString()
        .Is(@"<div></div>");
    }

    [Fact]
    public void OneLine_IsParagraph()
    {
      new Container(new List<TreeNode> {
          new InlineLeaf(new List<InlineInsert> {
            new TextInsert("A")
          })
        })
        .ToString()
        .Is(@"<div><p>A</p></div>");
    }

    [Fact]
    public void TwoLines_AreTwoParagraph()
    {
      new Container(new List<TreeNode> {
          new InlineLeaf(new List<InlineInsert> {
            new TextInsert("A")
          }),
          new InlineLeaf(new List<InlineInsert> {
            new TextInsert("B")
          })
        })
        .ToString()
        .Is(@"<div><p>A</p><p>B</p></div>");
    }

    [Fact]
    public void Quote_IsBlockQuote()
    {
      new Container(new List<TreeNode> {
          new QuoteContainer(new List<TreeNode> {
            new InlineLeaf(new List<InlineInsert> {
              new TextInsert("A")
            })
          })
        })
        .ToString()
        .Is(@"<div><blockquote><p>A</p></blockquote></div>");
    }

    [Fact]
    public void Bullets_AreUnorderedList()
    {
      new Container(new List<TreeNode> {
          new BulletsContainer(new List<TreeNode> {
            new InlineLeaf(new List<InlineInsert> {
              new TextInsert("A")
            })
          })
        })
        .ToString()
        .Is(@"<div><ul><li><p>A</p></li></ul></div>");
    }

    [Fact]
    public void Numbers_AreOrderedList()
    {
      new Container(new List<TreeNode> {
          new NumbersContainer(new List<TreeNode> {
            new InlineLeaf(new List<InlineInsert> {
              new TextInsert("A")
            })
          })
        })
        .ToString()
        .Is(@"<div><ol><li><p>A</p></li></ol></div>");
    }

    [Fact]
    public void MultipleIndents_AreAdjacent()
    {
      new Container(new List<TreeNode> {
          new NumbersContainer(new List<TreeNode> {
            new InlineLeaf(new List<InlineInsert> {
              new TextInsert("A")
            })
          }),
          new BulletsContainer(new List<TreeNode> {
            new InlineLeaf(new List<InlineInsert> {
              new TextInsert("B")
            })
          }),
          new InlineLeaf(new List<InlineInsert> {
            new TextInsert("C")
          }),
          new QuoteContainer(new List<TreeNode>())
        })
        .ToString()
        .Is(@"<div><ol><li><p>A</p></li></ol><ul><li><p>B</p></li></ul><p>C</p><blockquote></blockquote></div>");
    }

    [Fact]
    public void MultipleIndents_AreNested()
    {
      new Container(new List<TreeNode> {
          new NumbersContainer(new List<TreeNode> {
            new BulletsContainer(new List<TreeNode> {
              new QuoteContainer(new List<TreeNode> {
                new InlineLeaf(new List<InlineInsert> {
                  new TextInsert("A")
                })
              })
            })
          })
        })
        .ToString()
        .Is(@"<div><ol><li><ul><li><blockquote><p>A</p></blockquote></li></ul></li></ol></div>");
    }

    [Fact]
    public void Dividers_AreConvertedToHtml()
    {
      new Container(new List<TreeNode> {
          new DividerLeaf(new DividerInsert())
        })
        .ToString()
        .Is(@"<div><hr /></div>");
    }

    [Fact]
    public void CodeBlocks_AreConvertedToHtml()
    {
      new Container(new List<TreeNode> {
          new CodeLeaf(new CodeInsert("A", "B"))
        })
        .ToString()
        .Is(@"<div><pre class=""language-B"" contenteditable=""false""><code class=""language-B"">A</code></pre></div>");

      new Container(new List<TreeNode> {
          new CodeLeaf(new CodeInsert("A"))
        })
        .ToString()
        .Is(@"<div><pre class=""language-none"" contenteditable=""false""><code class=""language-none"">A</code></pre></div>");
    }

    [Fact]
    public void Headers_AreConvertedToHtml()
    {
      new Container(new List<TreeNode> {
          new InlineLeaf(new List<InlineInsert> {
            new TextInsert("A")
          }, 1)
        })
        .ToString()
        .Is(@"<div><h1>A</h1></div>");

      new Container(new List<TreeNode> {
          new InlineLeaf(new List<InlineInsert> {
            new TextInsert("A")
          }, 2)
        })
        .ToString()
        .Is(@"<div><h2>A</h2></div>");

      new Container(new List<TreeNode> {
          new InlineLeaf(new List<InlineInsert> {
            new TextInsert("A")
          }, 3)
        })
        .ToString()
        .Is(@"<div><h3>A</h3></div>");

      new Container(new List<TreeNode> {
          new InlineLeaf(new List<InlineInsert> {
            new TextInsert("A")
          }, 4)
        })
        .ToString()
        .Is(@"<div><h4>A</h4></div>");

      new Container(new List<TreeNode> {
          new InlineLeaf(new List<InlineInsert> {
            new TextInsert("A")
          }, 5)
        })
        .ToString()
        .Is(@"<div><h5>A</h5></div>");

      new Container(new List<TreeNode> {
          new InlineLeaf(new List<InlineInsert> {
            new TextInsert("A")
          }, 6)
        })
        .ToString()
        .Is(@"<div><h6>A</h6></div>");
    }

    [Fact]
    public void DoubleLinks_AreConvertedToHtml()
    {
      new Container(new List<TreeNode> {
          new InlineLeaf(new List<InlineInsert> {
            new TextInsert("A"),
            new TextInsert("B", new TextFormat {
              Link = new ExistentLink("C")
            }),
            new TextInsert("D")
          }, 0)
        })
        .ToString()
        .Is(@"<div><p>A<a href=""C"">B</a>D</p></div>");
    }

    [Fact]
    public void ItalicsAndLink_AreConvertedToHtml()
    {
      new Container(new List<TreeNode> {
          new InlineLeaf(new List<InlineInsert> {
            new TextInsert("A"),
            new TextInsert("B", new TextFormat {
              Italic = true
            }),
            new TextInsert("C", new TextFormat {
              Italic = true, Link = new ExistentLink("F")
            }),
            new TextInsert("D", new TextFormat {
              Link = new ExistentLink("F")
            }),
            new TextInsert("E")
          }, 0)
        })
        .ToString()
        .Is(@"<div><p>A<em>B</em><a href=""F""><em>C</em>D</a>E</p></div>");
    }
  }
}
