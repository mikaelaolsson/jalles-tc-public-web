using Jalles.Core.Helpers;

namespace Jalles.Core.Tests.Helpers;

public class AsciiHelperTests
{
    [Theory]
    [InlineData("", "")]
    [InlineData("umeå", "umea")]
    [InlineData("gisslén", "gisslen")]
    [InlineData("München", "Munchen")]
    [InlineData("façade", "facade")]
    [InlineData("coöperate", "cooperate")]
    [InlineData("Crème brûlée", "Creme brulee")]
    [InlineData("født", "fodt")]
    [InlineData("smörgåsbord", "smorgasbord")]
    [InlineData("ælesund", "aelesund")]
    [InlineData("ålesund", "alesund")]
    [InlineData("båda", "bada")]
    public void FoldToAscii_ShouldConvertSpecialCharactersToAscii(string input, string expected)
    {
        var result = AsciiHelper.FoldToAscii(input);
        result.ShouldBe(expected);
    }
}
