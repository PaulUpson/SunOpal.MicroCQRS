namespace SunOpal.MicroCQRS
{
  public interface IEventPublisher 
  {
    void Publish<T>(T @event) where T : Event;
    void PublishSync<T>(T @event) where T : Event;
  }
}