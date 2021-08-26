using Markdraw.Delta.Formats;

namespace Markdraw.Delta.Operations.Inserts.Inlines
{
  public abstract record InlineInsert : Insert
  {
    public InlineFormat Format { get; init; }

    public override InlineInsert SetFormat(Format format)
    {
      if (format is InlineFormat textFormat)
      {
        return this with {
          Format = Format.Merge(textFormat)
        };
      }
      return null;
    }
  }
}
