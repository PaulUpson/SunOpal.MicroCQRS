using System.Runtime.Serialization;

namespace BigTree.MicroCQRS.Tests
{
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
}