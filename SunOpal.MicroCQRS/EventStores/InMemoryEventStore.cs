using System;
using System.Collections.Generic;
using System.Linq;

namespace SunOpal.MicroCQRS {
  /// <summary>
  /// In memory example of writing and reading from the event store
  /// </summary>
  public class InMemoryEventStore : IEventStore
  {
    private readonly IEventPublisher _publisher;
    private readonly IEventConverter _converter;

    private readonly Dictionary<Guid, List<EventDescriptor>> _current = new Dictionary<Guid, List<EventDescriptor>>();

    private struct EventDescriptor
    {
      public readonly Event EventData;
      public readonly Guid Id;
      public readonly int Version;

      public EventDescriptor(Guid id, Event eventData, int version)
      {
        EventData = eventData;
        Version = version;
        Id = id;
      }
    }

    public InMemoryEventStore(IEventPublisher publisher, IEventConverter converter)
    {
      _publisher = publisher;
      _converter = converter;
    }

    public void SaveEvents(Guid aggregateId, Type aggregateType, IEnumerable<Event> events, int expectedVersion)
    {
      List<EventDescriptor> eventDescriptors;
      if (!_current.TryGetValue(aggregateId, out eventDescriptors))
      {
        eventDescriptors = new List<EventDescriptor>();
        _current.Add(aggregateId, eventDescriptors);
      }
      else if (eventDescriptors[eventDescriptors.Count - 1].Version != expectedVersion && expectedVersion != -1)
      {
        throw new ConcurrencyException();
      }
      var i = expectedVersion;
      foreach (var @event in events)
      {
        i++;
        @event.Version = i;
        eventDescriptors.Add(new EventDescriptor(aggregateId, @event, i));
        _publisher.Publish(@event);
      }
    }

    public List<Event> GetEventsForAggregate(Guid aggregateId)
    {
      List<EventDescriptor> eventDescriptors;
      if (!_current.TryGetValue(aggregateId, out eventDescriptors))
      {
        throw new AggregateNotFoundException();
      }
      return ConvertEventDescriptors(eventDescriptors);
    }

    public List<Event> GetAllEvents()
    {
      var eventDescriptors = new List<EventDescriptor>();
      foreach (var aggregate in _current)
      {
        eventDescriptors.AddRange(aggregate.Value);
      }
      return ConvertEventDescriptors(eventDescriptors);
    }

    public IDictionary<Guid, List<Event>> GetAllEventsByAggregate() {
      return _current.ToDictionary(item => item.Key, item => ConvertEventDescriptors(item.Value));
    }

    public IList<Event> PeekChanges()
    {
      throw new NotImplementedException();
    }

    private List<Event> ConvertEventDescriptors(IEnumerable<EventDescriptor> descriptors) {
      return descriptors.Select(x => x.EventData).Select(_converter.Convert).ToList();
    } 
  }
}