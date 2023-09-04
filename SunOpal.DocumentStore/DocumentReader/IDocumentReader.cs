namespace SunOpal.DocumentStore;

public interface IDocumentReader<in TKey, TView>
{
  bool TryGet(TKey key, out TView view);
}