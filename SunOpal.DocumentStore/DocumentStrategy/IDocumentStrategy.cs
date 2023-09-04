using System.IO;

namespace SunOpal.DocumentStore;

/// <summary>
/// Document storage and retrieval strategy
/// </summary>
public interface IDocumentStrategy
{
  string GetEntityBucket<TEntity>();
  string GetEntityLocation<TEntity>(object key);

  void Serialize<TEntity>(TEntity entity, Stream stream);
  TEntity Deserialize<TEntity>(Stream stream);
}