using AutoMapper;
using Jalles.Core.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Jalles.Web.Views.Components.Footer;

public class FooterViewComponent : ViewComponent
{
    private readonly ILogger<FooterViewComponent> _logger;
    private readonly IMapper _mapper;

    public FooterViewComponent(ILogger<FooterViewComponent> logger, IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;
    }

    public IViewComponentResult Invoke(IPublishedContent content)
    {
        var startPage = content.Root<StartPage>();

        if (startPage == null)
        {
            _logger.LogError("{StartPage} cannot be found.", nameof(StartPage));
            throw new NullReferenceException(nameof(StartPage));
        }

        var footer = _mapper.Map<FooterViewModel>(startPage.Footer.GetElement<FooterBlock>());

        return View(footer);
    }
}
