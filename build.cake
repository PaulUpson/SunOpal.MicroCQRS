#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0
#tool nuget:?package=JetBrains.DotCover.CommandLineTools&version=2018.1.2
#addin nuget:?package=Cake.Incubator&version=4.0.2
#load build/paths.cake
#load build/utils.cake
//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var codeCoverageReportPath = Argument<FilePath>("CodeCoverageReportPath", "coverage.zip");
var packageOutputPath = Argument<DirectoryPath>("PackageOutputPath", "package");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

var testAssemblyPattern = $"**/bin/{configuration}/*.Tests.dll";
// Define directories.
var buildDir = Paths.BinDirectory + Directory(configuration);

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
  .Does(() =>
{
  CleanDirectory(buildDir);
  CleanDirectory(packageOutputPath);
});

Task("Restore-NuGet")  
  .Does(() =>
{
  NuGetRestore(Paths.SolutionFile);
});

Task("Build-Src")  
  .Does(() =>
{
  MSBuild(
    Paths.SolutionFile, 
    settings => settings.SetConfiguration(configuration)
                        .WithTarget("Build")); // just in case the default target has been changed for the project.
});

Task("Test-NUnit")
  .WithCriteria(() => BuildSystem.IsLocalBuild)
  .Does(() =>
{
  var nunitSettings = new NUnit3Settings {
    NoResults = true, // governs whether TestResult.xml is generated
    Where = "cat != IntegrationTest",
  };

  var testAssemblies = GetFiles(testAssemblyPattern);
  NUnit3(testAssemblies, nunitSettings);
});

Task("Test-DotCover")
  .WithCriteria(() => BuildSystem.IsRunningOnTeamCity)
  .Does(() =>
{
  var nunitSettings = new NUnit3Settings {
    Where = "cat != IntegrationTest",
    TeamCity = true,
    Results = new[] { new NUnit3Result { FileName = Paths.TestResultFile } },
  };
  var testAssemblies = GetFiles(testAssemblyPattern);
  DotCoverCover(
    tool => tool.NUnit3(testAssemblies, nunitSettings),
    Paths.CodeCoverageResultFile,
    new DotCoverCoverSettings()
      .WithFilter($"+:{Paths.LibName}*")
      .WithFilter("-:*.*Tests")
  );

  BuildSystem.TeamCity.ImportData("nunit", Paths.TestResultFile);

  BuildSystem.TeamCity.ImportDotCoverCoverage(
    MakeAbsolute(Paths.CodeCoverageResultFile));
});

///////////////////////////////////////////////////////////////////
// N.B. Remember to modify the <version> value in the project file
// before committing to ensure the package is published successfully
///////////////////////////////////////////////////////////////////

Task("Package-GetVersion")
  .Does(() =>
{
  // Having to use a ParseProject alias extension from the Cake.Incubator addin here 
  // until they roll the .NetCore2 support into Cake.Common in a future Cake release
  var projData = ParseProject(new FilePath(Paths.ProjectFile), "Release");
  Information("Version is {0}", projData.NetCore.Version);

  if (BuildSystem.IsRunningOnTeamCity) {
    BuildSystem.TeamCity.SetBuildNumber(projData.NetCore.Version);
  }
});

Task("Package-NuGet")  
  .Does(() =>
{
  EnsureDirectoryExists(packageOutputPath);
  // for .NET standard we use dotnetcorepack rather than nugetpack
  DotNetCorePack(
    Paths.ProjectFile,
    new DotNetCorePackSettings
    {
      Configuration = configuration,
      OutputDirectory = packageOutputPath
    });
});

Task("Publish-NuGet")  
  .Does(() =>
{
  var package = GetFiles($"./{packageOutputPath}/*.nupkg").First();
  var nugetSource = EnvironmentVariable("INVESTIGATE_NUGET_SOURCE") ?? Paths.LocalRepository.ToString();
  var nugetApiKey = EnvironmentVariable("INVESTIGATE_NUGET_API_KEY");

  Information($"Publishing to {nugetSource}.");

  var pushSettings = new NuGetPushSettings
  {
    Source = nugetSource,
    ApiKey = nugetApiKey
  };
    
  NuGetPush(package, pushSettings);  
})
  .ReportError(exception =>
{  
  // Report the error.
  if (BuildSystem.IsRunningOnTeamCity) {
    BuildSystem.TeamCity.BuildProblem("Failed to add package to Nuget Repo. Have you updated the version?", "Publish-NuGet");
  }
});
//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Restore")
  .IsDependentOn("Clean")
  .IsDependentOn("Restore-NuGet");

Task("Build")
  .IsDependentOn("Restore")
  .IsDependentOn("Build-Src");

Task("Test")
  .IsDependentOn("Build")
  .IsDependentOn("Test-NUnit")
  .IsDependentOn("Test-DotCover");

Task("Package")
  .IsDependentOn("Test")
  .IsDependentOn("Package-GetVersion")
  .IsDependentOn("Package-NuGet");

Task("Publish")
  .IsDependentOn("Package")
  .IsDependentOn("Publish-NuGet");
Task("Default")
  .IsDependentOn("Package");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);