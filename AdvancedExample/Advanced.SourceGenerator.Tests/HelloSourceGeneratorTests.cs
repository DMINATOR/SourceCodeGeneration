using Microsoft.CodeAnalysis.Text;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using VerifyTestFiles = Advanced.SourceGenerator.Tests.CSharpSourceGeneratorVerifier<Advanced.SourceGenerator.HelloSourceGenerator>;

namespace Advanced.SourceGenerator.Tests
{
    public class HelloSourceGeneratorTests
    {
        [Fact]
        public async Task AddSimpleFile2()
        {
            var code = HelloSourceGenerator.GetGeneratedSource("Advanced.SourceGenerator.Tests", "Program", "dd");

            await new VerifyTestFiles.Test
            {
                TestState =
                {
                    Sources = { code },
                    GeneratedSources =
                    {
                        (typeof(Advanced.SourceGenerator.HelloSourceGenerator), "TestFile1.g.cs", SourceText.From("// Test File content 1", Encoding.UTF8, SourceHashAlgorithm.Sha1)),
                    },
                },
            }.RunAsync();
        }
    }
}
