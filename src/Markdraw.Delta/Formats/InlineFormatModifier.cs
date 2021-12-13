using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Markdraw.Delta.Styles;

namespace Markdraw.Delta.Formats
{
  [return: NotNull]
  public delegate ImmutableList<Style> StyleModifier([NotNull] ImmutableList<Style> styles);

  public delegate bool CodeModifier(bool code);

  public record InlineFormatModifier
    (StyleModifier ModifyStyles = null, CodeModifier ModifyCode = null) : IFormatModifier<InlineFormat>
  {
    public InlineFormat Modify([NotNull] InlineFormat format)
    {
      var (styles, code) = format;
      var newStyles = ModifyStyles?.Invoke(styles) ?? styles;
      var newCode = ModifyCode?.Invoke(code) ?? code;

      if (newCode.Equals(code) && (newStyles == styles || newStyles.SequenceEqual(styles)))
      {
        return null;
      }

      return new InlineFormat(newStyles, newCode);
    }

    public override string ToString()
    {
      return "{LineFormatModifier}";
    }
  }
}
