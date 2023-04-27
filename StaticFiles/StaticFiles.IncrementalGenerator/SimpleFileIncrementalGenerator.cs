using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace StaticFiles.IncrementalGenerator
{
    /// <summary>
    /// Generates two source files
    /// </summary>
    [Generator]
    public class SimpleFileIncrementalGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            // Add the source code to the compilation
            context.RegisterPostInitializationOutput(ctx =>
            ctx.AddSource(
                       "TestFile1.g.cs",
                       SourceText.From("// Test File content 1", Encoding.UTF8)));

            // Add the source code to the compilation
            context.RegisterPostInitializationOutput(ctx =>
            ctx.AddSource(
                       "TestFile2.g.cs",
                       SourceText.From("// Test File content 2", Encoding.UTF8)));
        }
    }
}
