using System;
using System.Collections.Generic;

namespace BigTree.MicroCQRS 
{
  public interface IEventStore
  {
    void SaveEvents(Guid aggregateId, Type aggregateType, IEnumerable<Event> events, int expectedVersion);
    List<Event> GetEventsForAggregate(Guid aggregateId);
    List<Event> GetAllEvents();
    IDictionary<Guid, List<Event>> GetAllEventsByAggregate();
    IList<Event> PeekChanges();
  }
}