namespace Markdraw.Delta.Formats
{
  public interface IFormatModifier<T> : IFormatModifier where T : Format
  {
    public T? Modify(T format);
  }

  public interface IFormatModifier {}
}
