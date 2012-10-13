namespace BigTree.MicroCQRS.Sagas {
  public interface HasCompleted {
    bool Completed { get; }
  }
}