using System;

namespace Markdraw.Delta.Links
{
  public record NonExistentLink : Link
  {
    public override Link Merge(Link other)
    {
      switch (other)
      {
        case null or NonExistentLink:
          return this;
        case ExistentLink existentLink:
          var (url, _) = existentLink;
          if (url is not null)
          {
            return existentLink;
          }
          return this;
        default:
          throw new ArgumentOutOfRangeException(nameof(other));
      }
    }
  };
}
