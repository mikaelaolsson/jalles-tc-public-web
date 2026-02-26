using Lucene.Net.Analysis.Miscellaneous;

namespace Jalles.Core.Helpers;

public static class AsciiHelper
{
    public static string FoldToAscii(string input)
    {
        if(string.IsNullOrEmpty(input))
            return input;

        var inputChars = input.ToCharArray();
        var outputChars = new char[inputChars.Length * 4];
        var outputLength = ASCIIFoldingFilter.FoldToASCII(inputChars, 0, outputChars, 0, inputChars.Length);
        return new string(outputChars, 0, outputLength);
    }
}
