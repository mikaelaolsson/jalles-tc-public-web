using System.Globalization;
using AutoMapper;
using Jalles.Core.Services.DataTransferObjects;
using Jalles.Web.Contracts;

namespace Jalles.Web.ApiControllers;

[Route("api/search")]
[ApiController]
public class SearchController : ControllerBase
{
    private readonly ISearchService _searchService;
    private readonly IMapper _mapper;

    public SearchController(ISearchService searchService, IMapper mapper)
    {
        _searchService = searchService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<SearchResultDto> GetAsync(
        [FromQuery] string searchTerm,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 10,
        [FromQuery] string culture = "sv-SE")
    {
        CultureInfo.CurrentUICulture = CultureInfo.CurrentCulture = new CultureInfo(culture);
        var searchResult = await _searchService.QueryAsync(searchTerm, skip, take, culture);
        return _mapper.Map<SearchResultDto>(searchResult);
    }
}
