namespace ConsoleApp
{
    /// <summary>
    /// This example is based on - https://learn.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/source-generators-overview
    /// </summary>
    partial class Program
    {
        static void Main(string[] args)
        {
            HelloFrom("Console App !");
        }

        // This will be generated
        static partial void HelloFrom(string name);
    }
}