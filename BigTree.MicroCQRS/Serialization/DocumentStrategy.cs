using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using BigTree.MicroCQRS.DocumentStore;
using DocumentStore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ProtoBuf;

namespace BigTree.MicroCQRS
{
  public class ViewStrategy : IDocumentStrategy
  {
    public string GetEntityBucket<TEntity>()
    {
      return GlobalConfig.Conventions.ViewsFolder + "/" + NameCache<TEntity>.Name;
    }

    public string GetEntityLocation<TEntity>(object key)
    {
      if(key is unit)
        return NameCache<TEntity>.Name + ".pb";

      if(key is Guid)
      {
        var b = (byte) ((uint) ((Guid) key).GetHashCode() % 251);
        return b + "/" + key.ToString().ToLowerInvariant() + ".pb";
      }

      if(key is string)
      {
        var corrected = ((string) key).ToLowerInvariant().Trim();
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

  public class DocumentStrategy : IDocumentStrategy
  {
    public string GetEntityBucket<TEntity>()
    {
      return GlobalConfig.Conventions.DocsFolder + "/" + NameCache<TEntity>.Name;
    }

    public string GetEntityLocation<TEntity>(object key)
    {
      if(key is unit)
        return NameCache<TEntity>.Name + ".pb";

      return key.ToString().ToLowerInvariant() + ".pb";
    }

    public void Serialize<TEntity>(TEntity entity, Stream stream)
    {
      stream.WriteByte(42);
      Serializer.Serialize(stream, entity);
    }

    public TEntity Deserialize<TEntity>(Stream stream)
    {
      var signature = stream.ReadByte();
      if(signature != 42)
        throw new InvalidOperationException("Unknown document format");

      return Serializer.Deserialize<TEntity>(stream);
    }
  }

  public class JsonDocumentStrategy : IDocumentStrategy
  {
    public string GetEntityBucket<TEntity>()
    {
      return GlobalConfig.Conventions.DocsFolder + "/" + NameCache<TEntity>.Name;
    }

    public string GetEntityLocation<TEntity>(object key)
    {
      if(key is unit)
        return NameCache<TEntity>.Name + ".js";

      return key.ToString().ToLowerInvariant() + ".js";
    }

    public void Serialize<TEntity>(TEntity entity, Stream stream)
    {
      using (var writer = new StreamWriter(stream))
      {
        writer.Write(JsonConvert.SerializeObject(entity, new StringEnumConverter()));
      }
    }

    public TEntity Deserialize<TEntity>(Stream stream)
    {
      using (var reader = new StreamReader(stream))
      {
        var result = reader.ReadToEnd();
        return JsonConvert.DeserializeObject<TEntity>(result);
      }
    }
  }

  static class NameCache<T>
  {
    public static readonly string Name;
    public static readonly string Namespace;

    static NameCache()
    {
      var type = typeof(T);

      Name = new string(Splice(type.Name).ToArray()).TrimStart('-');
      var dcs = type.GetCustomAttributes(false).OfType<DataContractAttribute>().ToArray();

      if (dcs.Length <= 0) return;
      var attribute = dcs.First();

      if (!string.IsNullOrEmpty(attribute.Name))
      {
        Name = attribute.Name;
      }

      if (!string.IsNullOrEmpty(attribute.Namespace))
      {
        Namespace = attribute.Namespace;
      }
    }

    static IEnumerable<char> Splice(string source)
    {
      foreach (var c in source)
      {
        if (char.IsUpper(c))
        {
          yield return '-';
        }
        yield return char.ToLower(c);
      }
    }
  }
}