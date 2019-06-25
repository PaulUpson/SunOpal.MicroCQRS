namespace BigTree.MicroCQRS
{
  public static class GlobalConfig {
    private static IConventions _conventions;
    public static IConventions Conventions { get {
      return _conventions ?? new Conventions();
    }
      set { _conventions = value; }
    }
  }

  public interface IConventions {
    string ViewsFolder { get; }
    string DocsFolder { get; }
    string StorageConfigName { get; }
  }

  public class Conventions : IConventions {
    private string _prefix = "BigTree";

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