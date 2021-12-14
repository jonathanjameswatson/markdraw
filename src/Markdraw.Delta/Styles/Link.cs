namespace Markdraw.Delta.Styles;

public record Link(string Url = "", string Title = "") : Style
{
  public Link Merge(Link other)
  {
    var (url, title) = other;
    return new Link(url.Equals("") ? Url : url, title.Equals("") ? Title : title);
  }

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