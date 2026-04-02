using System.Globalization;
using AutoMapper;
using Jalles.Core.Contracts;

namespace Jalles.Core.MappingProfiles.Resolvers;

public class SourceResolver :
    IValueResolver<ExcelBlock, ExcelBlockViewModel, string>,
    IValueResolver<FooterBlock, FooterViewModel, string>,
    IValueResolver<Sponsor, SponsorViewModel, string>,
    IValueResolver<Attachment, AttachmentViewModel, string>,
    IValueResolver<ImageGalleryBlock, ImageGalleryBlockViewModel, IEnumerable<string>>
{
    private readonly IUmbracoPagePathService _umbracoPagePathService;

    public SourceResolver(IUmbracoPagePathService umbracoPagePathService)
    {
        _umbracoPagePathService = umbracoPagePathService;
    }

    public string Resolve(ExcelBlock source, ExcelBlockViewModel destination, string destMember, ResolutionContext context)
    {
        var file = source.File;
        if(file == null)
        {
            return string.Empty;
        }

        var culture = CultureInfo.CurrentUICulture;
        return _umbracoPagePathService.GetMediaUrl(file, culture);
    }

    public string Resolve(FooterBlock source, FooterViewModel destination, string destMember, ResolutionContext context)
    {
        var umemaranLogo = source.UmeMaranLogo;
        if(umemaranLogo == null)
        {
            return string.Empty;
        }

        var culture = CultureInfo.CurrentUICulture;
        return _umbracoPagePathService.GetMediaUrl(umemaranLogo, culture);
    }

    public string Resolve(Sponsor source, SponsorViewModel destination, string destMember, ResolutionContext context)
    {
        var logo = source.SponsorLogo;
        if(logo == null)
        {
            return string.Empty;
        }

        var culture = CultureInfo.CurrentUICulture;
        return _umbracoPagePathService.GetMediaUrl(logo, culture);
    }

    public string Resolve(Attachment source, AttachmentViewModel destination, string destMember, ResolutionContext context)
    {
        var file = source.File;
        if(file == null)
        {
            return string.Empty;
        }

        var culture = CultureInfo.CurrentUICulture;
        return _umbracoPagePathService.GetMediaUrl(file, culture);
    }

    public IEnumerable<string> Resolve(ImageGalleryBlock source, ImageGalleryBlockViewModel destination, IEnumerable<string> destMember, ResolutionContext context)
    {
        var images = source.ImageGallery;
        if(images?.Any() != true)
        {
            return [];
        }

        var culture = CultureInfo.CurrentUICulture;
        return images.Select(image => _umbracoPagePathService.GetMediaUrl(image, culture));
    }
}
