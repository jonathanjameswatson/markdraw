using System.Diagnostics.CodeAnalysis;
using Markdraw.Delta.Formats;

namespace Markdraw.Delta.Operations.Inserts.Inlines
{
  public record InlineHtmlInsert(string Content, [NotNull] InlineFormat Format) : InlineInsert(Format), IHtmlInsert
  {
    public InlineHtmlInsert(string content = "") : this(content, new InlineFormat()) {}

    public override string ToString()
    {
      return Format.Wrap(Content);
    }
  }
}
