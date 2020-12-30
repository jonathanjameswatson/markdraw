using System;
using Xunit;

namespace Markdraw.Delta.Test
{
  public class TransformTest
  {
    [Fact]
    public void RetainAndInsert_Appends()
    {
      new Ops().Insert("A").Transform(new Ops().Retain(1).Insert("B")).Is(new Ops().Insert("AB"));
      new Ops().Insert("A").Transform(new Ops().Retain(1).Insert("B", TextFormat.BoldPreset)).Is(
        new Ops().Insert("A").Insert("B", TextFormat.BoldPreset)
      );
    }

    [Fact]
    public void Retain_FormatsText()
    {
      var turnBold = new TextFormat(true, null, null);
      new Ops().Insert("A").Transform(new Ops().Retain(1, turnBold)).Is(
        new Ops().Insert("A", TextFormat.BoldPreset)
      );
      new Ops().Insert("AA").Transform(new Ops().Retain(1, turnBold)).Is(
        new Ops().Insert("A", TextFormat.BoldPreset).Insert("A")
      );
    }
  }
}
