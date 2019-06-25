using System;
using System.Collections.Generic;

namespace BigTree.MicroCQRS.DocumentStore
{
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

  public sealed class DocumentRecord
  {
    /// <summary>
    /// Path of the view in the subfolder, using '/' as split on all platforms
    /// </summary>
    public readonly string Key;
    public readonly Func<byte[]> Read;

    public DocumentRecord(string key, Func<byte[]> read)
    {
      Key = key;
      Read = read;
    }
  }
}