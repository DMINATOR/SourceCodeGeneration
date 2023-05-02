using Microsoft.CodeAnalysis.Text;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using VerifyTestFiles = CSharpSourceGeneratorVerifier<StaticFiles.SourceGenerator.SimpleFileSourceGenerator>;

namespace StaticFiles.Tests
{
    public class SimpleFileSourceGeneratorTests
    {
        [Fact]
        public async Task AddSimpleFile2()
        {
            var code = "// No code as input";

            await new VerifyTestFiles.Test
            {
                TestState =
                {
                    Sources = { code },
                    GeneratedSources =
                    {
                        (typeof(StaticFiles.SourceGenerator.SimpleFileSourceGenerator), "TestFile1.g.cs", SourceText.From("// Test File content 1", Encoding.UTF8, SourceHashAlgorithm.Sha1)),
                        (typeof(StaticFiles.SourceGenerator.SimpleFileSourceGenerator), "TestFile2.g.cs", SourceText.From("// Test File content 2", Encoding.UTF8, SourceHashAlgorithm.Sha1)),
                    },
                },
            }.RunAsync();
        }
    }
}
