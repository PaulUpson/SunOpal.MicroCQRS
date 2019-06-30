namespace SunOpal.MicroCQRS
{
  public interface IEventConverter {
    Event Convert(Event sourceEvent);
  }
}