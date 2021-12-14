using Markdraw.Delta.Formats;

namespace Markdraw.Delta.Operations.Inserts.Inlines
{
  public record ImageInsert(string Url, string Alt, string Title, InlineFormat Format) : InlineInsert(Format)
  {
    public ImageInsert(string url, string alt = "", string title = "") : this(url, alt, title, new InlineFormat()) {}

    public override string ToString()
    {
      return Format.Wrap($"![{Alt}]({Url})");
    }
  }
}
