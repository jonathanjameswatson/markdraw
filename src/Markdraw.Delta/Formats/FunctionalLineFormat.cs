using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using Markdraw.Delta.Indents;

namespace Markdraw.Delta.Formats
{
  public record FunctionalLineFormat : Format, ILineFormatModifier
  {
    public Func<ImmutableList<Indent>, ImmutableList<Indent>> IndentsFunction { get; init; } = x => x;
    public Func<int, int> HeaderFunction { get; init; } = i => i;

    public LineFormat Modify(LineFormat format)
    {
      Debug.Assert(format.Header != null, "format.Header != null");
      var indents = format.Indents;
      var newIndents = IndentsFunction(indents);
      var header = (int)format.Header;
      var newHeader = HeaderFunction(header);
      if (newHeader.Equals(header) && (newIndents == indents || newIndents.SequenceEqual(indents)))
      {
        return null;
      }
      return new LineFormat {
        Indents = newIndents, Header = newHeader
      };
    }

    public override string ToString()
    {
      return "{FunctionalLineFormat}";
    }
  }
}
