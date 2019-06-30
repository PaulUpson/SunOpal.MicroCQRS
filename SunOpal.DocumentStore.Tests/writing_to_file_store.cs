using System;
using System.IO;
using System.Reflection;
using SunOpal.DocumentStore;
using NUnit.Framework;

namespace SunOpal.DocumentStore.Tests
{
  [TestFixture]
  public class writing_to_file_store
  {
    private IDocumentStrategy _strategy;
    private IDocumentStore _store;
    private TextWriter _log;

    [SetUp]
    public void SetUp()
    {
      _strategy = new DocumentStrategy();
      _log = Console.Out;
      _store = new FileDocumentStore(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase).Remove(0,6), _strategy, _log);
      _store.Reset(_strategy.GetEntityBucket<TestObject>());
    }

    [Test]
    public void can_write_to_store()
    {
      var sut = new TestObject { Name = "TestObject", Id = 1, OtherData = "some other data to test the serialization." };

      var writer = _store.GetWriter<string, TestObject>();
      writer.Add(sut.Name, sut);

      var reader = _store.GetReader<string, TestObject>();
      var result = reader.Load(sut.Name);

      Assert.AreEqual(sut.Name, result.Name);
      Assert.AreEqual(sut.Id, result.Id);
      Assert.AreEqual(sut.OtherData, result.OtherData);
    }
  }
}