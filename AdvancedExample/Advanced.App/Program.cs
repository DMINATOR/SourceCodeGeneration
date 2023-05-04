using System.Xml.Linq;

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

        // This will be generated
        static partial void HelloFromSourceGenerator(string name);

        static partial void HelloFromIncrementalGenerator(string name);
    }
}



