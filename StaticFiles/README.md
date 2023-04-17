# Static File Generator

This is based on [Incremental generator.](https://github.com/dotnet/roslyn/blob/main/docs/features/incremental-generators.md)

Incremental generators are a new API that exists alongside source generators to allow users to specify generation strategies that can be applied in a high performance way by the hosting layer.

> Note that you will also need to reference the 4.x version of the Microsoft.CodeAnalysis.CSharp.Workspaces NuGet package to access the interface.

High Level Design Goals
* Allow for a finer grained approach to defining a generator
* Scale source generators to support 'Roslyn/CoreCLR' scale projects in Visual Studio
* Exploit caching between fine grained steps to reduce duplicate work
* Support generating more items that just source texts
* Exist alongside ISourceGenerator based implementations

# Sources
* [Source generator updates: incremental generators](https://andrewlock.net/exploring-dotnet-6-part-9-source-generator-updates-incremental-generators/)