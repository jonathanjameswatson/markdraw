using System;
using System.Collections.Generic;
using Markdraw.Tree;
using Markdraw.Delta;
using Xunit;

namespace Markdraw.Tree.Test
{
  public class StructureTest
  {
    [Fact]
    public void EmptyOps_IsEmptyContainer()
    {
      DeltaTree
        .Parse(new Ops())
        .Is(new Container(new List<TreeNode>()));
    }

    [Fact]
    public void OneWord_IsOneWord()
    {
      DeltaTree
        .Parse(
          new Ops()
            .Insert("A")
        )
        .Is(
          new Container(new List<TreeNode>() {
            new TextLeaf(new List<TextInsert>() {
              new TextInsert("A")
            })
          })
        );
    }

    [Fact]
    public void TwoLines_IsTwoLines()
    {
      DeltaTree
        .Parse(
          new Ops()
            .Insert("A")
            .Insert(new LineInsert())
            .Insert("B")
        )
        .Is(
          new Container(new List<TreeNode>() {
            new TextLeaf(new List<TextInsert>() {
              new TextInsert("A")
            }),
            new TextLeaf(new List<TextInsert>() {
              new TextInsert("B")
            })
          })
        );
    }
  }
}
