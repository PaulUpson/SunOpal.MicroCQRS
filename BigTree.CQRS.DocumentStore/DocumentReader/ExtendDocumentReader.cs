using System;

namespace BigTree.MicroCQRS.DocumentStore
{
  public static class ExtendDocumentReader
  {
     public static TEntity Load<TKey, TEntity>(this IDocumentReader<TKey, TEntity> reader, TKey key)
     {
       TEntity entity;
       if(reader.TryGet(key, out entity))
       {
         return entity;
       }
       var txt = string.Format("Failed to load '{0}' with key '{1}'.", typeof (TEntity).Name, key);
       throw new InvalidOperationException(txt);
     }
  }
}