# SourceCodeGeneration

## Presentation

[Source Generators](https://1drv.ms/p/s!AkzSU-9_DOfS2SU-RPNIUBxygjds?e=mH6XDO)

## Debugging Source Generators

[Reference:](https://github.com/JoanComasFdz/dotnet-how-to-debug-source-generator-vs2022)

[Install The .NET Compiler Platform SDK](https://learn.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/)

## Debug Configuration
1. Right click on the SourceGenerator project
2. Click `Properties`.
2. Click `Debug`.
3. Click `Open debug launch profiles UI`.
4. Click on `Delete` to delete the profile shown.
5. Click on `Add`
6. Select `Roslyn component`.
7. In `Target project` select the Console Application project.
8. Close the UI.
9. Restart Visual Studio 2022.
10. In the debug profiles dropdown next to the `Play` button, select your SourceGenerator project.
11. Put a break point in your SourceGenerator to make sure the debugger stops.
12. Click `Play`.

## Notes
Every time you change your source generator code, you will need to restart Visual Studio, otherwise Rebuilding the target project will not use the new version. This has something to do with Visual Studio caching.

[Extension to the help](https://marketplace.visualstudio.com/items?itemName=AlexanderGayko.AutoUpdateAssemblyName&ssr=false#overview)

# References

* [Source Generators](https://learn.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/source-generators-overview)
* [Getting started with Roslyn Analyzers](https://denace.dev/getting-started-with-roslyn-analyzers)
* [Examples](https://github.com/dotnet/roslyn-sdk/tree/main/samples/CSharp/SourceGenerators)