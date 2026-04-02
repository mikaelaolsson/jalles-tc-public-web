using AutoMapper;
using Jalles.Core.DomainModels;
using Jalles.Core.Services.DataTransferObjects;

namespace Jalles.Core.MappingProfiles.Dtos;

public class SearchResultDtoProfile : Profile
{
    public SearchResultDtoProfile()
    {
        CreateMap<SearchResult, SearchResultDto>();
    }
}
