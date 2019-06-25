using BigTree.MicroCQRS.DocumentStore;
using NUnit.Framework;

namespace BigTree.MicroCQRS.Tests
{
  [TestFixture]
  public class writing_to_memory_store
  {
    private IDocumentStrategy _strategy;
    private IDocumentStore _store;

    [SetUp]
    public void SetUp()
    {
      _strategy = new DocumentStrategy();
      _store = new MemoryDocumentStore(_strategy, new MemoryStorageConfig().Data);
    }

    [Test]
    public void can_write_to_store()
    {
      var sut = new TestObject {Name = "TestObject", Id = 1, OtherData = "some other data to test the serialization."};

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
