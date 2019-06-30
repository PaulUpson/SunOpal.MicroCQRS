using System;

namespace SunOpal.MicroCQRS {
  public interface ISerializationStrategy {
    string Serialize(object data);
    object Deserialize(string data, Type type);
  }
}