﻿# Static File Generator

This is based on [Incremental generator.](https://github.com/dotnet/roslyn/blob/main/docs/features/incremental-generators.md)

Incremental generators are a new API that exists alongside source generators to allow users to specify generation strategies that can be applied in a high performance way by the hosting layer.

> Note that you will also need to reference the 4.x version of the Microsoft.CodeAnalysis.CSharp.Workspaces NuGet package to access the interface.

High Level Design Goals
* Allow for a finer grained approach to defining a generator
* Scale source generators to support 'Roslyn/CoreCLR' scale projects in Visual Studio
* Exploit caching between fine grained steps to reduce duplicate work
* Support generating more items that just source texts
* Exist alongside ISourceGenerator based implementations

## Some notes

* Working side-by-side is a problem, hence Visual Studio has to be restarted to see changes.

[See ](https://github.com/dotnet/roslyn/issues/48083)

```
The recommended development path from Microsoft seems to be:

Develop source generators via unit test
Ship and consume source generators as NuGet packages.
Don't directly link to a source generator project, even though this is possible.
If the above recommendations are followed, then VS never needs to be restarted.

That being said, I hope that taking a direct dependency on a source generator project is supported in the future! 🙂 For internal projects, having to package the source generator as a NuGet should be unnecessary 🙂
```

```
You are literally hooking the compiler at the deepest level. You get to run prior to us even getting the compilation value that powers literally everything higher up. With that great power comes great responsibility. It's difficult by it's very nature, and will continue to be that way.
```

## Testing

https://github.com/dotnet/roslyn/blob/main/docs/features/source-generators.cookbook.md#unit-testing-of-generators
https://www.meziantou.net/testing-roslyn-incremental-source-generators.htm

Documentation is rough:

[Source code to generators](https://github.com/dotnet/roslyn-sdk/blob/d16bce93b36d078682776b93d5435287b038783f/tests/Microsoft.CodeAnalysis.Testing/Microsoft.CodeAnalysis.SourceGenerators.Testing.UnitTests/SourceGeneratorValidationTests.cs)

# Sources
* [Source generator updates: incremental generators](https://andrewlock.net/exploring-dotnet-6-part-9-source-generator-updates-incremental-generators/)
* [Современные (инкрементные) Source Generators в .NET](https://habr.com/ru/articles/721874/) 
* [Using Source Generators for Fun - by Jason Bock](https://www.youtube.com/watch?v=4DVV7FXukC8&list=PLdo4fOcmZ0oVFtp9MDEBNbA2sSqYvXSXO&index=78&t=71s)
* [Source generators](https://dominikjeske.github.io/source-generators/) 
* [Supporting additional files](https://devblogs.microsoft.com/dotnet/new-c-source-generator-samples/)