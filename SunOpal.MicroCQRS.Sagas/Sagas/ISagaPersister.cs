using System;

namespace SunOpal.MicroCQRS.Sagas
{
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
}