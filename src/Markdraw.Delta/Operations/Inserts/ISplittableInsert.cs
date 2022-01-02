namespace Markdraw.Delta.Operations.Inserts;

public interface ISplittableInsert : IInsert
{
  ISplittableInsert? Merge(ISplittableInsert before);

  ISplittableInsert? Merge(ISplittableInsert middle, ISplittableInsert before);

  ISplittableInsert? DeleteUpTo(int position);

  (ISplittableInsert, ISplittableInsert)? SplitAt(int position);
}
