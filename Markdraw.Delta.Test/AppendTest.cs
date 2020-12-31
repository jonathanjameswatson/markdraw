using System;
using Xunit;

namespace Markdraw.Delta.Test
{
  public class AppendTest
  {
    [Fact]
    public void EmptyDelta_IsEmptyDelta()
    {
      new Ops()
        .Is(new Ops());
    }

    [Fact]
    public void Delta_IsSameDelta()
    {
      new Ops()
        .Insert("A")
        .Is(new Ops().Insert("A"));

      new Ops()
        .Insert("A")
        .IsNot(new Ops().Insert("AA"));

      new Ops()
        .Insert("A")
        .IsNot(new Ops().Insert("A", TextFormat.BoldPreset));
    }

    [Fact]
    public void Delta_HasAppendedTextMerged()
    {
      new Ops()
        .Insert("A")
        .Insert("A")
        .Is(new Ops().Insert("AA"));

      new Ops()
        .Insert("A")
        .Insert("A", TextFormat.BoldPreset)
        .Length
        .Is(2);
    }

    [Fact]
    public void Delta_DeleteFails_AtStart()
    {
      Assert.Throws<InvalidOperationException>(
        () => new Ops().Delete(1)
      );

      Assert.Throws<InvalidOperationException>(
        () => new Ops().Insert("A").Delete(2)
      );
    }

    [Fact]
    public void Delta_DeleteRemovesInserts()
    {
      new Ops()
        .Insert("A")
        .Delete(1)
        .Is(new Ops());

      new Ops()
        .Insert("AA")
        .Delete(1)
        .Is(new Ops().Insert("A"));

      new Ops()
        .Insert("AA")
        .Insert("AA", TextFormat.BoldPreset)
        .Delete(3)
        .Is(new Ops().Insert("A"));
    }

    [Fact]
    public void Delta_AppendingOps_FailsWithZeroLength()
    {
      Assert.Throws<ArgumentOutOfRangeException>(
        () => new Ops().Retain(0)
      );

      Assert.Throws<ArgumentOutOfRangeException>(
        () => new Ops().Delete(0)
      );

      Assert.Throws<ArgumentException>(
        () => new Ops().Insert("")
      );
    }
  }
}
