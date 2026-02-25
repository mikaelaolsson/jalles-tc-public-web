using Jalles.Core.Contracts;
using Jalles.Core.ViewModels.Blocks;
using Jalles.Web.Services;
using Microsoft.AspNetCore.Http;

namespace Jalles.Web.Tests.Services;

public class ExcelBlockDataLoaderTests
{
    private readonly IExcelService _excelService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ExcelBlockDataLoader _dataLoader;

    public ExcelBlockDataLoaderTests()
    {
        _excelService = A.Fake<IExcelService>();
        _httpContextAccessor = A.Fake<IHttpContextAccessor>();
        _dataLoader = new ExcelBlockDataLoader(_excelService, _httpContextAccessor);
    }

    [Fact]
    public async Task LoadExcelDataAsync_WhenExcelFileSourceIsEmpty_ReturnsWithoutLoadingData()
    {
        // Arrange
        var viewModel = new ExcelBlockViewModel { ExcelFileSource = string.Empty };

        // Act
        await _dataLoader.LoadExcelDataAsync(viewModel);

        // Assert
        A.CallTo(() => _excelService.GetExcelRowsAsync(A<string>._, A<int>._))
            .MustNotHaveHappened();
    }

    [Fact]
    public async Task LoadExcelDataAsync_WhenDataLoadsSuccessfully_PopulatesRows()
    {
        // Arrange
        var testRows = new List<IDictionary<string, string>>
        {
            new Dictionary<string, string> { { "Name", "John" }, { "Age", "30" } },
            new Dictionary<string, string> { { "Name", "Jane" }, { "Age", "28" } }
        };

        var httpContext = A.Fake<HttpContext>();
        var request = A.Fake<HttpRequest>();
        var hostString = new HostString("example.com", 443);

        A.CallTo(() => request.Host).Returns(hostString);
        A.CallTo(() => httpContext.Request).Returns(request);
        A.CallTo(() => _httpContextAccessor.HttpContext).Returns(httpContext);
        A.CallTo(() => _excelService.GetExcelRowsAsync("https://example.com:443/files/test.xlsx", A<int>._))
            .Returns(testRows);

        var viewModel = new ExcelBlockViewModel { ExcelFileSource = "/files/test.xlsx" };

        // Act
        await _dataLoader.LoadExcelDataAsync(viewModel);

        // Assert
        viewModel.Rows.ShouldBe(testRows);
        viewModel.LoadFailed.ShouldBeFalse();
    }

    [Fact]
    public async Task LoadExcelDataAsync_WhenExcelServiceReturnsNull_SetsLoadFailedFlag()
    {
        // Arrange
        var httpContext = A.Fake<HttpContext>();
        var request = A.Fake<HttpRequest>();
        var hostString = new HostString("example.com", 443);

        A.CallTo(() => request.Host).Returns(hostString);
        A.CallTo(() => httpContext.Request).Returns(request);
        A.CallTo(() => _httpContextAccessor.HttpContext).Returns(httpContext);
        A.CallTo(() => _excelService.GetExcelRowsAsync(A<string>._, A<int>._))
            .Returns((List<IDictionary<string, string>>)null);

        var viewModel = new ExcelBlockViewModel { ExcelFileSource = "/files/test.xlsx" };

        // Act
        await _dataLoader.LoadExcelDataAsync(viewModel);

        // Assert
        viewModel.LoadFailed.ShouldBeTrue();
        viewModel.Rows.ShouldBeEmpty();
    }
}
