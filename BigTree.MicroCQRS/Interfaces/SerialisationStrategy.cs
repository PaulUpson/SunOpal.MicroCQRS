using System;

namespace BigTree.MicroCQRS {
  public interface ISerializationStrategy {
    string Serialize(object data);
    object Deserialize(string data, Type type);
  }
}