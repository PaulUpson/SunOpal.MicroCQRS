using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BigTree.MicroCQRS {
  public sealed class JsonSerializationStrategy : ISerializationStrategy {
    public string Serialize(object data) {
      return JsonConvert.SerializeObject(data, new StringEnumConverter());
    }

    public object Deserialize(string data, Type type) {
      return JsonConvert.DeserializeObject(data, type);
    }
  }
}