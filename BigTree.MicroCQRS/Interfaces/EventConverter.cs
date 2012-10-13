namespace BigTree.MicroCQRS
{
  public interface IEventConversion<in TSourceEvent, out TTargetEvent>
    where TSourceEvent : Event
    where TTargetEvent : Event
  {
    TTargetEvent Convert(TSourceEvent sourceEvent);
  }

  public interface IEventConverter {
    Event Convert(Event sourceEvent);
  }
}