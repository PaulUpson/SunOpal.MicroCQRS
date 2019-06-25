namespace BigTree.MicroCQRS.DocumentStore
{
  public interface IDocumentReader<in TKey,TView>
  {
    bool TryGet(TKey key, out TView view);
  }
}