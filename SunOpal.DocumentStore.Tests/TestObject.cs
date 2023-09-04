using System.Runtime.Serialization;
using Newtonsoft.Json;
using SunOpal.MicroCQRS;

namespace SunOpal.DocumentStore.Tests;

[DataContract]
public class TestObject
{
  [DataMember(Order = 1)]
  public string Name { get; set; }
  [DataMember(Order = 2)]
  public int Id { get; set; }
  [DataMember(Order = 3)]
  public string OtherData { get; set; }
}

public class TestEvent : Event {
  public readonly string Name;
  public readonly int ObjectId;
  public readonly string OtherData;

  public TestEvent(int userId, string name, int objectId, string otherData) : base(userId) {
    Name = name;
    ObjectId = objectId;
    OtherData = otherData;
  }
}

public class TestWrapper {
  public readonly string Name;
  public readonly int ObjectId;
  public readonly object Event;
  public readonly string Type;
  public readonly string OtherData;

  [JsonConstructor]
  public TestWrapper(string name, int objectId, object @event, string type, string otherData) {
    Name = name;
    ObjectId = objectId;
    Event = @event;
    Type = type;
    OtherData = otherData;
  }
}