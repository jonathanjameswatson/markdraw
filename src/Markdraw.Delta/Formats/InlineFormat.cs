using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Markdraw.Delta.Styles;

namespace Markdraw.Delta.Formats
{
  public record InlineFormat([NotNull] ImmutableList<Style> Styles, bool Code = false) : Format
  {
    public static readonly InlineFormat BoldPreset = new(ImmutableList.Create<Style>(Style.Bold));
    public static readonly InlineFormat ItalicPreset = new(ImmutableList.Create<Style>(Style.Italic));

    private readonly Lazy<int> _hashCode = new(() => {
      var hash = new HashCode();
      foreach (var style in Styles)
      {
        hash.Add(style);
      }
      hash.Add(Code);
      return hash.ToHashCode();
    });

    public InlineFormat() : this(ImmutableList<Style>.Empty) {}

    public virtual bool Equals(InlineFormat other)
    {
      return other is not null && Code == other.Code && Styles.SequenceEqual(other.Styles);
    }

    public string Wrap(string text)
    {
      var intermediate = text.TrimStart();
      if (Code)
      {
        intermediate = $"`{intermediate}`";
      }
      return Styles.Aggregate(intermediate, (current, style) => style.Wrap(current));
    }

    public override int GetHashCode()
    {
      return _hashCode.Value;
    }

    public override string ToString()
    {
      var stringBuilder = new StringBuilder("{");
      var tags = Styles.Select(style => style.ToString());
      if (Code)
      {
        tags = tags.Append("<CODE>");
      }
      stringBuilder.AppendJoin(" ", tags);
      stringBuilder.Append('}');
      return stringBuilder.ToString();
    }
  }
}
