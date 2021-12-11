using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Markdraw.Delta.Indents;

namespace Markdraw.Delta.Formats
{
  [return: NotNull]
  public delegate ImmutableList<Indent> IndentsModifier([NotNull] ImmutableList<Indent> indents);

  public delegate int HeaderModifier(int header);

  public record LineFormatModifier(IndentsModifier ModifyIndents = null, HeaderModifier ModifyHeader = null) : IFormatModifier<LineFormat>
  {
    public LineFormat Modify([NotNull] LineFormat format)
    {
      var (indents, header) = format;
      var newIndents = ModifyIndents?.Invoke(indents) ?? indents;
      var newHeader = ModifyHeader?.Invoke(header) ?? header;

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
      return "{LineFormatModifier}";
    }
  }
}
