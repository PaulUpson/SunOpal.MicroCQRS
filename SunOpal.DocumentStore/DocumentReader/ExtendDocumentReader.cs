using System;

namespace SunOpal.DocumentStore;

public static class ExtendDocumentReader
{
  public static TEntity Load<TKey, TEntity>(this IDocumentReader<TKey, TEntity> reader, TKey key)
  {
    if(reader.TryGet(key, out TEntity entity)) return entity;      
    throw new InvalidOperationException($"Failed to load '{typeof (TEntity).Name}' with key '{key}'.");
  }
}