using System;

namespace Markdraw.Delta
{
  public record Link(string Url = "", string Title = "")
  {
    public Link Merge(Link other)
    {
      if (other is null)
      {
        return this;
      }
      if (Url is null || Title is null)
      {
        throw new InvalidOperationException("Neither Url nor Title may be null.");
      }
      var (url, title) = other;
      var newUrl = url ?? Url;
      return new Link(newUrl, newUrl == "" ? "" : title ?? Title);
    }
  };
}
