namespace ConsoleApp
{
    /// <summary>
    /// This example is based on - https://learn.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/source-generators-overview
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }

        static partial void HelloFrom(string name);
    }
}