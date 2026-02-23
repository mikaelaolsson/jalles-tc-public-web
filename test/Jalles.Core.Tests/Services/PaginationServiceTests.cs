using Jalles.Core.Services;
using Jalles.Core.ViewModels;

namespace Jalles.Core.Tests.Services;

public class PaginationServiceTests
{
    private readonly PaginationService _paginationService;

    public PaginationServiceTests()
    {
        _paginationService = new PaginationService();
    }

    [Fact]
    public void GetPaginatedViewModel_WhenPageIsValid_SetPageCorrectly()
    {
        // Arrange
        var viewModel = CreateListingPageViewModel(25);
        const int page = 2;

        // Act
        var result = _paginationService.GetPaginatedViewModel(viewModel, page);

        // Assert
        result.Page.ShouldBe(2);
    }

    [Fact]
    public void GetPaginatedViewModel_WhenContentPagesExist_CalculatesPaginationCorrectly()
    {
        // Arrange
        var viewModel = CreateListingPageViewModel(25);
        const int page = 1;

        // Act
        var result = _paginationService.GetPaginatedViewModel(viewModel, page);

        // Assert
        result.Pagination.ShouldNotBeNull();
        result.Pagination.TotalPages.ShouldBe(3); // 25 items / 10 per page = 3 pages
        result.Pagination.Page.ShouldBe(1);
        result.Pagination.PageSize.ShouldBe(10);
    }

    [Theory]
    [InlineData(1, 10, "Page 0", "Page 9")]
    [InlineData(2, 10, "Page 10", "Page 19")]
    [InlineData(3, 5, "Page 20", "Page 24")]
    public void GetPaginatedViewModel_WhenOnMultiplePages_ReturnsPaginatedContent(
        int page,
        int expectedCount,
        string expectedFirstTitle,
        string expectedLastTitle)
    {
        // Arrange
        var contentPages = CreateContentPages(25);
        var viewModel = new ListingPageViewModel { ContentPages = contentPages };

        // Act
        var result = _paginationService.GetPaginatedViewModel(viewModel, page);

        // Assert
        result.ContentPages.Count().ShouldBe(expectedCount);
        result.ContentPages.First().Title.ShouldBe(expectedFirstTitle);
        result.ContentPages.Last().Title.ShouldBe(expectedLastTitle);
    }

    [Fact]
    public void GetPaginatedViewModel_WhenOnMultiplePages_CalculatesHasPreviousPageCorrectly()
    {
        // Arrange page 1
        var viewModelPage1 = CreateListingPageViewModel(25);
        var resultPage1 = _paginationService.GetPaginatedViewModel(viewModelPage1, 1);

        // Arrange page 2
        var viewModelPage2 = CreateListingPageViewModel(25);
        var resultPage2 = _paginationService.GetPaginatedViewModel(viewModelPage2, 2);

        // Assert
        resultPage1.Pagination.HasPreviousPage.ShouldBeFalse();
        resultPage2.Pagination.HasPreviousPage.ShouldBeTrue();
    }

