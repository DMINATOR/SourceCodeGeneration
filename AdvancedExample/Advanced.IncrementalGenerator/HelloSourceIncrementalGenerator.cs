using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace Advanced.IncrementalGenerator
{
    public class GenerateHelloSourceIncrementalAttribute : Attribute
    {

    }

    /// <summary>
    /// Generates, 
    /// 
    /// see: https://andrewlock.net/exploring-dotnet-6-part-9-source-generator-updates-incremental-generators/
    /// 
    /// Note that there's no Execute() method now, in contrast to the previous ISourceGenerator interface.
    /// </summary>
    [Generator]
    public class HelloSourceIncrementalGenerator : IIncrementalGenerator
    {
        // A method that has attributes
        //static bool IsSyntaxTargetForGeneration(SyntaxNode node)
        //    => node is MethodDeclarationSyntax m && m.AttributeLists.Count > 0;

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            IncrementalValuesProvider<MethodDeclarationSyntax> methodDeclarations = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: static (s, _) => IsSyntaxTargetForGeneration(s),
                    transform: static (ctx, _) => GetSemanticTargetForGeneration(ctx))
                .Where(static m => m is not null);

            IncrementalValueProvider<(Compilation, ImmutableArray<MethodDeclarationSyntax>)> compilationAndClasses
                = context.CompilationProvider.Combine(methodDeclarations.Collect());

            context.RegisterSourceOutput(compilationAndClasses,
                static (spc, source) => Execute(source.Item1, source.Item2, spc));

            //// Add the source code to the compilation
            //context.RegisterPostInitializationOutput(ctx =>
            //ctx.AddSource(
            //           "TestFile1.g.cs",
            //           SourceText.From("// Test File content 1", Encoding.UTF8)));

            //// Add the source code to the compilation
            //context.RegisterPostInitializationOutput(ctx =>
            //ctx.AddSource(
            //           "TestFile2.g.cs",
            //           SourceText.From("// Test File content 2", Encoding.UTF8)));
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
                    if( attributeSyntax.ToString() == "GenerateHelloSourceIncremental")
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
