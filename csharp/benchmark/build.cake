#tool nuget:?package=NUnit.ConsoleRunner&version=3.7.0

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    DotNetCoreClean("./Benchmarks.sln");
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetCoreRestore("./Benchmarks.sln");
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    DotNetCoreBuild("./Benchmarks.sln");
});

Task("RunNet46")
    .IsDependentOn("Build")
    .Does(() =>
{
    DotNetCoreRun("./src/Benchmarks.csproj", ""/*List of classes for benchmarking or empty for all classes processing*/, new DotNetCoreRunSettings
    {
        Framework = "net46",
        Configuration = configuration
    });
});

Task("RunNetCoreApp20")
    .IsDependentOn("Build")
    .Does(() =>
{
    DotNetCoreRun("./src/Benchmarks.csproj", ""/*List of classes for benchmarking or empty for all classes processing*/, new DotNetCoreRunSettings
    {
        Framework = "netcoreapp2.0",
        Configuration = configuration
    });
});

Task("Run")
    .IsDependentOn("RunNet46")
    .IsDependentOn("RunNetCoreApp20")
    .Does(() =>
{
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Run");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