    [Fact]
    public void GetPaginatedViewModel_WhenOnMultiplePages_CalculatesHasNextPageCorrectly()
    {
        // Arrange page 2
        var viewModelPage2 = CreateListingPageViewModel(25);
        var resultPage2 = _paginationService.GetPaginatedViewModel(viewModelPage2, 2);

        // Arrange page 3
        var viewModelPage3 = CreateListingPageViewModel(25);
        var resultPage3 = _paginationService.GetPaginatedViewModel(viewModelPage3, 3);

        // Assert
        resultPage2.Pagination.HasNextPage.ShouldBeTrue();
        resultPage3.Pagination.HasNextPage.ShouldBeFalse();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void GetPaginatedViewModel_WhenPageIsZeroOrNegative_DefaultsToPageOne(int invalidPage)
    {
        // Arrange
        var viewModel = CreateListingPageViewModel(25);

        // Act
        var result = _paginationService.GetPaginatedViewModel(viewModel, invalidPage);

        // Assert
        result.Pagination.Page.ShouldBe(1);
        result.ContentPages.Count().ShouldBe(10);
    }

    [Fact]
    public void GetPaginatedViewModel_WhenViewModelIsSecondary_ReturnsSecondaryListingPageViewModelWithCorrectPagination()
    {
        // Arrange
        var contentPages = CreateContentPages(15);
        var viewModel = new SecondaryListingPageViewModel
        {
            ContentPages = contentPages,
            MainCategory = "Bulbasaur"
        };
        const int page = 1;

        // Act
        var result = _paginationService.GetPaginatedViewModel(viewModel, page);

        // Assert
        result.ShouldBeOfType<SecondaryListingPageViewModel>();
        result.Pagination.ShouldNotBeNull();
        result.Pagination.TotalPages.ShouldBe(2);
        result.ContentPages.Count().ShouldBe(10);
        result.MainCategory.ShouldBe("Bulbasaur");
    }

    [Fact]
    public void GetPaginatedViewModel_WhenFewerItemsThanPageSize_ReturnsAllItems()
    {
        // Arrange
        var contentPages = CreateContentPages(5);
        var viewModel = new ListingPageViewModel { ContentPages = contentPages };
        const int page = 1;

        // Act
        var result = _paginationService.GetPaginatedViewModel(viewModel, page);

        // Assert
        result.ContentPages.Count().ShouldBe(5);
        result.Pagination.TotalPages.ShouldBe(1);
        result.Pagination.HasNextPage.ShouldBeFalse();
    }

    [Fact]
    public void GetPaginatedViewModel_WhenNoContentPages_ReturnsEmpty()
    {
        // Arrange
        var viewModel = new ListingPageViewModel { ContentPages = [] };
        const int page = 1;

        // Act
        var result = _paginationService.GetPaginatedViewModel(viewModel, page);

        // Assert
        result.ContentPages.ShouldBeEmpty();
        result.Pagination.TotalPages.ShouldBe(0);
    }

    [Fact]
    public void GetPaginatedViewModel_WhenPageOne_ShowsCorrectDisplayedPages()
    {
        // Arrange
        var viewModel = CreateListingPageViewModel(100);
        const int page = 1;

        // Act
        var result = _paginationService.GetPaginatedViewModel(viewModel, page);

        // Assert
        result.Pagination.DisplayedPages.ShouldContain(1);
        result.Pagination.DisplayedPages.Count().ShouldBe(5);
        result.Pagination.DisplayedPages.Max().ShouldBe(5);
    }

    [Fact]
    public void GetPaginatedViewModel_WhenPageIsInMiddle_ShowsCorrectDisplayedPages()
    {
        // Arrange
        var viewModel = CreateListingPageViewModel(100);
        const int page = 5;

        // Act
        var result = _paginationService.GetPaginatedViewModel(viewModel, page);

        // Assert
        result.Pagination.DisplayedPages.ShouldContain(5);
        result.Pagination.DisplayedPages.Count().ShouldBe(5);
    }

    [Fact]
    public void GetPaginatedViewModel_WhenPageIsLast_ShowsCorrectDisplayedPages()
    {
        // Arrange
        var viewModel = CreateListingPageViewModel(100);
        const int page = 10; // 100 items / 10 per page = 10 pages total

        // Act
        var result = _paginationService.GetPaginatedViewModel(viewModel, page);

        // Assert
        result.Pagination.DisplayedPages.ShouldContain(10);
        result.Pagination.DisplayedPages.Count().ShouldBe(5);
        result.Pagination.DisplayedPages.Min().ShouldBe(6); // Shows pages 6-10
    }

    private static ListingPageViewModel CreateListingPageViewModel(int contentPageCount)
    {
        return new ListingPageViewModel
        {
            ContentPages = CreateContentPages(contentPageCount)
        };
    }

    private static List<ContentPageViewModel> CreateContentPages(int count)
    {
        return Enumerable
            .Range(0, count)
            .Select(i => new ContentPageViewModel { Title = $"Page {i}" })
            .ToList();
    }
}
