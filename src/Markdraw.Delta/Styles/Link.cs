using System.Diagnostics.CodeAnalysis;

namespace Markdraw.Delta.Styles
{
  public record Link([NotNull] string Url = "", [NotNull] string Title = "") : Style
  {
    [return: NotNull]
    public Link Merge([NotNull] Link other)
    {
      var (url, title) = other;
      return new Link(url.Equals("") ? Url : url, title.Equals("") ? Title : title);
    }

    [return: NotNull]
    public override string Wrap(string contents)
    {
      var titleString = Title == "" ? "" : $@" ""{Title}""";
      return $"[{contents}]({Url}{titleString})";
    }

    public override string ToString()
    {
      var titleString = Title == "" ? "" : $@" ""{Title}""";
      return $"<Link ({Url}{titleString})>";
    }
  }
}
