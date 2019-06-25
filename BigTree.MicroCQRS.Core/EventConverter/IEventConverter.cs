namespace BigTree.MicroCQRS
{
  public interface IEventConverter {
    Event Convert(Event sourceEvent);
  }
}