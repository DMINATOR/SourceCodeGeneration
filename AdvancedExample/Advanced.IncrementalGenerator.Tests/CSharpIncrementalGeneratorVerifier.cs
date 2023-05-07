﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;

namespace StaticFiles.IncrementalGenerator.Tests
{
    public static class CSharpIncrementalGeneratorVerifier<TIncrementalGenerator> where TIncrementalGenerator : IIncrementalGenerator
    {
        public static GeneratorDriverRunResult Verify(string source, TIncrementalGenerator generator)
        {
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(source);

            // Create references for assemblies we require
            // We could add multiple references if required
            IEnumerable<PortableExecutableReference> references = new[]
            {
                 MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
            };

            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName: "Tests",
                syntaxTrees: new[] { syntaxTree },
                references: references); // 👈 pass the references to the compilation

            GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

            // Or we can look at the results directly:
            driver = driver.RunGenerators(compilation);
            GeneratorDriverRunResult runResult = driver.GetRunResult();

            return runResult;
        }
    }
}
