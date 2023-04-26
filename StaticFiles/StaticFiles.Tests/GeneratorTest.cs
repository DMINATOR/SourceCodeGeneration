using Microsoft.CodeAnalysis.Text;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using VerifyCS = CSharpSourceGeneratorVerifier<StaticFilesSourceGenerator.LocateStaticFilesGenerator>;

namespace StaticFiles.Tests
{
    public class GeneratorTest
    {
        [Fact]
        public async Task AddSimpleFile()
        {
            var code = "initial code";
            var generated = "expected generated code";
            await new VerifyCS.Test
            {
                TestState =
                {
                    Sources = { code },
                    GeneratedSources =
                    {
                        (typeof(StaticFilesSourceGenerator.LocateStaticFilesGenerator), "GeneratedFileName", SourceText.From(generated, Encoding.UTF8, SourceHashAlgorithm.Sha256)),
                    },
                },
            }.RunAsync();
        }
    }
}
