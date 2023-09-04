using System;
using System.IO;
using System.Linq;
using ProtoBuf;

namespace SunOpal.DocumentStore;

public class ViewStrategy : IDocumentStrategy
{
  public string GetEntityBucket<TEntity>() => GlobalConfig.Conventions.ViewsFolder + "/" + NameCache<TEntity>.Name;

  public string GetEntityLocation<TEntity>(object key)
  {
    if(key is unit)
      return NameCache<TEntity>.Name + ".pb";

    if(key is Guid guid)
    {
      var b = (byte) ((uint) guid.GetHashCode() % 251);
      return b + "/" + key.ToString().ToLowerInvariant() + ".pb";
    }

    if(key is string v)
    {
      var corrected = v.ToLowerInvariant().Trim();
      var b = (byte) ((uint) CalculatedStringHash(corrected)%251);
      return b + "/" + corrected + ".pb";
    }

    return null;
  }

  static int CalculatedStringHash(string value)
  {
    if(value == null) return 0;
    unchecked
    {
      return value.Aggregate(2, (current, c) => current*31 + c);
    }
  }

  public void Serialize<TEntity>(TEntity entity, Stream stream)
  {
    // ProtoBuf must have non-zero files
    stream.WriteByte(42);
    Serializer.Serialize(stream, entity);
  }

  public TEntity Deserialize<TEntity>(Stream stream)
  {
    var signature = stream.ReadByte();
    if(signature != 42)
      throw new InvalidOperationException("Unknown view format");

    return Serializer.Deserialize<TEntity>(stream);
  }
}