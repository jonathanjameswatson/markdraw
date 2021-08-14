namespace Markdraw.Delta.Formats
{
  public interface ILineFormatModifier
  {
    public LineFormat Modify(LineFormat format);
  }
}
