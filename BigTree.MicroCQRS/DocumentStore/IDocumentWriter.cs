using System;

namespace BigTree.MicroCQRS.DocumentStore
{
  public interface IDocumentWriter<in TKey, TEntity>
  {
    TEntity AddOrUpdate(TKey key, Func<TEntity> addFactory, Func<TEntity, TEntity> update, AddOrUpdateHint hint = AddOrUpdateHint.ProbablyExists);
    bool TryDelete(TKey key);
  }
}