using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SunOpal.DocumentStore
{
  public class JsonDocumentStrategy : IDocumentStrategy
  {
    public string GetEntityBucket<TEntity>()
    {
      return GlobalConfig.Conventions.DocsFolder + "/" + NameCache<TEntity>.Name;
    }

    public string GetEntityLocation<TEntity>(object key)
    {
      if(key is unit)
        return NameCache<TEntity>.Name + ".json";

      return key.ToString().ToLowerInvariant() + ".json";
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
}