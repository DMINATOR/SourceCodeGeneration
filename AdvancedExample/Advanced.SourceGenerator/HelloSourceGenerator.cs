﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Advanced.SourceGenerator
{
    /// <summary>
    /// Attribute to mark method for source code generation
    /// </summary>
    public class GenerateHelloSourceAttribute : Attribute
    {

    }

    /// <summary>
    /// 
    /// This example is based on - https://itnext.io/getting-into-source-generators-in-net-6bf6d4e9e346
    /// 
    /// A source generator needs to both implement the Microsoft.CodeAnalysis.ISourceGenerator interface, 
    /// and have the Microsoft.CodeAnalysis.GeneratorAttribute. Not all source generators require initialization, 
    /// and that is the case with this example implementation—where ISourceGenerator.Initialize is empty.
    /// 
    /// You also need to add EnforceExtendedAnalyzerRules=true property, see https://github.com/dotnet/roslyn-analyzers/blob/main/src/Microsoft.CodeAnalysis.Analyzers/Microsoft.CodeAnalysis.Analyzers.md#rs1036-specify-analyzer-banned-api-enforcement-setting
    /// </summary>
    [Generator]
    public class HelloSourceGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            // Get our SyntaxReceiver back
            if (!(context.SyntaxReceiver is SyntaxReceiverForGenerateHelloSourceAttribute receiver))
            {
                throw new ArgumentException("Received invalid receiver in Execute step");
            }
            else
            {
                foreach (var methodDeclarationSyntax in receiver.FoundMethodsToGenerate)
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
        }

        // Retrieve element from the syntax tree
        public T GetElement<T>(SyntaxNode syntaxNode) where T : SyntaxNode
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

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiverForGenerateHelloSourceAttribute());
        }

        internal class SyntaxReceiverForGenerateHelloSourceAttribute : ISyntaxReceiver
        {
            // Methods that were found that need to be generated
            public List<MethodDeclarationSyntax> FoundMethodsToGenerate { get; } = new List<MethodDeclarationSyntax>();

            public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
            {
                // Get all MethodDeclarations with any attributes
                // See: https://learn.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.syntax?view=roslyn-dotnet-3.11.0
                if (!(syntaxNode is MethodDeclarationSyntax methodDeclaration) || !methodDeclaration.AttributeLists.Any())
                {
                    return;
                }

                // See if this specific method declaration attributes match
                var attributes = methodDeclaration.AttributeLists
                    .Select(x => x.Attributes)
                    .SelectMany(x => x);

                var names = attributes.Select(x => x.Name);

                // Find attribute defined for the method
                var hasSimplifiedAttribute = HasSimplifiedAttribute(names);
                var hasFullNameAttribute = HasFullNameAttribute(names);

                // Add to generation list
                if (hasSimplifiedAttribute || hasFullNameAttribute)
                {
                    FoundMethodsToGenerate.Add(methodDeclaration);
                }
            }

            bool HasSimplifiedAttribute(IEnumerable<NameSyntax> names)
            {
                return names.OfType<IdentifierNameSyntax>().Any(x => x.Identifier.ValueText == "GenerateHelloSource");
            }

            bool HasFullNameAttribute(IEnumerable<NameSyntax> names)
            {
                return names.OfType<QualifiedNameSyntax>().Any(x => x.Right.Identifier.ValueText == "GenerateHelloSource");
            }
        }
    }
}
