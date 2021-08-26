using System.Collections;
using System.Collections.Generic;
using System.Text;
using Markdraw.Delta.Indents;
using Markdraw.Delta.Operations.Inserts;
using Markdraw.Delta.Operations.Inserts.Inlines;
using Markdraw.Delta.OperationSequences;

namespace Markdraw.Tree
{
  public class Container : TreeNode, IEnumerable<TreeNode>
  {

    protected virtual string Tag => "div";
    protected virtual string StartingTag => null;
    protected bool Loose = true;
    protected virtual bool WrapAllInside => false;
    public List<TreeNode> ElementsInside { get; } = new();

    protected Container(DeltaTree deltaTree = null, int i = 0) : base(deltaTree, i) {}

    public static Container CreateInstance(int depth, Document document, DeltaTree deltaTree = null, int i = 0)
    {
      var container = new Container(deltaTree, i);

      return Initialise(depth, document, i, container);
    }

    protected static T Initialise<T>(int depth, Document document, int i, T container) where T : Container
    {
      var opBuffer = new Document();
      var lineOpBuffer = new Document();
      var indented = false;
      Indent lastIndent = null;
      var currentI = i;

      foreach (var op in document)
      {
        if (op is LineInsert lineInsert)
        {
          var indents = lineInsert.Format.Indents;
          var header = lineInsert.Format.Header ?? 0;
          var numberOfIndents = indents.Count;
          var goneForward = numberOfIndents > depth;
          var goneBack = numberOfIndents <= depth;

          if (indented)
          {
            if (goneBack || indents[depth] is not ContinueIndent && indents[depth] != NextIndent(lastIndent))
            {
              currentI = container.AddContainer(lastIndent, opBuffer, depth + 1, currentI);
              currentI += 1;

              if (goneBack)
              {
                indented = false;
                currentI = container.AddLeaves(lineOpBuffer, header, currentI);
                currentI += 1;
              }
              else
              {
                lastIndent = indents[depth];
                opBuffer = lineOpBuffer;
              }
            }
            else
            {
              opBuffer.InsertMany(lineOpBuffer);
            }

            opBuffer.Insert(lineInsert);
          }
          else
          {
            if (goneForward)
            {
              lastIndent = indents[depth];
              indented = true;

              opBuffer = lineOpBuffer;
              opBuffer.Insert(lineInsert);
            }
            else
            {
              currentI = container.AddLeaves(lineOpBuffer, header, currentI);
              currentI += 1;
            }
          }

          lineOpBuffer = new Document();
        }
        else
        {
          lineOpBuffer.Insert(op);
        }
      }

      if (opBuffer.Length != 0 && indented)
      {
        currentI = container.AddContainer(lastIndent, opBuffer, depth + 1, currentI);
        currentI += 1;
      }

      if (lineOpBuffer.Length != 0)
      {
        container.AddLeaves(lineOpBuffer, 0, currentI);
      }

      if (container.ElementsInside.Count > 0)
      {
        var lastElement = container.ElementsInside[^1];

        container.Length = lastElement.I + lastElement.Length - i;
      }
      else
      {
        container.Length = 0;
      }

      return container;
    }

    public Container(List<TreeNode> elementsInside, DeltaTree deltaTree = null, int i = 0) : base(deltaTree, i)
    {
      ElementsInside = elementsInside;
    }

    private static Indent NextIndent(Indent indent)
    {
      return indent switch {
        BulletIndent { Start: true } bulletIndent => bulletIndent with {
          Start = false
        },
        NumberIndent { Start: > 0 } numberIndent => numberIndent with {
          Start = 0
        },
        QuoteIndent { Start: true } quoteIndent => quoteIndent with {
          Start = false
        },
        _ => indent
      };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }


    public IEnumerator<TreeNode> GetEnumerator()
    {
      return ElementsInside.GetEnumerator();
    }

    private int AddContainer(Indent indent, Document document, int depth, int i)
    {
      var newContainer = indent switch {
        QuoteIndent => QuoteContainer.CreateInstance(depth, document, ParentTree, i),
        BulletIndent { Loose: var loose } => BulletsContainer.CreateInstance(depth, document, ParentTree, i, loose),
        NumberIndent { Loose: var loose, Start: var start } => NumbersContainer.CreateInstance(depth, document, ParentTree, i, start, loose),
        CodeIndent => QuoteContainer.CreateInstance(depth, document, ParentTree, i),
        _ => CreateInstance(depth, document, ParentTree, i)
      };

      ElementsInside.Add(newContainer);

      return i + newContainer.Length;
    }

    private int AddLeaves(Document document, int header, int i)
    {
      var inlineBuffer = new List<InlineInsert>();
      var newI = i;

      if (document.Length == 0 && header != 0)
      {
        AddEmptyInline(header);
        return i;
      }

      foreach (var op in document)
      {
        if (op is InlineInsert inlineInsert)
        {
          inlineBuffer.Add(inlineInsert);
        }
        else
        {
          if (inlineBuffer.Count != 0)
          {
            var textLeaf = new InlineLeaf(inlineBuffer, header == 0 ? "p" : $"h{header}", ParentTree, newI, Loose);
            ElementsInside.Add(textLeaf);
            inlineBuffer = new List<InlineInsert>();
            newI += textLeaf.Length;
          }

          Leaf newElement = op switch {
            DividerInsert dividerInsert => new DividerLeaf(dividerInsert, ParentTree, newI),
            CodeInsert codeInsert => new CodeLeaf(codeInsert, ParentTree, newI),
            BlockHtmlInsert blockHtmlInsert => new BlockHtmlLeaf(blockHtmlInsert, ParentTree, newI),
            _ => null
          };

          ElementsInside.Add(newElement);

          newI += 1;
        }
      }

      if (inlineBuffer.Count == 0) return newI;

      var finalTextLeaf = new InlineLeaf(inlineBuffer, header == 0 ? "p" : $"h{header}", ParentTree, newI, Loose);
      ElementsInside.Add(finalTextLeaf);
      newI += finalTextLeaf.Length;

      return newI;
    }

    private void AddEmptyInline(int header)
    {
      ElementsInside.Add(new InlineLeaf(new List<InlineInsert>(), header));
    }

    public override string ToString()
    {
      var stringBuilder = new StringBuilder();

      stringBuilder.Append(StartingTag ?? $@"<{Tag}>");

      foreach (var child in ElementsInside)
      {
        if (WrapAllInside)
        {
          stringBuilder.Append(@"<li>");
        }

        stringBuilder.Append(child);

        if (WrapAllInside)
        {
          stringBuilder.Append(@"</li>");
        }
      }

      stringBuilder.Append($@"</{Tag}>");

      return stringBuilder.ToString();
    }
  }

}
