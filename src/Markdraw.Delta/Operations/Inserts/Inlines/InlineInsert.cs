using Markdraw.Delta.Formats;

namespace Markdraw.Delta.Operations.Inserts.Inlines
{
  public abstract record InlineInsert(InlineFormat Format) : Insert
  {
    public override InlineInsert? SetFormat(IFormatModifier formatModifier)
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
