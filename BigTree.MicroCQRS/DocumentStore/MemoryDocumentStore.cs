using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace BigTree.MicroCQRS.DocumentStore
{
  /// <summary>
  /// This class acts as a document factory for all individual memory document Reader/Writers.
  /// </summary>
  public sealed class MemoryDocumentStore : IDocumentStore
  {
    private readonly IDocumentStrategy _strategy;
    private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, byte[]>> _store;


    public MemoryDocumentStore(IDocumentStrategy strategy, ConcurrentDictionary<string, ConcurrentDictionary<string, byte[]>> store)
    {
      _strategy = strategy;
      _store = store;
    }

    public IDocumentStrategy Strategy
    {
      get { return _strategy; }
    }

    public IDocumentWriter<TKey, TEntity> GetWriter<TKey, TEntity>()
    {
      var bucket = _strategy.GetEntityBucket<TEntity>();
      var store = _store.GetOrAdd(bucket, s => new ConcurrentDictionary<string, byte[]>());
      return new MemoryDocumentReaderWriter<TKey, TEntity>(_strategy, store);
    }

    public void WriteContents(string bucket, IEnumerable<DocumentRecord> records)
    {
      var pairs = records.Select(r => new KeyValuePair<string, byte[]>(r.Key, r.Read())).ToArray();
      _store[bucket] = new ConcurrentDictionary<string, byte[]>(pairs);
    }

    public void ResetAll()
    {
      _store.Clear();
    }

    public void Reset(string bucket)
    {
      throw new NotSupportedException();
    }

    public IDocumentReader<TKey, TEntity> GetReader<TKey, TEntity>()
    {
      var bucket = _strategy.GetEntityBucket<TEntity>();
      var store = _store.GetOrAdd(bucket, s => new ConcurrentDictionary<string, byte[]>());
      return new MemoryDocumentReaderWriter<TKey, TEntity>(_strategy, store);
    }
    
    public IEnumerable<DocumentRecord> EnumerateContents(string bucket)
    {
      var store = _store.GetOrAdd(bucket, s => new ConcurrentDictionary<string, byte[]>());
      return store.Select(p => new DocumentRecord(p.Key, () => p.Value)).ToArray();
    }
  }
}