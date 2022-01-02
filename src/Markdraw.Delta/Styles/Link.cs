using System.Text;

namespace Markdraw.Delta.Styles;

public record Link(string Url = "", string Title = "") : Style
{
  public Link Merge(Link other)
  {
    var (url, title) = other;
    return new Link(url.Equals("", StringComparison.Ordinal) ? Url : url,
      title.Equals("", StringComparison.Ordinal) ? Title : title);
  }

  public override string Wrap(string contents)
  {
    var stringBuilder = new StringBuilder();
    stringBuilder.Append('[');
    stringBuilder.Append(contents);
    stringBuilder.Append("](");
    stringBuilder.Append(Url);
    if (!Title.Equals("", StringComparison.Ordinal))
    {
      stringBuilder.Append(@" """);
      stringBuilder.Append(Title);
      stringBuilder.Append('"');
    }
    stringBuilder.Append(')');
    return stringBuilder.ToString();
  }

  public override string ToString()
  {
    var stringBuilder = new StringBuilder();
    stringBuilder.Append('<');
    stringBuilder.Append(Url);
    if (!Title.Equals("", StringComparison.Ordinal))
    {
      stringBuilder.Append($@" ""{Title}""");
    }
    stringBuilder.Append('>');
    return stringBuilder.ToString();
  }
}
