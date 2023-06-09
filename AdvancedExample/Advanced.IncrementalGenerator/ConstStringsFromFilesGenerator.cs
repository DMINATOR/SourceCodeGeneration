﻿using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.IO;
using System.Text;
using System.Threading;

namespace Advanced.IncrementalGenerator
{
    /// <summary>
    /// Generates const strings for file locations from included files.
    /// 
    /// Make sure files are set to be "C# analyzer additional file" in their Properties for this to work.
    /// 
    /// Example for including additional files: https://devblogs.microsoft.com/dotnet/new-c-source-generator-samples/
    /// For incremental generators: https://stackoverflow.com/questions/72095200/c-sharp-incremental-generator-how-i-can-read-additional-files-additionaltexts
    /// </summary>
    [Generator]
    public class ConstStringsFromFilesGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            // Locate all additional files that match
            var textFiles = context.AdditionalTextsProvider.Where(static file => IsAdditionalFileTargetForGeneration(file));

            // Combine the selected enums with the `Compilation`
            IncrementalValueProvider<(Compilation, ImmutableArray<AdditionalText>)> compilationAndFiles = context.CompilationProvider.Combine(textFiles.Collect());

            // Generate consts from the files
            context.RegisterSourceOutput(compilationAndFiles, static (spc, source) => Execute(source.Item1, source.Item2, spc));
        }

        // Define condition for this pipeline compilation to proceed
        static bool IsAdditionalFileTargetForGeneration(AdditionalText file)
        {
            return file.Path.EndsWith(".txt");
        }

        // Execute pipeline to generate source code
        private static void Execute(Compilation compilation, ImmutableArray<AdditionalText> additionalTexts, SourceProductionContext context)
        {
            var generatedSource = GetGeneratedSource(additionalTexts, context.CancellationToken);
            context.AddSource($"ConstStringsFromIncrementalGenerator.g.cs", generatedSource);
        }

        // Code generation for the class
        public static string GetGeneratedSource(ImmutableArray<AdditionalText> additionalTexts, CancellationToken cancellationToken)
        {
            return $@"// <auto-generated/>
public static partial class ConstStringsFromIncrementalGenerator
{{
{GetGeneratedSourceForfiles(additionalTexts, cancellationToken)}
}}";
        }

        // Code generation for each file
        private static string GetGeneratedSourceForfiles(ImmutableArray<AdditionalText> additionalTexts, CancellationToken cancellationToken)
        {
            var result = new StringBuilder();

            foreach (var additionalText in additionalTexts)
            {
                result.AppendLine($"\t\tpublic const string {Path.GetFileNameWithoutExtension(additionalText.Path)} = \"{additionalText.GetText(cancellationToken)}\";");
                result.AppendLine();
            }

            return result.ToString();
        }
    }
}
