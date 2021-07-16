using System;
using Xunit;

namespace Markdraw.Parser.Test
{
  public class ParserTest
  {
    [Fact]
    public void SimpleTest()
    {
      Parser.Parse("Test")
        .ToString()
        .Is(Parser.Prettify(@"<p>Test</p>"));
    }
  }
}