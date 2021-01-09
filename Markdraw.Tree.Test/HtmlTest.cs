using System.Collections.Generic;
using Markdraw.Delta;
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
      new Container(new List<TreeNode>() {
        new TextLeaf(new List<TextInsert>() {
          new TextInsert("A")
        })
      })
        .ToString()
        .Is(@"<div><p>A</p></div>");
    }

    [Fact]
    public void TwoLines_AreTwoParagraph()
    {
      new Container(new List<TreeNode>() {
        new TextLeaf(new List<TextInsert>() {
          new TextInsert("A")
        }),
        new TextLeaf(new List<TextInsert>() {
          new TextInsert("B")
        })
      })
        .ToString()
        .Is(@"<div><p>A</p><p>B</p></div>");
    }

    [Fact]
    public void Quote_IsBlockQuote()
    {
      new Container(new List<TreeNode>() {
        new QuoteContainer(new List<TreeNode>() {
          new TextLeaf(new List<TextInsert>() {
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
      new Container(new List<TreeNode>() {
        new BulletsContainer(new List<TreeNode>() {
          new TextLeaf(new List<TextInsert>() {
            new TextInsert("A")
          })
        })
      })
        .ToString()
        .Is(@"<div><ul><li>A</li></ul></div>");
    }

    [Fact]
    public void Numbers_AreOrderedList()
    {
      new Container(new List<TreeNode>() {
        new NumbersContainer(new List<TreeNode>() {
          new TextLeaf(new List<TextInsert>() {
            new TextInsert("A")
          })
        })
      })
        .ToString()
        .Is(@"<div><ol><li>A</li></ol></div>");
    }

    [Fact]
    public void MultipleIndents_AreAdjacent()
    {
      new Container(new List<TreeNode>() {
        new NumbersContainer(new List<TreeNode>() {
          new TextLeaf(new List<TextInsert>() {
            new TextInsert("A")
          })
        }),
        new BulletsContainer(new List<TreeNode>() {
          new TextLeaf(new List<TextInsert>() {
            new TextInsert("B")
          })
        }),
        new TextLeaf(new List<TextInsert>() {
          new TextInsert("C")
        }),
        new QuoteContainer(new List<TreeNode>() {})
      })
        .ToString()
        .Is(@"<div><ol><li>A</li></ol><ul><li>B</li></ul><p>C</p><blockquote></blockquote></div>");
    }

    [Fact]
    public void MultipleIndents_AreNested()
    {
      new Container(new List<TreeNode>() {
        new NumbersContainer(new List<TreeNode>() {
          new BulletsContainer(new List<TreeNode>() {
            new QuoteContainer(new List<TreeNode>() {
              new TextLeaf(new List<TextInsert>() {
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
    public void Images_AreConvertedToHtml()
    {
      new Container(new List<TreeNode>() {
        new ImageLeaf(new ImageInsert("A", "B"))
      })
        .ToString()
        .Is(@"<div><img src=""A"" alt=""B"" /></div>");
    }

    [Fact]
    public void Dividers_AreConvertedToHtml()
    {
      new Container(new List<TreeNode>() {
        new DividerLeaf(new DividerInsert())
      })
        .ToString()
        .Is(@"<div><hr /></div>");
    }

    [Fact]
    public void CodeBlocks_AreConvertedToHtml()
    {
      new Container(new List<TreeNode>() {
        new CodeLeaf(new CodeInsert("A", "B"))
      })
        .ToString()
        .Is(@"<div><pre><code class=""language-B"">A</code></pre></div>");

      new Container(new List<TreeNode>() {
        new CodeLeaf(new CodeInsert("A", ""))
      })
        .ToString()
        .Is(@"<div><pre><code>A</code></pre></div>");
    }
  }
}