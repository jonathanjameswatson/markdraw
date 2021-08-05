using System;
using Markdraw.Delta.Formats;
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
