using System.Diagnostics.CodeAnalysis;

namespace Markdraw.Delta.Formats
{
  public interface IFormatModifier<T> : IFormatModifier where T : Format
  {
    public T Modify([NotNull] T format);
  }

  public interface IFormatModifier {}
}
