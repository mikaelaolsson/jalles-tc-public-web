using Jalles.Core.Contracts;

namespace Jalles.Web.Services;

public class ExcelBlockDataLoader
{
    private readonly IExcelService _excelService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ExcelBlockDataLoader(IExcelService excelService, IHttpContextAccessor httpContextAccessor)
    {
        _excelService = excelService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task LoadExcelDataAsync(ExcelBlockViewModel viewModel)
    {
        if(string.IsNullOrWhiteSpace(viewModel.ExcelFileSource))
        {
            return;
        }

        var httpContext = _httpContextAccessor.HttpContext;
        if(httpContext == null)
        {
            viewModel.LoadFailed = true;
            viewModel.Rows = [];
            return;
        }

        var fullUrl = $"https://{httpContext.Request.Host}{viewModel.ExcelFileSource}";

        var rows = await _excelService.GetExcelRowsAsync(fullUrl);
        if(rows == null)
        {
            viewModel.LoadFailed = true;
            viewModel.Rows = [];

            return;
        }

        viewModel.Rows = rows;
        viewModel.LoadFailed = false;
    }
}
