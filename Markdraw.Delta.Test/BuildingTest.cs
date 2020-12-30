using System;
using Markdraw.Delta;
using Xunit;

namespace Markdraw.Delta.Test
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
      new Ops().Insert("A").IsNot(new Ops().Insert("AA"));
      new Ops().Insert("A").IsNot(
        new Ops().Insert("A", new TextFormat(true, false, null, false, false)));
    }

    [Fact]
    public void Delta_HasAppendedTextMerged()
    {
      new Ops().Insert("A").Insert("A").Is(new Ops().Insert("AA"));
      new Ops().Insert("A").Insert("A", new TextFormat(true, false, null, false, false)).Length.Is(2);
    }

    [Fact]
    public void Delta_DeleteFails_AtStart()
    {
      Assert.Throws<InvalidOperationException>(() => new Ops().Delete(1));
    }
  }
}
