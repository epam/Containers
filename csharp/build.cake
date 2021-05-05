#tool nuget:?package=NUnit.ConsoleRunner&version=3.7.0

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var buildDir = Directory("./src/Containers/bin") + Directory(configuration);

var gradleProperties = new Dictionary<String, String>();
foreach (var row in System.IO.File.ReadAllLines("../gradle.properties"))
    gradleProperties.Add(row.Split('=')[0], String.Join("=",row.Split('=').Skip(1).ToArray()));
var version = gradleProperties["version"];
var index = version.IndexOf("-");
var dotNetVersion = (index > 0 ? version.Substring(0, index) : version) + ".0";

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory(buildDir);
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetCoreRestore("./Containers.sln");
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    DotNetCoreBuild("./Containers.sln", new DotNetCoreBuildSettings {
        Configuration = configuration,
        MSBuildSettings = new DotNetCoreMSBuildSettings()
            .WithProperty("Version", version)
            .WithProperty("FileVersion", dotNetVersion)
            .WithProperty("AssemblyVersion", dotNetVersion)
    });
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    var settings = new DotNetCoreTestSettings
    {
         Configuration = configuration,
         Framework = "netcoreapp2.0"
    };

    Information("Running tests with .NET Core 2.0");
    DotNetCoreTest("./tests/Containers.Tests/Deltix.Containers.Tests.csproj", settings);

    Information("Running tests with .NET Framework 4.5");
    NUnit3("./tests/Containers.Tests/bin/Release/net45/*.Tests.dll");
});

Task("Pack")
    .IsDependentOn("Build")
    .Does(() =>
{
    var settings = new DotNetCorePackSettings
    {
        Configuration = configuration,
        OutputDirectory = "./artifacts/",
        MSBuildSettings = new DotNetCoreMSBuildSettings()
            .WithProperty("Version", version)
            .WithProperty("FileVersion", dotNetVersion)
            .WithProperty("AssemblyVersion", dotNetVersion)
    };
    DotNetCorePack(".", settings);
});

Task("Push")
    .IsDependentOn("Pack")
    .Does(() =>
{
    var url = "https://packages.deltixhub.com/nuget/" + (EnvironmentVariable("FEED_BASE_NAME") ?? "Test") + ".NET";
    var apiKey = (EnvironmentVariable("PUBLISHER_USERNAME") ?? "") + ":" + (EnvironmentVariable("PUBLISHER_PASSWORD") ?? "");
    foreach (var file in GetFiles("./artifacts/*.nupkg"))
    {
        DotNetCoreTool(".", "nuget", "push " + file.FullPath + " --source " + url + " --api-key " + apiKey);
    }
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Run-Unit-Tests");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
