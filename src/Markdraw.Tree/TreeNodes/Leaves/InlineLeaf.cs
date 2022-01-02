using System.Text;
using Markdraw.Delta.Operations.Inserts.Inlines;
using Markdraw.Helpers;

namespace Markdraw.Tree.TreeNodes.Leaves;

public class InlineLeaf : Leaf
{
  public InlineLeaf(InlineInsert inlineInsert, DeltaTree? deltaTree = null, int i = 0) : base(deltaTree, i)
  {
    CorrespondingInsert = inlineInsert;
  }

  protected override InlineInsert CorrespondingInsert { get; }

  public override int Length
  {
    get => CorrespondingInsert.Length;
    protected set => throw new InvalidOperationException("Change the corresponding insert instead.");
  }

  private static string ImageTag(ImageInsert imageInsert)
  {
    var (url, alt, title, _) = imageInsert;
    var stringBuilder = new StringBuilder(@"<img src=""");
    stringBuilder.Append(url);
    stringBuilder.Append(@""" alt=""");
    stringBuilder.Append(alt);
    stringBuilder.Append('"');
    if (!title.Equals("", StringComparison.Ordinal))
    {
      stringBuilder.Append(@" title=""");
      stringBuilder.Append(EscapeHelpers.Escape(title));
      stringBuilder.Append('"');
    }
    stringBuilder.Append("/>");
    return stringBuilder.ToString();
  }

  public override string ToString()
  {
    return CorrespondingInsert switch {
      ImageInsert imageInsert => ImageTag(imageInsert),
      InlineHtmlInsert { Content: var content } => content,
      TextInsert { Text: var text } => EscapeHelpers.Escape(text),
      _ => throw new InvalidOperationException("Unsupported inline leaf insert given.")
    };
  }
}
