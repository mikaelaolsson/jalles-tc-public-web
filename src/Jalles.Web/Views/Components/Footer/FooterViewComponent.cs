using AutoMapper;
using Jalles.Core.Contracts;
using Jalles.Core.Extensions;

namespace Jalles.Web.Views.Components.Footer;

public class FooterViewComponent : ViewComponent
{
    private readonly ILogger<FooterViewComponent> _logger;
    private readonly IMapper _mapper;
    private readonly IContentAccessor _contentAccessor;

    public FooterViewComponent(
        IMapper mapper,
        IContentAccessor contentAccessor,
        ILogger<FooterViewComponent> logger)
    {
        _logger = logger;
        _mapper = mapper;
        _contentAccessor = contentAccessor;
    }

    public IViewComponentResult Invoke()
    {
        var root = _contentAccessor.GetRoot();

        if(root is not StartPage startPage)
        {
            _logger.LogError("{StartPage} cannot be found.", nameof(StartPage));
            throw new InvalidOperationException(nameof(StartPage));
        }

        var footer = _mapper.Map<FooterViewModel>(startPage.Footer.GetElement<FooterBlock>());

        return View(footer);
    }
}
