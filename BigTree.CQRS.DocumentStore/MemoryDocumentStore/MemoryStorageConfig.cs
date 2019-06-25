using System.Collections.Concurrent;

namespace BigTree.MicroCQRS.DocumentStore
{
  public sealed class MemoryStorageConfig
  {
     public readonly ConcurrentDictionary<string, ConcurrentDictionary<string, byte[]>> Data = 
     new ConcurrentDictionary<string, ConcurrentDictionary<string, byte[]>>(); 
  }
}