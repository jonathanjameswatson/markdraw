using Markdraw.Delta.Formats;

namespace Markdraw.Delta.Operations.Inserts.Inlines;

public abstract record InlineInsert(InlineFormat Format) : IInsert
{
  public virtual int Length => 1;

  public InlineInsert? SetFormat(IFormatModifier formatModifier)
  {
    if (formatModifier is not IFormatModifier<InlineFormat> inlineFormatModifier) return null;
    var newFormat = inlineFormatModifier.Modify(Format);
    if (newFormat is null) return null;
    return this with {
      Format = newFormat
    };
  }

  IInsert? IInsert.SetFormat(IFormatModifier formatModifier) => SetFormat(formatModifier);
}
