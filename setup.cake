#load nuget:https://www.myget.org/F/cake-contrib/api/v2?package=Cake.Recipe&prerelease

Environment.SetVariableNames();

BuildParameters.SetParameters(
    context: Context, 
    buildSystem: BuildSystem,
    sourceDirectoryPath: "./src",
    title: "Cake.Prca",
    repositoryOwner: "cake-contrib",
    repositoryName: "Cake.Prca",
    appVeyorAccountName: "cakecontrib");

BuildParameters.PrintParameters(Context);

ToolSettings.SetToolSettings(
    context: Context,
    dupFinderExcludePattern: new string[] 
	{ 
		BuildParameters.RootDirectoryPath + "/src/Cake.Prca.Tests/*.cs", 
		BuildParameters.RootDirectoryPath + "/src/Cake.Prca.Issues.MsBuild.Tests/*.cs", 
		BuildParameters.RootDirectoryPath + "/src/Cake.Prca.PullRequests.Tfs.Tests/*.cs" 
	},
    testCoverageFilter: "+[*]* -[xunit.*]* -[Cake.Core]* -[Cake.Testing]* -[*.Tests]* ",
    testCoverageExcludeByAttribute: "*.ExcludeFromCodeCoverage*",
    testCoverageExcludeByFile: "*/*Designer.cs;*/*.g.cs;*/*.g.i.cs");

Build.Run();
