using System;
using System.IO;
using ProtoBuf;

namespace SunOpal.DocumentStore;

public class ProtoDocumentStrategy : IDocumentStrategy
{
  public string GetEntityBucket<TEntity>()
  {
    return GlobalConfig.Conventions.DocsFolder + "/" + NameCache<TEntity>.Name;
  }

  public string GetEntityLocation<TEntity>(object key)
  {
    if(key is unit)
      return NameCache<TEntity>.Name + ".proto";

    return key.ToString().ToLowerInvariant() + ".proto";
  }

  public void Serialize<TEntity>(TEntity entity, Stream stream)
  {
    stream.WriteByte(42);
    Serializer.Serialize(stream, entity);
  }

  public TEntity Deserialize<TEntity>(Stream stream)
  {
    var signature = stream.ReadByte();
    if(signature != 42) throw new InvalidOperationException("Unknown document format");
    return Serializer.Deserialize<TEntity>(stream);
  }
}