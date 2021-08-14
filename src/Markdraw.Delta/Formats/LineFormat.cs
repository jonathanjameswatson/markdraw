using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using Markdraw.Delta.Indents;

namespace Markdraw.Delta.Formats
{
  public record LineFormat : Format, ILineFormatModifier
  {

    public static readonly LineFormat QuotePreset = new() {
      Indents = ImmutableList.Create<Indent>(Indent.Quote)
    };
    public static readonly LineFormat BulletPreset = new() {
      Indents = ImmutableList.Create<Indent>(Indent.LooseBullet)
    };
    public static readonly LineFormat NumberPreset = new() {
      Indents = ImmutableList.Create<Indent>(Indent.LooseNumber)
    };
    public static readonly LineFormat CodePreset = new() {
      Indents = ImmutableList.Create<Indent>(Indent.Code)
    };
    private readonly Lazy<int> _hashCode;
    public ImmutableList<Indent> Indents { get; init; } = ImmutableList<Indent>.Empty;

    public int? Header { get; init; } = 0;

    public LineFormat()
    {
      _hashCode = new Lazy<int>(() => {
        var hash = new HashCode();
        foreach (var indent in Indents)
        {
          hash.Add(indent);
        }
        hash.Add(Header);
        return hash.ToHashCode();
      });
    }

    public virtual bool Equals(LineFormat other)
    {
      return other is not null && Header == other.Header && Indents.SequenceEqual(other.Indents);
    }

    public LineFormat Modify(LineFormat format)
    {
      if (format.Equals(this)) return null;
      return format with {
        Indents = Indents ?? format.Indents, Header = Header ?? format.Header
      };
    }

    public override int GetHashCode()
    {
      return _hashCode.Value;
    }
  }
}
