namespace SunOpal.DocumentStore;

  public static class GlobalConfig {
    private static IConventions _conventions;
    public static IConventions Conventions { 
      get { return _conventions ?? new Conventions(); }
      set { _conventions = value; }
    }
  }