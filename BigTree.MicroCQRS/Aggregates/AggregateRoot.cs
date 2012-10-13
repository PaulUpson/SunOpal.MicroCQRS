using System;
using System.Collections.Generic;

namespace BigTree.MicroCQRS {
  public abstract class AggregateRoot
  {
    private readonly List<Event> _changes = new List<Event>();

    private DomainValidationException _domainValidationException = new DomainValidationException();

    public abstract Guid Id { get; }
    public int Version { get; internal set; }

    protected void InitialiseValidation()
    {
      _domainValidationException = new DomainValidationException();
    }

    protected void RaiseError(string paramName, string message)
    {
      _domainValidationException.AddError(paramName, message);
    }

    public IEnumerable<Event> GetUncommittedChanges()
    {
      return _changes;
    }

    public void MarkChangesAsCommitted()
    {
      _changes.Clear();
    }

    public void LoadFromHistory(IEnumerable<Event> history)
    {
      foreach (var e in history) ApplyChange(e, false);
    }

    protected void ApplyChange(Event @event)
    {
      if (_domainValidationException.HasErrors)
        throw _domainValidationException;
      ApplyChange(@event, true);
    }

    private void ApplyChange(Event @event, bool isNew)
    {
      this.AsDynamic().Apply(@event);
      if (isNew) _changes.Add(@event);
    }
  }
}