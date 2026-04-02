using Jalles.Core.Contracts;
using Microsoft.Extensions.Logging;
using MiniExcelLibs;

namespace Jalles.Core.Services;

public class ExcelService : IExcelService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ExcelService> _logger;

    public ExcelService(HttpClient httpClient, ILogger<ExcelService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<List<IDictionary<string, string>>?> GetExcelRowsAsync(string fileUrl, int sheetIndex = 0)
    {
        try
        {
            _logger.LogInformation("Attempting to download Excel file from: {FileUrl}", fileUrl);

            var response = await _httpClient.GetAsync(fileUrl);

            if(!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to download Excel file. Status: {StatusCode} - {ReasonPhrase}", response.StatusCode, response.ReasonPhrase);

                return null;
            }

            var excelBytes = await response.Content.ReadAsByteArrayAsync();
            await using var memoryStream = new MemoryStream(excelBytes);

            var sheetNames = memoryStream.GetSheetNames();

            if(sheetIndex >= sheetNames.Count)
            {
                _logger.LogWarning("Requested sheet index {SheetIndex} exceeds available sheets ({SheetCount})", sheetIndex, sheetNames.Count);

                sheetIndex = 0;
            }

            var sheetName = sheetNames[sheetIndex];

            var objectRows = (await memoryStream.QueryAsync(useHeaderRow: true, sheetName: sheetName))
                .Cast<IDictionary<string, object>>()
                .ToList();

            return objectRows
                .ConvertAll(row => (IDictionary<string, string>)row
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value?.ToString() ?? string.Empty));
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Error downloading or parsing Excel file from: {FileUrl}", fileUrl);

            return null;
        }
    }
}
