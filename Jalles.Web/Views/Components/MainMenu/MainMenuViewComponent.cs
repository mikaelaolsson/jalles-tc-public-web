using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Models.Trees;

namespace Jalles.Web.Views.Components.MainMenu;

public class MainMenuViewComponent : ViewComponent
{
    private readonly ILogger<MainMenuViewComponent> _logger;
    private readonly IMapper _mapper;

    public MainMenuViewComponent(ILogger<MainMenuViewComponent> logger, IMapper mapper)
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

        if (startPage.MainMenu == null)
        {
            _logger.LogWarning("{StartPage.MainMenu} could not be found.", nameof(StartPage.MainMenu));
        }

        var menuItems = _mapper.Map<IEnumerable<ListingPageViewModel>>(startPage?.MainMenu);

        var model = new MainMenuViewModel
        {
            StartPageTitle = startPage?.Title ?? string.Empty,
            StartPageUrl = startPage?.Url() ?? string.Empty,
            MenuItems = menuItems.ToList()
        };

        return View(model);
    }
}