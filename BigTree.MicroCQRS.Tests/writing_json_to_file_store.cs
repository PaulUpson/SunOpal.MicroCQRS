using System;
using System.IO;
using System.Reflection;
using BigTree.MicroCQRS.DocumentStore;
using NUnit.Framework;
using Newtonsoft.Json;

namespace BigTree.MicroCQRS.Tests
{
  [TestFixture]
  public class writing_json_to_file_store
  {
    private IDocumentStrategy _strategy;
    private IDocumentStore _store;

    [SetUp]
    public void SetUp()
    {
      _strategy = new JsonDocumentStrategy();
      _store = new FileDocumentStore(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase).Remove(0, 6), _strategy);
      _store.Reset(_strategy.GetEntityBucket<TestObject>());
      _store.Reset(_strategy.GetEntityBucket<TestEvent>());
      _store.Reset(_strategy.GetEntityBucket<TestWrapper>());
    }

    [Test]
    public void can_write_to_store()
    {
      var sut = new TestObject { Name = "JsonObject", Id = 1, OtherData = "some other data to test the serialization." };

      var writer = _store.GetWriter<string, TestObject>();
      writer.Add(sut.Name, sut);

      var reader = _store.GetReader<string, TestObject>();
      var result = reader.Load(sut.Name);

      Assert.AreEqual(sut.Name, result.Name);
      Assert.AreEqual(sut.Id, result.Id);
      Assert.AreEqual(sut.OtherData, result.OtherData);
    }

    [Test]
    public void can_write_TestEvent_to_store()
    {
      var sut = new TestEvent(1234, "TestEvent", 4, "some other datat to test the serialization.");

      var writer = _store.GetWriter<string, TestEvent>();
      writer.Add(sut.Name, sut);

      var reader = _store.GetReader<string, TestEvent>();
      var result = reader.Load(sut.Name);

      Assert.AreEqual(sut.Name, result.Name);
      Assert.AreEqual(sut.ObjectId, result.ObjectId);
      Assert.AreEqual(sut.OtherData, result.OtherData);
    }

    [Test]
    public void can_write_TestWrapper_to_store()
    {
      var testEvent = new TestEvent(1234, "TestEvent", 4, "some other datat to test the serialization.");
      var sut = new TestWrapper("TestWrapper", 1, testEvent, testEvent.GetType().AssemblyQualifiedName, "some other data etc");

      var writer = _store.GetWriter<string, TestWrapper>();
      writer.Add(sut.Name, sut);

      var reader = _store.GetReader<string, TestWrapper>();
      var result = reader.Load(sut.Name);

      Assert.AreEqual(sut.Name, result.Name);
      Assert.AreEqual(sut.ObjectId, result.ObjectId);
      Assert.AreEqual(sut.OtherData, result.OtherData);
      var returnEvent = (Event)JsonConvert.DeserializeObject(result.Event.ToString(), Type.GetType(result.Type));
      Assert.AreEqual(testEvent.UserId, returnEvent.UserId);
    }
  }
}