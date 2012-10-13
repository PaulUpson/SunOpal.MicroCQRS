namespace BigTree.MicroCQRS
{
  public static class Conventions
  {
    public const string Prefix = "BigTree";

    public static readonly string StorageConfigName = Prefix + "-storage";

    public static readonly string ViewsFolder = Prefix + "-view";
    public static readonly string DocsFolder = Prefix + "-doc";
  }
}