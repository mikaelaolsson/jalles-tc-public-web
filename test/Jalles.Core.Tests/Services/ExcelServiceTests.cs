#nullable enable
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using Jalles.Core.Services;
using Microsoft.Extensions.Logging;

namespace Jalles.Core.Tests.Services;

public class ExcelServiceTests
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ExcelService> _logger;
    private readonly ExcelService _excelService;

    public ExcelServiceTests()
    {
        var testExcelBytes = LoadRealExcelFile();
        var handler = new FakeHttpMessageHandler(testExcelBytes, HttpStatusCode.OK);
        _httpClient = new HttpClient(handler);
        _logger = A.Fake<ILogger<ExcelService>>();
        _excelService = new ExcelService(_httpClient, _logger);
    }

    [Fact]
    public async Task GetExcelRowsAsync_WhenFileUrlIsValid_ReturnsListOfRows()
    {
        // Act
        var result = await _excelService.GetExcelRowsAsync("https://example.com/adelskalender.xlsx");

        // Assert
        result.ShouldNotBeNull();
        result.ShouldNotBeEmpty();
        result.Count.ShouldBe(202);
        result.ShouldAllBe(row => row.Values.All(v => v is string));

        var headers = result[0].Keys;
        headers.ShouldContain("Förnamn");
        headers.ShouldContain("Efternamn");
        headers.ShouldContain("Marathon");
        headers.ShouldContain("Öppetspår");
        headers.ShouldContain("Vasaloppet");
        headers.ShouldContain("7-mila");
        headers.ShouldContain("Vättern-rundan");
        headers.ShouldContain("Vansbro-simningen");
        headers.ShouldContain("Lidingö-loppet");
        headers.ShouldContain("En Svensk Klassiker");
        headers.ShouldContain("Marcialonga");
        headers.ShouldContain("Kungsledenrännet");
        headers.ShouldContain("Ultralopp (längre än marathon)");
        headers.ShouldContain("Ironman");
        headers.ShouldContain("Nordensköldsloppet");
    }

    [Fact]
    public async Task GetExcelRowsAsync_WhenDownloadFails_ReturnsNull()
    {
        // Arrange
        var handler = new FakeHttpMessageHandler([], HttpStatusCode.NotFound);
        var httpClient = new HttpClient(handler);
        var service = new ExcelService(httpClient, _logger);

        // Act
        var result = await service.GetExcelRowsAsync("https://example.com/notfound.xlsx");

        // Assert
        result.ShouldBeNull();
    }

    private static byte[] LoadRealExcelFile()
    {
        // Load the real Excel file from the Resources folder
        var assembly = Assembly.GetExecutingAssembly();
        const string resourcePath = "Jalles.Core.Tests.Resources.adelskalender.xlsx";

        using var stream = assembly.GetManifestResourceStream(resourcePath) ??
            throw new FileNotFoundException($"Resource '{resourcePath}' not found");

        using var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);

        return memoryStream.ToArray();
    }

    private class FakeHttpMessageHandler : HttpMessageHandler
    {
        private readonly byte[] _content;
        private readonly HttpStatusCode _statusCode;

        public FakeHttpMessageHandler(byte[] content, HttpStatusCode statusCode)
        {
            _content = content;
            _statusCode = statusCode;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(_statusCode)
            {
                Content = new ByteArrayContent(_content)
            };

            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

            return Task.FromResult(response);
        }
    }
}
