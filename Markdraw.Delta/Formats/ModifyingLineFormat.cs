using System;
using System.Collections.Immutable;
using Markdraw.Delta.Indents;

namespace Markdraw.Delta.Formats
{
  public record ModifyingLineFormat : Format
  {
    public Func<ImmutableList<Indent>, ImmutableList<Indent>> IndentsFunction { get; init; } = x => x;
    public Func<int, int> HeaderFunction { get; init; } = i => i;
  }
}
