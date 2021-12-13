using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Markdraw.Delta.Indents;

namespace Markdraw.Delta.Formats
{
  public record LineFormat([NotNull] ImmutableList<Indent> Indents, int Header = 0) : Format
  {
    public static readonly LineFormat QuotePreset = new(ImmutableList.Create<Indent>(Indent.Quote));
    public static readonly LineFormat BulletPreset = new(ImmutableList.Create<Indent>(Indent.LooseBullet));
    public static readonly LineFormat NumberPreset = new(ImmutableList.Create<Indent>(Indent.LooseNumber));
    public static readonly LineFormat CodePreset = new(ImmutableList.Create<Indent>(Indent.Code));

    private readonly Lazy<int> _hashCode = new(() => {
      var hash = new HashCode();
      foreach (var indent in Indents)
      {
        hash.Add(indent);
      }
      hash.Add(Header);
      return hash.ToHashCode();
    });

    public LineFormat() : this(ImmutableList<Indent>.Empty) {}

    public virtual bool Equals(LineFormat other)
    {
      return other is not null && Header == other.Header && Indents.SequenceEqual(other.Indents);
    }

    public override int GetHashCode()
    {
      return _hashCode.Value;
    }

    public override string ToString()
    {
      var indentString = string.Join("", Indents);
      if (indentString != "")
      {
        indentString += " ";
      }
      return $"{{{indentString}{Header}}}";
    }
  }
}
