using System;
using DocumentStore;

namespace BigTree.MicroCQRS.DocumentStore
{
  public static class ExtendDocumentWriter
  {
    /// <summary>
    /// Given a <paramref name="key"/> either adds a new <typeparamref name="TEntity"/> OR updates an existing one.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="writer">The self.</param>
    /// <param name="key">The key.</param>
    /// <param name="entity">The new view that will be saved, if entity does not already exist</param>
    /// <param name="updateFactory">The update method (called to update an existing entity, if it exists).</param>
    /// <param name="hint">The hint.</param>
    /// <returns></returns>
    public static TEntity AddOrUpdate<TKey, TEntity>(this IDocumentWriter<TKey, TEntity> writer, TKey key, TEntity entity, 
      Action<TEntity> updateFactory, AddOrUpdateHint hint = AddOrUpdateHint.ProbablyExists)
    {
      return writer.AddOrUpdate(key, () => entity, e =>
          {
            updateFactory(e);
            return e;
          }, hint);
    }

    /// <summary>
    /// Saves new entity, using the provided <param name="key"></param> and throws 
    /// <exception cref="InvalidOperationException"></exception> if the entity actually already exists
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="writer">The self.</param>
    /// <param name="key">The key.</param>
    /// <param name="newEntity">The new entity.</param>
    /// <returns></returns>
    public static TEntity Add<TKey,TEntity>(this IDocumentWriter<TKey, TEntity> writer, TKey key, TEntity newEntity)
    {
       return writer.AddOrUpdate(key, newEntity, e =>
          {
            throw new InvalidOperationException(string.Format("Entity '{0}' with key '{1}' should not exist.",
                                                              typeof (TEntity).Name, key));
          }, AddOrUpdateHint.ProbablyDoesNotExist);
    }

     /// <summary>
     /// Updates an entity, creating a new instance before that, if needed.
     /// </summary>
     /// <typeparam name="TKey">The type of the key.</typeparam>
     /// <typeparam name="TView">The type of the view.</typeparam>
     /// <param name="self">The self.</param>
     /// <param name="key">The key.</param>
     /// <param name="update">The update.</param>
     /// <param name="hint">The hint.</param>
     /// <returns></returns>
     public static TView UpdateEnforcingNew<TKey, TView>(this IDocumentWriter<TKey, TView> self, TKey key,
         Action<TView> update, AddOrUpdateHint hint = AddOrUpdateHint.ProbablyExists)
         where TView : new()
     {
       return self.AddOrUpdate(key, () =>
       {
         var view = new TView();
         update(view);
         return view;
       }, v =>
       {
         update(v);
         return v;
       }, hint);
     }

     public static TView UpdateEnforcingNew<TView>(this IDocumentWriter<unit, TView> self, Action<TView> update, AddOrUpdateHint hint = AddOrUpdateHint.ProbablyExists)
         where TView : new()
     {
       return self.UpdateEnforcingNew(unit.it, update, hint);

     }
  }
}