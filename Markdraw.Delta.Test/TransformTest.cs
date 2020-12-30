using System;
using Xunit;

namespace Markdraw.Delta.Test
{
  public class TransformTest
  {
    [Fact]
    public void ADelta_IsAppendedTo()
    {
      new Ops().Insert("A").Transform(new Ops().Retain(1).Insert("B")).Is(new Ops().Insert("AB"));
      new Ops().Insert("A").Transform(new Ops().Retain(1).Insert("B", TextFormat.BoldPreset)).Is(
        new Ops().Insert("A").Insert("B", TextFormat.BoldPreset)
      );
    }
  }
}
