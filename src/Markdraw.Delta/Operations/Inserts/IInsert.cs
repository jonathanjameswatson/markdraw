using Markdraw.Delta.Formats;

namespace Markdraw.Delta.Operations.Inserts;

public interface IInsert : IOp
{
  int Length => 1;

  IInsert? SetFormat(IFormatModifier formatModifier)
  {
    return null;
  }
}
