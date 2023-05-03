using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Advanced.IncrementalGenerator;

namespace StaticFiles.IncrementalGenerator.Tests
{
    public static class CSharpIncrementalGeneratorVerifier
    {
        public static GeneratorDriverRunResult Verify(string source)
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

            var generator = new SimpleFileIncrementalGenerator();

            GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

            // Or we can look at the results directly:
            driver = driver.RunGenerators(compilation);
            GeneratorDriverRunResult runResult = driver.GetRunResult();

            return runResult;
        }
    }
}
