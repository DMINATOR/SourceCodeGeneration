using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;
using System.Linq;

namespace Advanced.IncrementalGenerator.Tests
{
    public static class CSharpIncrementalGeneratorVerifier<TIncrementalGenerator> where TIncrementalGenerator : IIncrementalGenerator
    {
        public static GeneratorDriverRunResult Verify(string source, TIncrementalGenerator generator, IEnumerable<AdditionalText> additionalTexts = null)
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

            // The only method that supports passing additional texts
            GeneratorDriver driver = CSharpGeneratorDriver.Create((new IIncrementalGenerator[]{generator})
                .Select(GeneratorExtensions.AsSourceGenerator), additionalTexts: additionalTexts);

            // Or we can look at the results directly:
            driver = driver.RunGenerators(compilation);
            GeneratorDriverRunResult runResult = driver.GetRunResult();

            return runResult;
        }
    }
}
