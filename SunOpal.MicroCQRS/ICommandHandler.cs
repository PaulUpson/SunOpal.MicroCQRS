namespace SunOpal.MicroCQRS
{
  public interface ICommandHandler<in T> where T : Command
  {
    void Handle(T cmd);
  }
}