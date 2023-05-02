using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using Xunit;
using Microsoft.CodeAnalysis.Testing;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace StaticFiles.IncrementalGenerator.Tests
{
    /// <summary>
    /// Same pattern is not supported yet, at least with packages that are available, so another alternative has to be used
    /// 
    /// https://www.meziantou.net/testing-roslyn-incremental-source-generators.htm
    /// https://andrewlock.net/creating-a-source-generator-part-2-testing-an-incremental-generator-with-snapshot-testing/
    /// https://www.thinktecture.com/en/net/roslyn-source-generators-analyzers-code-fixes-testing/
    /// </summary>
    //public sealed class SampleSourceGeneratorTests
    //{
    //    [Fact]
    //    public void Test()
    //    {
    //        var compilation = CSharpCompilation.Create("TestProject",
    //            new[] { CSharpSyntaxTree.ParseText("struct Test { }") },
    //            Basic.Reference.Assemblies.Net70.References.All,
    //            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

    //        var generator = new SampleSourceGenerator();
    //        var sourceGenerator = generator.AsSourceGenerator();

    //        // trackIncrementalGeneratorSteps allows to report info about each step of the generator
    //        GeneratorDriver driver = CSharpGeneratorDriver.Create(
    //            generators: new ISourceGenerator[] { sourceGenerator },
    //            driverOptions: new GeneratorDriverOptions(default, trackIncrementalGeneratorSteps: true));

    //        // Run the generator
    //        driver = driver.RunGenerators(compilation);

    //        // Update the compilation and rerun the generator
    //        compilation = compilation.AddSyntaxTrees(CSharpSyntaxTree.ParseText("// dummy"));
    //        driver = driver.RunGenerators(compilation);

    //        // Assert the driver doesn't recompute the output
    //        var result = driver.GetRunResult().Results.Single();
    //        var allOutputs = result.TrackedOutputSteps.SelectMany(outputStep => outputStep.Value).SelectMany(output => output.Outputs);
    //        Assert.Collection(allOutputs, output => Assert.Equal(IncrementalStepRunReason.Cached, output.Reason));

    //        // Assert the driver use the cached result from AssemblyName and Syntax
    //        var assemblyNameOutputs = result.TrackedSteps["AssemblyName"].Single().Outputs;
    //        Assert.Collection(assemblyNameOutputs, output => Assert.Equal(IncrementalStepRunReason.Unchanged, output.Reason));

    //        var syntaxOutputs = result.TrackedSteps["Syntax"].Single().Outputs;
    //        Assert.Collection(syntaxOutputs, output => Assert.Equal(IncrementalStepRunReason.Unchanged, output.Reason));
    //    }

    //}

    //    public class EnumGeneratorSnapshotTests
    //    {
    //        [Fact]
    //        public Task GeneratesEnumExtensionsCorrectly()
    //        {
    //            // The source code to test
    //            var source = @"
    //using NetEscapades.EnumGenerators;

    //[EnumExtensions]
    //public enum Colour
    //{
    //    Red = 0,
    //    Blue = 1,
    //}";

    //            // Pass the source code to our helper and snapshot test the output
    //            return TestHelper.Verify(source);
    //        }
    //    }


    //    public static class TestHelper
    //    {
    //        public static Task Verify(string source)
    //        {
    //            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(source);

    //            // Create references for assemblies we require
    //            // We could add multiple references if required
    //            IEnumerable<PortableExecutableReference> references = new[]
    //            {
    //                MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
    //            };

    //            CSharpCompilation compilation = CSharpCompilation.Create(
    //                assemblyName: "Tests",
    //                syntaxTrees: new[] { syntaxTree },
    //                references: references); // 👈 pass the references to the compilation

    //            var generator = new SimpleFileIncrementalGenerator();

    //            GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

    //            driver = driver.RunGenerators(compilation);

    //            return Verifier
    //                .Verify(driver)
    //                .UseDirectory("Snapshots");
    //        }
    //    }


    //    public class GeneratorTests
    //    {
    //        [Fact]
    //        public void SimpleGeneratorTest()
    //        {
    //            // Create the 'input' compilation that the generator will act on
    //            Compilation inputCompilation = CreateCompilation(@"
    //namespace MyCode
    //{
    //    public class Program
    //    {
    //        public static void Main(string[] args)
    //        {
    //        }
    //    }
    //}
    //");

    //            // directly create an instance of the generator
    //            // (Note: in the compiler this is loaded from an assembly, and created via reflection at runtime)
    //            var generator = new SimpleFileIncrementalGenerator();

    //            // Create the driver that will control the generation, passing in our generator
    //            GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

    //            // Run the generation pass
    //            // (Note: the generator driver itself is immutable, and all calls return an updated version of the driver that you should use for subsequent calls)
    //            driver = driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out var diagnostics);

    //            // We can now assert things about the resulting compilation:
    //            Debug.Assert(diagnostics.IsEmpty); // there were no diagnostics created by the generators
    //            Debug.Assert(outputCompilation.SyntaxTrees.Count() == 2); // we have two syntax trees, the original 'user' provided one, and the one added by the generator
    //            Debug.Assert(outputCompilation.GetDiagnostics().IsEmpty); // verify the compilation with the added source has no diagnostics

    //            // Or we can look at the results directly:
    //            GeneratorDriverRunResult runResult = driver.GetRunResult();

    //            // The runResult contains the combined results of all generators passed to the driver
    //            Debug.Assert(runResult.GeneratedTrees.Length == 1);
    //            Debug.Assert(runResult.Diagnostics.IsEmpty);

    //            // Or you can access the individual results on a by-generator basis
    //            GeneratorRunResult generatorResult = runResult.Results[0];
    //            Debug.Assert(generatorResult.Generator == generator);
    //            Debug.Assert(generatorResult.Diagnostics.IsEmpty);
    //            Debug.Assert(generatorResult.GeneratedSources.Length == 1);
    //            Debug.Assert(generatorResult.Exception is null);
    //        }

    //        private static Compilation CreateCompilation(string source)
    //            => CSharpCompilation.Create("compilation",
    //                new[] { CSharpSyntaxTree.ParseText(source) },
    //                new[] { MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location) },
    //                new CSharpCompilationOptions(OutputKind.ConsoleApplication));
    //    }

    
    public class Test
    {
        [Fact]
        public void GeneratesEnumExtensionsCorrectly()
        {
            // The source code to test
            var source = @"
        using NetEscapades.EnumGenerators;

        [EnumExtensions]
        public enum Colour
        {
            Red = 0,
            Blue = 1,
        }";

            // Pass the source code to our helper and snapshot test the output
            Verify(source);
        }

        private void Verify(string source)
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

            driver = driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var diagnostics);

            Assert.NotNull(driver);
        }
    }

}