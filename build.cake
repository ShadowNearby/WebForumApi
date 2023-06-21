#addin nuget:?package=Cake.Coverlet

var target = Argument("target", "Test");
var configuration = Argument("configuration", "Debug");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////


Task("Build")
    .Does(() =>
{
    DotNetBuild("WebApiWebForumApi.sln", new DotNetBuildSettings
    {
        Configuration = configuration,
    });
});

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
{
	var coverletSettings = new CoverletSettings {
        CollectCoverage = true,
        CoverletOutputFormat = CoverletOutputFormat.opencover | CoverletOutputFormat.json,
        MergeWithFile = MakeAbsolute(new DirectoryPath("./coverage.json")).FullPath,
        CoverletOutputDirectory = MakeAbsolute(new DirectoryPath(@"./coverage")).FullPath
    };
	
	Coverlet(
        "./tests/WebForumApi.Api.IntegrationTests/bin/Debug/net7.0/WebForumApi.Api.IntegrationTests.dll", 
        "./tests/WebForumApi.Api.IntegrationTests/WebForumApi.Api.IntegrationTests.csproj", 
        coverletSettings);
		
	Coverlet(
        "./tests/WebForumApi.Api.UnitTests/bin/Debug/net7.0/WebForumApi.Api.UnitTests.dll", 
        "./tests/WebForumApi.Api.UnitTests/WebForumApi.Api.UnitTests.csproj", 
        coverletSettings);
	
});

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);