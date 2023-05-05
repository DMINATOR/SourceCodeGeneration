using Advanced.IncrementalGenerator;
using Advanced.SourceGenerator;

namespace Advanced.App
{
    /// <summary>
    /// This example is based on - https://learn.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/source-generators-overview
    /// </summary>
    partial class Program
    {
        static void Main(string[] args)
        {
            HelloFromSourceGenerator("SourceGenerator !");

            HelloFromIncrementalGenerator("IncrementalGenerator !");
        }

        // This will be generated:
        [GenerateHelloSource]
        static partial void HelloFromSourceGenerator(string name);

        [GenerateHelloSourceIncremental]
        static partial void HelloFromIncrementalGenerator(string name);
    }
}



