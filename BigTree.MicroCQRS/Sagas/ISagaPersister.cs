using System;
using System.Collections.Generic;
using System.Linq;

namespace BigTree.MicroCQRS.Sagas {
  public interface ISagaPersister {

    /// <summary>
    /// saves the saga entity to the persistance store.
    /// </summary>
    /// <param name="saga">Saga entity to save</param>
    void Save(ISagaEntity saga);

    /// <summary>
    /// Updates an existing saga entity in the persistence store.
    /// </summary>
    /// <param name="saga">Saga entity to update</param>
    void Update(ISagaEntity saga);

    /// <summary>
    /// Gets a saga entity from the persistence store by its Id.
    /// </summary>
    /// <param name="sagaId">The Id of the saga entity to get.</param>
    /// <returns></returns>
    T Get<T>(Guid sagaId) where T : ISagaEntity;

    T Get<T>(string property, object value);

    /// <summary>
    /// Sets a saga as completed and removes it from the active saga list.
    /// </summary>
    /// <param name="saga"></param>
    void Complete(ISagaEntity saga);
  }

  public class InMemorySagaPersister : ISagaPersister {
    private readonly Dictionary<Guid, ISagaEntity> data = new Dictionary<Guid, ISagaEntity>();
    private readonly object syncRoot = new object();

    void ISagaPersister.Complete(ISagaEntity saga) {
      lock (syncRoot) {
        data.Remove(saga.Id);
      }
    }

    T ISagaPersister.Get<T>(string property, object value)
    {
      lock(syncRoot) {
        var values = data.Values.Where(x => x is T);
        foreach (var entity in values) {
          var prop = entity.GetType().GetProperty(property);
          if (prop != null)
            if (prop.GetValue(entity, null).Equals(value))
              return (T) entity;
        }
      }
      return default(T);
    }

    T ISagaPersister.Get<T>(Guid sagaId)
    {
      lock (syncRoot) {
        ISagaEntity result;
        data.TryGetValue(sagaId, out result);
        if ((result != null) && (result is T))
          return (T) result;
      }
      return default(T);
    }

    void ISagaPersister.Save(ISagaEntity saga) {
      lock(syncRoot) {
        data[saga.Id] = saga;
      }
    }
    void ISagaPersister.Update(ISagaEntity saga) {
      ((ISagaPersister)this).Save(saga);
    }
    

    
  }
}