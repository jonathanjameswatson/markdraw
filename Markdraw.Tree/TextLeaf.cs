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

    private List<TextInsert> _correspondingInserts
    {
      set
      {
        _correspondingInserts = value;
        _text = String.Join("", value.Select(textInsert => textInsert.Text));
      }
    }
    private string _text;

    public List<TextInsert> CorrespondingInserts { get; }
    public string Text { get => _text; }

    public TextLeaf(List<TextInsert> correspondingInserts)
    {
      _correspondingInserts = correspondingInserts;
    }
  }
}
