using Markdraw.Delta.Formats;
using Markdraw.Delta.OperationSequences;
using Xunit;

namespace Markdraw.Delta.Test;

public class AppendTest
{
  [Fact]
  public void EmptyDelta_IsEmptyDelta()
  {
    new Document().Is(new Document());
  }

  [Fact]
  public void Delta_IsSameDelta()
  {
    new Document().Insert("A").Is(new Document().Insert("A"));

    new Document().Insert("A").IsNot(new Document().Insert("AA"));

    new Document().Insert("A").IsNot(new Document().Insert("A", InlineFormat.BoldPreset));
  }

  [Fact]
  public void Delta_HasAppendedTextMerged()
  {
    new Document().Insert("A").Insert("A").Is(new Document().Insert("AA"));

    new Document().Insert("A").Insert("A", InlineFormat.BoldPreset).Length.Is(2);
  }

  [Fact]
  public void Delta_AppendingOps_FailsWithZeroLength()
  {
    Assert.Throws<ArgumentOutOfRangeException>(() => new Transformation().Retain(0));

    Assert.Throws<ArgumentOutOfRangeException>(() => new Transformation().Delete(0));

    Assert.Throws<ArgumentOutOfRangeException>(() => new Transformation().Insert(""));
  }
}