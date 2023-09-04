namespace SunOpal.DocumentStore
{
  /// <summary>
  /// Implementation of naming conventions
  /// </summary>
  public class Conventions : IConventions {
    private string _prefix = "SunOpal";

    public string Prefix {
      set { _prefix = value; }
    }

    public string ViewsFolder {
      get { return _prefix + "-view"; }
    }

    public string DocsFolder {
      get { return _prefix + "-doc"; }
    }

    public string StorageConfigName {
      get { return _prefix + "-storage"; }
    }
  }
}