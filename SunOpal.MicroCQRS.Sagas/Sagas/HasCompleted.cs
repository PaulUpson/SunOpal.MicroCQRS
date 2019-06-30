namespace SunOpal.MicroCQRS.Sagas {
  public interface HasCompleted {
    bool Completed { get; }
  }
}