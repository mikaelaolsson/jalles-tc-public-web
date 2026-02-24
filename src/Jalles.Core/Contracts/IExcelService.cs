namespace Jalles.Core.Contracts;

public interface IExcelService
{
    Task<List<IDictionary<string, string>>?> GetExcelRowsAsync(string fileUrl, int sheetIndex = 0);
}
