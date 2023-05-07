﻿using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using System.IO;

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
            IncrementalValuesProvider<AdditionalText> textFiles = context.AdditionalTextsProvider
                 .Where(static file => file.Path.EndsWith(".txt"));

            IncrementalValuesProvider<(string name, string content)> namesAndContents = textFiles
                .Select((text, cancellationToken) => (name: Path.GetFileNameWithoutExtension(text.Path), content: text.GetText(cancellationToken)!.ToString()));

            context.RegisterSourceOutput(namesAndContents, (spc, nameAndContent) =>
            {
                spc.AddSource($"TxtFile.{nameAndContent.name}.g.cs", $@"
// Counter
public static partial class TxtFile
{{
    public const string {nameAndContent.name} = ""{nameAndContent.content}"";
}}");
            });
        }

        static bool IsSyntaxTargetForGeneration(SyntaxNode node)
        {
            return node is MethodDeclarationSyntax m && m.AttributeLists.Count > 0;
        }

        static MethodDeclarationSyntax? GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
        {
            // we know the node is a MethodDeclarationSyntax thanks to IsSyntaxTargetForGeneration
            var methodDeclarationSyntax = (MethodDeclarationSyntax)context.Node;

            // loop through all the attributes on the method
            foreach (AttributeListSyntax attributeListSyntax in methodDeclarationSyntax.AttributeLists)
            {
                foreach (AttributeSyntax attributeSyntax in attributeListSyntax.Attributes)
                {
                    // Match by attribute name exactly
                    if (attributeSyntax.ToString() == "GenerateHelloSourceIncremental")
                    {
                        return methodDeclarationSyntax;
                    }
                }
            }

            // we didn't find the attribute we were looking for
            return null;
        }

        private static void Execute(Compilation compilation, ImmutableArray<MethodDeclarationSyntax> methods, SourceProductionContext context)
        {
            if (methods.IsDefaultOrEmpty)
            {
                // nothing to do yet
                return;
            }

            foreach (var methodDeclarationSyntax in methods)
            {
                // Get properties of method for generation
                var methodName = methodDeclarationSyntax.Identifier.Text;
                var className = GetElement<ClassDeclarationSyntax>(methodDeclarationSyntax).Identifier.Text;
                var namespaceName = GetElement<NamespaceDeclarationSyntax>(methodDeclarationSyntax).Name.ToString();

                var generatedSourceCode = GetGeneratedSource(namespaceName, className, methodName);

                //add file to generation
                context.AddSource($"{className}.g.cs", generatedSourceCode);
            }
        }

        // Retrieve element from the syntax tree
        public static T GetElement<T>(SyntaxNode syntaxNode) where T : SyntaxNode
        {
            while (syntaxNode != null)
            {
                if (syntaxNode is T)
                {
                    return syntaxNode as T;
                }
                else
                {
                    syntaxNode = syntaxNode.Parent;
                }
            }

            throw new Exception($"Namespace cannot be located for '{syntaxNode.GetLocation()}'");
        }

        public static string GetGeneratedSource(string namespaceName, string className, string methodName)
        {
            return $@"// <auto-generated/>
using System;

namespace {namespaceName}
{{
    public static partial class {className}
    {{
        static partial void {methodName}(string name) =>
            Console.WriteLine($""Generator says: Hi from '{{name}}'"");
    }}
}}
";
        }
    }
}