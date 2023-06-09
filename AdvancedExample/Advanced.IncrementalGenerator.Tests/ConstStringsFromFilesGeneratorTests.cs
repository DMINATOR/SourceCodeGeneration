﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Threading;
using Xunit;

namespace Advanced.IncrementalGenerator.Tests
{
    public class ConstStringsFromFilesGeneratorTests
    {
        public class AdditionalTextTestFile : AdditionalText
        {
            private string _fileId;

            public override string Path => $"path/{_fileId}.txt";

            public override SourceText GetText(CancellationToken cancellationToken = default)
            {
                return SourceText.From("This is a text file");
            }

            public AdditionalTextTestFile(string fileId)
            {
                _fileId = fileId;
            }
        }

        [Fact]
        public void Generate_2_Files_Success()
        {
            // Input to use as a source for generation
            var inputSourceCode = "";

            var additionalTexts = new List<AdditionalText>
            {
                new AdditionalTextTestFile("file1"),
                new AdditionalTextTestFile("file2")
            };

            // Actual result generated by the Source Generator based on input
            var expectedGeneratedCode =
@"// <auto-generated/>
public static partial class ConstStringsFromIncrementalGenerator
{
		public const string file1 = ""This is a text file"";

		public const string file2 = ""This is a text file"";


}";
            var result = CSharpIncrementalGeneratorVerifier<ConstStringsFromFilesGenerator>.Verify(inputSourceCode, new ConstStringsFromFilesGenerator(), additionalTexts);

            Assert.True(result.Diagnostics.IsEmpty);
            Assert.Single(result.Results);

            var resultRun = result.Results[0];
            Assert.Single(resultRun.GeneratedSources); // 1 File with two values

            Assert.Equal(expectedGeneratedCode, resultRun.GeneratedSources[0].SourceText.ToString());
            Assert.Equal("ConstStringsFromIncrementalGenerator.g.cs", resultRun.GeneratedSources[0].HintName);
        }
    }
}
