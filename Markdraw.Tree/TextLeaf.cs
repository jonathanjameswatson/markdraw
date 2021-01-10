using System;
using System.Collections.Generic;
using System.Linq;
using Markdraw.Delta;

namespace Markdraw.Tree
{
  public class TextLeaf : Leaf
  {
    public override Insert CorrespondingInsert
    {
      get => CorrespondingInserts?[0];
    }

    private List<TextInsert> __correspondingInserts;
    private List<TextInsert> _correspondingInserts
    {
      get => __correspondingInserts;
      set
      {
        __correspondingInserts = value;
        _text = String.Join("", value.Select(textInsert => textInsert.Text));
      }
    }
    private string _text;
    public string Tag { get; set; }

    public List<TextInsert> CorrespondingInserts { get => _correspondingInserts; }
    public string Text { get => _text; }

    public TextLeaf(List<TextInsert> correspondingInserts, int header)
    {
      _correspondingInserts = correspondingInserts;
      Tag = header == 0 ? "p" : $"h{header}";
    }

    public TextLeaf(List<TextInsert> correspondingInserts) : this(correspondingInserts, 0) { }

    public override string ToString()
    {
      return $@"<{Tag}>{Text}</{Tag}>";
    }
  }
}
