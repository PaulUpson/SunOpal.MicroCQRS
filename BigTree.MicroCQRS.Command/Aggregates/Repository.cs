using System;

namespace BigTree.MicroCQRS {
  public interface IRepository<T> where T : AggregateRoot, new()
  {
    void Save(AggregateRoot aggregate, int expectedVersion);
    T GetById(Guid id);
  }

  /// <summary>
  /// Simple repository for writing the state of the aggregate root
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class Repository<T> : IRepository<T> where T : AggregateRoot, new() //shortcut you can do as you see fit with new()
  {
    private readonly IEventStore _storage;

    public Repository(IEventStore storage)
    {
      _storage = storage;
    }

    public void Save(AggregateRoot aggregate, int expectedVersion)
    {
      _storage.SaveEvents(aggregate.Id, typeof(T), aggregate.GetUncommittedChanges(), expectedVersion);
    }

    public T GetById(Guid id)
    {
      var obj = new T();//lots of ways to do this
      var e = _storage.GetEventsForAggregate(id);
      obj.LoadFromHistory(e);
      return obj;
    }
  }
}