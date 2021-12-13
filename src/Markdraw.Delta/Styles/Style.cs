using System.Diagnostics.CodeAnalysis;

namespace Markdraw.Delta.Styles
{
  public abstract record Style
  {
    public static Bold Bold => new();
    public static Italic Italic => new();

    public static Link Link([NotNull] string url = "", [NotNull] string title = "")
    {
      return new Link(url, title);
    }

    public abstract string Wrap(string contents);

    public override string ToString()
    {
      return "<Style>";
    }
  }
}
