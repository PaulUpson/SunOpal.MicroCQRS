using System;
using System.Collections.Generic;
using System.Linq;

namespace SunOpal.MicroCQRS.Sagas
{
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
      return default;
    }

    T ISagaPersister.Get<T>(Guid sagaId)
    {
      lock (syncRoot) {
        data.TryGetValue(sagaId, out ISagaEntity result);
        if ((result != null) && (result is T))
          return (T) result;
      }
      return default;
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