using AutoMapper;
using Jalles.Core.Constants;
using Jalles.Core.Contracts;

namespace Jalles.Web.Views.Components.MainMenu;

public class MainMenuViewComponent : ViewComponent
{
    private readonly ILogger<MainMenuViewComponent> _logger;
    private readonly IMapper _mapper;
    private readonly IContentAccessor _contentAccessor;
    private readonly IUmbracoPagePathService _umbracoPagePathService;

    public MainMenuViewComponent(
        IMapper mapper,
        IContentAccessor contentAccessor,
        IUmbracoPagePathService umbracoPagePathService,
        ILogger<MainMenuViewComponent> logger)
    {
        _logger = logger;
        _mapper = mapper;
        _contentAccessor = contentAccessor;
        _umbracoPagePathService = umbracoPagePathService;
    }

    public IViewComponentResult Invoke()
    {
        var root = _contentAccessor.GetRoot();

        if(root is not StartPage startPage)
        {
            _logger.LogError("{StartPage} cannot be found.", nameof(StartPage));
            throw new NullReferenceException(nameof(StartPage));
        }

        if(startPage.MainMenu == null)
        {
            _logger.LogWarning("{StartPage.MainMenu} could not be found.", nameof(StartPage.MainMenu));
        }

        var menuItems = _mapper.Map<IEnumerable<BasePageViewModel>>(startPage?.MainMenu);
        var startPagePath = _umbracoPagePathService.GetPagePath(startPage);

        var model = new MainMenuViewModel
        {
            StartPageTitle = startPage?.Title ?? "Jalles TC",
            StartPageUrl = startPagePath,
            Facebook = startPage?.Facebook?.Url ?? JallesConstants.FacebookUrl,
            MenuItems = [.. menuItems]
        };

        return View(model);
    }
}
