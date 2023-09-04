using System.Collections.Generic;

namespace SunOpal.DocumentStore;

  public interface IDocumentStore
{
  IDocumentWriter<TKey, TEntity> GetWriter<TKey, TEntity>();
  IDocumentReader<TKey, TEntity> GetReader<TKey, TEntity>();
  IDocumentStrategy Strategy { get; }
  IEnumerable<DocumentRecord> EnumerateContents(string bucket);
  void WriteContents(string bucket, IEnumerable<DocumentRecord> records);
  void Reset(string bucket);
  void ResetAll();
}