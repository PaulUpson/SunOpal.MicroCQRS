using System.IO;

namespace BigTree.MicroCQRS.DocumentStore
{
  public interface IDocumentStrategy
  {
    string GetEntityBucket<TEntity>();
    string GetEntityLocation<TEntity>(object key);

    void Serialize<TEntity>(TEntity entity, Stream stream);
    TEntity Deserialize<TEntity>(Stream stream);
  }
}