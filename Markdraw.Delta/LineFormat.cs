using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Markdraw.Delta
{
  public record LineFormat : Format
  {

    public static readonly LineFormat QuotePreset = new() {
      Indents = ImmutableList.Create(
        Indent.Quote, Indent.Empty(1)
      )
    };
    public static readonly LineFormat BulletPreset = new() {
      Indents = ImmutableList.Create(Indent.Bullet, Indent.Empty(1))
    };
    public static readonly LineFormat NumberPreset = new() {
      Indents = ImmutableList.Create(Indent.Number(2), Indent.Empty(1))
    };
    public static readonly LineFormat CodePreset = new() {
      Indents = ImmutableList.Create(
        Indent.Code
      )
    };
    private int? _hashCode;
    public ImmutableList<Indent> Indents { get; init; } = ImmutableList<Indent>.Empty;
    public ImmutableList<Indent> NonEmptyIndents => Indents.Where(indent => !indent.IsEmpty()).ToImmutableList();

    public int? Header { get; init; } = 0;

    public virtual bool Equals(LineFormat other)
    {
      return other is not null && Header == other.Header && Indents.SequenceEqual(other.Indents);
    }

    public LineFormat Merge(LineFormat other)
    {
      return new LineFormat {
        Indents = other.Indents ?? Indents, Header = other.Header ?? Header
      };
    }

    public LineFormat Modify(ModifyingLineFormat other)
    {
      Debug.Assert(Header != null, nameof(Header) + " != null");
      return new LineFormat {
        Indents = other.IndentsFunction(Indents), Header = other.HeaderFunction((int)Header)
      };
    }

    [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
    public override int GetHashCode()
    {
      if (_hashCode is not null) return (int)_hashCode;

      var hash = new HashCode();
      foreach (var indent in Indents)
      {
        hash.Add(indent);
      }
      hash.Add(Header);
      _hashCode = hash.ToHashCode();

      return (int)_hashCode;
    }
  }
}
