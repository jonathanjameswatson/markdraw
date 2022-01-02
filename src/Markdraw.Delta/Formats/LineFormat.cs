using System.Collections;
using System.Collections.Immutable;
using System.Text;
using Markdraw.Delta.Indents;

namespace Markdraw.Delta.Formats;

public record LineFormat(ImmutableList<Indent> Indents, int Header = 0) : Format, IEnumerable<Indent>
{
  public static readonly LineFormat QuotePreset = new(ImmutableList.Create<Indent>(Indent.Quote));
  public static readonly LineFormat BulletPreset = new(ImmutableList.Create<Indent>(Indent.LooseBullet));
  public static readonly LineFormat NumberPreset = new(ImmutableList.Create<Indent>(Indent.LooseNumber));

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

  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator();
  }

  public IEnumerator<Indent> GetEnumerator()
  {
    return Indents.GetEnumerator();
  }

  public virtual bool Equals(LineFormat? other)
  {
    return other is not null && Header == other.Header && Indents.SequenceEqual(other.Indents);
  }

  public override int GetHashCode()
  {
    return _hashCode.Value;
  }

  public override string ToString()
  {
    var stringBuilder = new StringBuilder();
    stringBuilder.Append('{');
    stringBuilder.Append(string.Concat(Indents));
    if (Indents.Count > 0)
    {
      stringBuilder.Append(' ');
    }
    stringBuilder.Append(Header);
    stringBuilder.Append('}');
    return stringBuilder.ToString();
  }
}
