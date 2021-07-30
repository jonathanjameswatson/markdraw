using System;
using System.Collections.Immutable;

namespace Markdraw.Delta
{
  public record ModifyingLineFormat : Format
  {
    public Func<ImmutableList<Indent>, ImmutableList<Indent>> IndentsFunction { get; init; } = x => x;
    public Func<int, int> HeaderFunction { get; init; } = i => i;
  }
}
