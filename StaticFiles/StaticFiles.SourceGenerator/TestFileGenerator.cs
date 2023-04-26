using Microsoft.CodeAnalysis;

namespace StaticFiles.SourceGenerator
{
    /// <summary>
    /// 
    /// </summary>
    [Generator]
    public class TestFilesGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            // Add the source code to the compilation
            context.AddSource($"TestFile1.g.cs", "// Test File content 1");
            context.AddSource($"TestFile2.g.cs", "// Test File content 2");
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            // No initialization required for this one
        }
    }
}
