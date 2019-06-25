namespace BigTree.MicroCQRS
{

  public interface Handles<in T> where T : Event
  {
    void Handle(T e);
  }
}