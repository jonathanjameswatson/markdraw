using System.Diagnostics.CodeAnalysis;
using Markdraw.Delta.Formats;

namespace Markdraw.Delta.Operations.Inserts.Inlines
{
  public abstract record InlineInsert([NotNull] InlineFormat Format) : Insert
  {

    public override InlineInsert SetFormat([NotNull] IFormatModifier formatModifier)
    {
      if (formatModifier is not IFormatModifier<InlineFormat> inlineFormatModifier) return null;
      var newFormat = inlineFormatModifier.Modify(Format);
      if (newFormat is null) return null;
      return this with {
        Format = newFormat
      };
    }
  }
}
