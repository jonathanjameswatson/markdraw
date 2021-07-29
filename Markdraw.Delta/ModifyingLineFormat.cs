using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Markdraw.Delta
{
  public record ModifyingLineFormat : Format
  {
    public Func<ImmutableList<Indent>, ImmutableList<Indent>> IndentsFunction { get; init; } = x => x;
    public Func<int, int> HeaderFunction { get; init; } = i => i;
  }
}