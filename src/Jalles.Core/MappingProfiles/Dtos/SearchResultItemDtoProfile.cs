using AutoMapper;
using Jalles.Core.DomainModels;
using Jalles.Core.Services.DataTransferObjects;

namespace Jalles.Core.MappingProfiles.Dtos;

public class SearchResultItemDtoProfile : Profile
{
    public SearchResultItemDtoProfile()
    {
        CreateMap<SearchResultItem, SearchResultItemDto>();
    }
}
