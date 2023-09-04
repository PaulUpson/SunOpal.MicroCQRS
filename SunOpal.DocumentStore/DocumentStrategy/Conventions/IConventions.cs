namespace SunOpal.DocumentStore;

/// <summary>
/// Define the naming conventions
/// </summary>
public interface IConventions {
  string ViewsFolder { get; }
  string DocsFolder { get; }
  string StorageConfigName { get; }
}