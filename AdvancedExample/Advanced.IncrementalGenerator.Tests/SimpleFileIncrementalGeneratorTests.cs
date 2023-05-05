using Advanced.IncrementalGenerator;
using Microsoft.CodeAnalysis.Text;
using System.Text;
using Xunit;

namespace StaticFiles.IncrementalGenerator.Tests
{
    public class SimpleFileIncrementalGeneratorTests
    {
        [Fact]
        public void AddSimpleFile2()
        {
            var code = "// No code as input";

            var result = CSharpIncrementalGeneratorVerifier<SimpleFileIncrementalGenerator>.Verify(code, new SimpleFileIncrementalGenerator());

            Assert.True(result.Diagnostics.IsEmpty);
            Assert.Single(result.Results);

            var resultRun = result.Results[0];
            Assert.Equal(2, resultRun.GeneratedSources.Length);

            Assert.True(SourceText.From("// Test File content 1", Encoding.UTF8, SourceHashAlgorithm.Sha1).ContentEquals(resultRun.GeneratedSources[0].SourceText));
            Assert.Equal("TestFile1.g.cs", resultRun.GeneratedSources[0].HintName);

            Assert.True(SourceText.From("// Test File content 2", Encoding.UTF8, SourceHashAlgorithm.Sha1).ContentEquals(resultRun.GeneratedSources[1].SourceText));
            Assert.Equal("TestFile2.g.cs", resultRun.GeneratedSources[1].HintName);
        }
    }
}
