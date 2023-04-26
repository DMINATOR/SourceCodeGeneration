using Microsoft.CodeAnalysis.Text;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using VerifyCS = CSharpSourceGeneratorVerifier<StaticFiles.SourceGenerator.LocateStaticFilesGenerator>;
using VerifyTestFiles = CSharpSourceGeneratorVerifier<StaticFiles.SourceGenerator.TestFilesGenerator>;

namespace StaticFiles.Tests
{
    public class GeneratorTest
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
                        (typeof(StaticFiles.SourceGenerator.TestFilesGenerator), "TestFile1.g.cs", SourceText.From("// Test File content 1", Encoding.UTF8, SourceHashAlgorithm.Sha1)),
                        (typeof(StaticFiles.SourceGenerator.TestFilesGenerator), "TestFile2.g.cs", SourceText.From("// Test File content 2", Encoding.UTF8, SourceHashAlgorithm.Sha1)),
                    },
                },
            }.RunAsync();
        }



        //[Fact]
        //public async Task AddSimpleFile()
        //{
        //    var code = "initial code";
        //    var generated = "expected generated code";
        //    await new VerifyCS.Test
        //    {
        //        TestState =
        //        {
        //            Sources = { code },
        //            GeneratedSources =
        //            {
        //                (typeof(StaticFilesSourceGenerator.LocateStaticFilesGenerator), "GeneratedFileName", SourceText.From(generated, Encoding.UTF8, SourceHashAlgorithm.Sha256)),
        //            },
        //        },
        //    }.RunAsync();
        //}
    }
}
