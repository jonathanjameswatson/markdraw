using System;

namespace Markdraw.Delta.Operations.Inserts
{
  public class ImageInsert : InlineInsert
  {

    public ImageInsert(string url = "", string alt = "", string title = "")
    {
      Url = url;
      Alt = alt;
      Title = title;
    }
    public string Url { get; set; }
    public string Alt { get; set; }
    public string Title { get; set; }

    public override bool Equals(object obj)
    {
      return obj is ImageInsert x && x.Url == Url && x.Alt == Alt && x.Title == Title;
    }

    public override int GetHashCode()
    {
      return HashCode.Combine(Url, Alt, Title);
    }

    public override string ToString()
    {
      return $"![{Alt}]({Url})";
    }
  }
}
