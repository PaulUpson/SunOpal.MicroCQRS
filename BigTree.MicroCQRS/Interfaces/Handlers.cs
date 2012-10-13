namespace BigTree.MicroCQRS 
{
  public interface ICommandSender 
  {
    void Send<T>(T command) where T : Command;
  }
  
  public interface ICommandHandler<in T> where T : Command
  {
    void Handle(T cmd);
  }

  public interface IEventPublisher 
  {
    void Publish<T>(T @event) where T : Event;
    void PublishSync<T>(T @event) where T : Event;
  }

  public interface Handles<in T> where T : Event
  {
    void Handle(T e);
  }
}