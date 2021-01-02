using Markdraw.Delta;
using Xunit;

namespace Markdraw.MarkdownToDelta.Test
{
  public class ParseTest
  {
    [Fact(Skip = "Not implemented")]
    public void EmptyTest()
    {
      MarkdownToDeltaConverter
        .Parse("")
        .Is(new Ops());
    }
  }
}
