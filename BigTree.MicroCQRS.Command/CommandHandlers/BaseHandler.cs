using System;
using System.Collections.Generic;
using System.Reflection;

namespace BigTree.MicroCQRS.CommandHandlers {
  public class BaseHandler<TEntity> where TEntity : AggregateRoot, new() {
    protected readonly IRepository<TEntity> Repository;
    private readonly IDictionary<Type, MethodInfo> finders = new Dictionary<Type, MethodInfo>();

    public BaseHandler(IRepository<TEntity> repository) {
      Repository = repository;
    }

    protected void HandleUpdate(Command command, Action<TEntity> action) {
      if (!finders.TryGetValue(command.GetType(), out MethodInfo info))
      {
        throw new InvalidOperationException(string.Format("No id mapping configured for {0}. Have your remembered to set a ConfigureMapping<{0}>() definition?", command.GetType().Name));
      }

      var id = (Guid)info.Invoke(null, new[] {(object) command});
      var item = Repository.GetById(id);
      action(item);
      Repository.Save(item, command.OriginalVersion);
    }

    protected void HandleUpdate(Guid id, Action<TEntity> action, int version) {
      var item = Repository.GetById(id);
      action(item);
      Repository.Save(item, version);
    }

    protected void HandleNew(Action<TEntity> action) {
      var item = new TEntity();
      action(item);
      Repository.Save(item, -1);
    }

    protected void ConfigureMapping<TCommand>(Func<TCommand, Guid> getId) {
      finders[typeof(TCommand)] = getId.Method;
    }
  }
}