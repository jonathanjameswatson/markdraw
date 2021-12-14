namespace Markdraw.Delta.Styles
{
  public abstract record Style
  {
    public static Bold Bold => new();
    public static Italic Italic => new();

    public static Link Link(string url = "", string title = "")
    {
      return new Link(url, title);
    }

    public abstract string Wrap(string contents);

    public override string ToString()
    {
      return "<Style>";
    }
  }
}
