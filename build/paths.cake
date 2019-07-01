public static class Paths
{
  public static string LibName => "SunOpal.MicroCQRS";
  public static FilePath SolutionFile => $"./{LibName}.sln";
  public static string ProjectFile => $"./{LibName}/{LibName}.csproj";
  public static DirectoryPath BinDirectory => $"./{LibName}/bin";
  public static FilePath CodeCoverageResultFile => "coverage.xml";
  public static FilePath TestResultFile => "TestResult.xml";
  public static DirectoryPath LocalRepository => "C:/LocalPackages";
}

public static FilePath Combine(DirectoryPath directory, FilePath file)
{
  return directory.CombineWithFilePath(file);
}