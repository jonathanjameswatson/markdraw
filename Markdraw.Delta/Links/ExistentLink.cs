using System;

namespace Markdraw.Delta.Links
{
  public record ExistentLink(string Url = "", string Title = "") : Link
  {
    public override Link Merge(Link other)
    {
      if (other is null)
      {
        return this;
      }
      if (Url is null || Title is null)
      {
        throw new InvalidOperationException("Neither Url nor Title may be null.");
      }
      switch (other)
      {
        case ExistentLink existentLink:
          var (url, title) = existentLink;
          var newUrl = url ?? Url;
          return new ExistentLink(newUrl, newUrl == "" ? "" : title ?? Title);
        case NonExistentLink nonExistentLink:
          return nonExistentLink;
        default:
          throw new ArgumentOutOfRangeException(nameof(other));

      }

    }
  }
}
