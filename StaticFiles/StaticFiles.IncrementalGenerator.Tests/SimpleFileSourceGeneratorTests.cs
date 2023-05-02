using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StaticFiles.IncrementalGenerator.Tests
{
    public class SimpleFileSourceGeneratorTests
    {
        [Fact]
        public async Task AddSimpleFile2()
        {
            var code = "// No code as input";

            var result = CSharpIncrementalGeneratorVerifier.Verify(code);

            Assert.True(result.Diagnostics.IsEmpty);
            Assert.Single(result.Results);

            var resultRun = result.Results[0];
            Assert.Equal(2, resultRun.GeneratedSources.Length);

            Assert.True(SourceText.From("// Test File content 1", Encoding.UTF8, SourceHashAlgorithm.Sha1).ContentEquals(resultRun.GeneratedSources[0].SourceText));
            Assert.Equal("TestFile1.g.cs", resultRun.GeneratedSources[0].HintName);

            Assert.True(SourceText.From("// Test File content 2", Encoding.UTF8, SourceHashAlgorithm.Sha1).ContentEquals(resultRun.GeneratedSources[1].SourceText));
            Assert.Equal("TestFile2.g.cs", resultRun.GeneratedSources[1].HintName);

            //            GeneratorRunResult generatorResult = runResult.Results[0];
            //            Debug.Assert(generatorResult.Generator == generator);
            //            Debug.Assert(generatorResult.Diagnostics.IsEmpty);
            //            Debug.Assert(generatorResult.GeneratedSources.Length == 1);
            //            Debug.Assert(generatorResult.Exception is null);

            //Assert.Equal(2, result.GeneratedTrees.Length);
            //Assert.True(SourceText.From("// Test File content 1", Encoding.UTF8, SourceHashAlgorithm.Sha1).ContentEquals(result.GeneratedTrees[0].GetText()));
            //Assert.True(SourceText.From("// Test File content 2", Encoding.UTF8, SourceHashAlgorithm.Sha1).ContentEquals(result.GeneratedTrees[1].GetText()));

            //await new VerifyTestFiles.Test
            //{
            //    TestState =
            //    {
            //        Sources = { code },
            //        GeneratedSources =
            //        {
            //            (typeof(StaticFiles.SourceGenerator.SimpleFileSourceGenerator), "TestFile1.g.cs", SourceText.From("// Test File content 1", Encoding.UTF8, SourceHashAlgorithm.Sha1)),
            //            (typeof(StaticFiles.SourceGenerator.SimpleFileSourceGenerator), "TestFile2.g.cs", SourceText.From("// Test File content 2", Encoding.UTF8, SourceHashAlgorithm.Sha1)),
            //        },
            //    },
            //}.RunAsync();
        }
    }
}
