using System;
using Markdraw.Delta;
using Xunit;

namespace Markdraw.Test
{
  public class BuildingTest
  {
    [Fact]
    public void EmptyDelta_IsEmptyDelta()
    {
      new Ops().Is(new Ops());
    }

    [Fact]
    public void Delta_IsSameDelta()
    {
      new Ops().Insert("A").Is(new Ops().Insert("A"));
      new Ops().Insert("A").IsNot(new Ops().Insert("Aa"));
      new Ops().Insert("A").IsNot(
        new Ops().Insert("A", new TextFormat(true, false, null, false, false)));
    }
  }
}
