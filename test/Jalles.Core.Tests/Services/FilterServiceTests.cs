using Jalles.Core.Services;
using Jalles.Core.ViewModels;

namespace Jalles.Core.Tests.Services;

public class FilterServiceTests
{
    private readonly FilterService _filterService;

    public FilterServiceTests()
    {
        _filterService = new FilterService();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("Alla")]
    public void GetFilteredContentPages_ReturnsAllPages_WhenCategoryIsNullEmptyOrAlla(string category)
    {
        // Arrange
        var pages = CreateContentPages("Bulbasaur", "Charmander", "Squirtle");

        // Act
        var result = _filterService.GetFilteredContentPages(pages, category);

        // Assert
        result.Count().ShouldBe(3);
        result.ShouldContain(p => p.Categories.Contains("Bulbasaur"));
        result.ShouldContain(p => p.Categories.Contains("Charmander"));
        result.ShouldContain(p => p.Categories.Contains("Squirtle"));
    }

    [Fact]
    public void GetFilteredContentPages_ReturnsFilteredPages_WhenCategoryMatches()
    {
        // Arrange
        var pages = CreateContentPages("Bulbasaur", "Charmander", "Squirtle");

        // Act
        var result = _filterService.GetFilteredContentPages(pages, "Bulbasaur");

        // Assert
        result.Count().ShouldBe(1);
        result.First().Categories.ShouldContain("Bulbasaur");
    }

    [Fact]
    public void GetFilteredContentPages_ReturnsMultiplePages_WhenMultiplePagesMatchCategory()
    {
        // Arrange
        var pages = new List<ContentPageViewModel>
        {
            new() { Categories = ["Bulbasaur", "Venusaur"] },
            new() { Categories = ["Charmander"] },
            new() { Categories = ["Bulbasaur"] },
            new() { Categories = ["Squirtle", "Bulbasaur"] }
        };

        // Act
        var result = _filterService.GetFilteredContentPages(pages, "Bulbasaur");

        // Assert
        result.Count().ShouldBe(3);
        result.ShouldAllBe(p => p.Categories.Contains("Bulbasaur"));
    }

    [Theory]
    [InlineData("Pikachu")]
    [InlineData("Jigglypuff")]
    public void GetFilteredContentPages_ReturnsEmptyCollection_WhenNoPagesMatchCategory(string nonMatchingCategory)
    {
        // Arrange
        var pages = CreateContentPages("Bulbasaur", "Charmander", "Squirtle");

        // Act
        var result = _filterService.GetFilteredContentPages(pages, nonMatchingCategory);

        // Assert
        result.ShouldBeEmpty();
    }

    [Fact]
    public void GetFilteredContentPages_IsCaseSensitive_WhenFilteringByCategory()
    {
        // Arrange
        var pages = CreateContentPages("Bulbasaur");

        // Act
        var result = _filterService.GetFilteredContentPages(pages, "bulbasaur");

        // Assert
        result.ShouldBeEmpty();
    }

    private static List<ContentPageViewModel> CreateContentPages(params string[] categories)
    {
        return [.. categories
            .Select(category => new ContentPageViewModel
            {
                Categories = [category]
            })];
    }
}
