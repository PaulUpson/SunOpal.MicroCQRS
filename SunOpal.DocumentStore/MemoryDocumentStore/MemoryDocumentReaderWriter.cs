using System;
using System.Collections.Concurrent;
using System.IO;

namespace SunOpal.DocumentStore;

/// <summary>
/// This is a device for reading and writing to a single document in memory, as represented by the TKey and TEntity components.
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TEntity"></typeparam>
public sealed class MemoryDocumentReaderWriter<TKey, TEntity> : IDocumentReader<TKey, TEntity>, IDocumentWriter<TKey, TEntity>
{
  private readonly IDocumentStrategy _strategy;
  private readonly ConcurrentDictionary<string, byte[]> _store;

  public MemoryDocumentReaderWriter(IDocumentStrategy strategy, ConcurrentDictionary<string, byte[]> store)
  {
    _strategy = strategy;
    _store = store;
  }

  string GetName(TKey key) => _strategy.GetEntityLocation<TEntity>(key);

  public bool TryGet(TKey key, out TEntity entity)
  {
    var name = GetName(key);
    if (_store.TryGetValue(name, out byte[] bytes)) // Get the byte array from the store
    {
      using var mem = new MemoryStream(bytes);
      // Using a memory stream deserialise the stream to an entity
      entity = _strategy.Deserialize<TEntity>(mem);
      return true;
    }
    entity = default;
    return false;
  }

  public TEntity AddOrUpdate(TKey key, Func<TEntity> addFactory, Func<TEntity, TEntity> update, AddOrUpdateHint hint = AddOrUpdateHint.ProbablyExists)
  {
    var result = default(TEntity);
    // Adds a key/value pair to the ConcurrentDictionary<TKey, TEntity> if the key does not exist, 
    // or updates a key/value pair in the ConcurrentDictionary<TKey, TEntity> if the key already exists.
    _store.AddOrUpdate(GetName(key), s =>
        {
          result = addFactory();
          using var memory = new MemoryStream();
          _strategy.Serialize(result, memory);
          return memory.ToArray();
        }, (s, bytes) =>
            {
              TEntity entity;
              using (var memory = new MemoryStream(bytes))
              {
                entity = _strategy.Deserialize<TEntity>(memory);
              }
              result = update(entity);
              using (var memory = new MemoryStream())
              {
                _strategy.Serialize(result, memory);
                return memory.ToArray();
              }
            });
    return result;
  }

  public bool TryDelete(TKey key) => _store.TryRemove(GetName(key), out byte[] _);
}