using AutoMapper;
using Jalles.Core.Contracts;
using Jalles.Core.Models.Content;
using Jalles.Core.ViewModels;
using Jalles.Core.ViewModels.Blocks;
using Jalles.Web.Views.Components.MainMenu;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Jalles.Web.Tests.ViewComponents;

public class MainMenuViewComponentTests
{
    private const string _defaultPagePath = "/";
    private const string _defaultStartPageTitle = "Jalles TC";

    private readonly IMapper _mapper;
    private readonly IContentAccessor _contentAccessor;
    private readonly IUmbracoPagePathService _umbracoPagePathService;
    private readonly MainMenuViewComponent _component;

    public MainMenuViewComponentTests()
    {
        _mapper = A.Fake<IMapper>();
        _contentAccessor = A.Fake<IContentAccessor>();
        _umbracoPagePathService = A.Fake<IUmbracoPagePathService>();
        var nullLogger = NullLogger<MainMenuViewComponent>.Instance;

        _component = new MainMenuViewComponent(
            _mapper,
            _contentAccessor,
            _umbracoPagePathService,
            nullLogger);
    }

    [Fact]
    public void Invoke_ReturnsValidViewModel_WhenStartPageAndMenuExist()
    {
        // Arrange
        var startPage = A.Fake<StartPage>();
        var mainMenu = new List<IPublishedContent>
        {
            A.Fake<IPublishedContent>(),
            A.Fake<IPublishedContent>()
        };
        var expectedMenuItems = new[]
        {
            new BasePageViewModel {
                Title = "Bulbasaur",
                PagePath = "/bulbasaur",
                ParentPagePath = "/pokemon",
                Guid = Guid.NewGuid(),
                MetaDescription = "Bulbasaur is a Grass/Poison Pokémon.",
                Header = new MediaBlockViewModel(),
                Thumbnail = null
            },
            new BasePageViewModel {
                Title = "Charmander",
                PagePath = "/charmander",
                ParentPagePath = "/pokemon",
                Guid = Guid.NewGuid(),
                MetaDescription = "Charmander is a Fire Pokémon.",
                Header = new MediaBlockViewModel(),
                Thumbnail = null
            }
        };

        A.CallTo(() => _contentAccessor.GetRoot()).Returns(startPage);
        A.CallTo(() => startPage.Title).Returns(_defaultStartPageTitle);
        A.CallTo(() => startPage.MainMenu).Returns(mainMenu);
        A.CallTo(() => _mapper.Map<IEnumerable<BasePageViewModel>>(mainMenu)).Returns(expectedMenuItems);
        A.CallTo(() => _umbracoPagePathService.GetPagePath(startPage)).Returns(_defaultPagePath);

        // Act
        var result = _component.Invoke();

        // Assert
        result.ShouldBeOfType<ViewViewComponentResult>();
        var viewResult = (ViewViewComponentResult)result;
        var mainMenuViewModel = (MainMenuViewModel)viewResult.ViewData.Model!;

        mainMenuViewModel.ShouldNotBeNull();
        mainMenuViewModel.StartPageTitle.ShouldBe(_defaultStartPageTitle);
        mainMenuViewModel.StartPageUrl.ShouldBe(_defaultPagePath);

        var items = mainMenuViewModel.MenuItems.ToList();
        items.ShouldNotBeEmpty();
        items.Count.ShouldBe(2);
        items[0].Title.ShouldBe("Bulbasaur");
        items[0].PagePath.ShouldBe("/bulbasaur");
        items[0].ParentPagePath.ShouldBe("/pokemon");
        items[0].MetaDescription.ShouldBe("Bulbasaur is a Grass/Poison Pokémon.");
        items[0].Header.ShouldNotBeNull();
        items[0].Thumbnail.ShouldBeNull();
        items[1].Title.ShouldBe("Charmander");
        items[1].PagePath.ShouldBe("/charmander");
        items[1].ParentPagePath.ShouldBe("/pokemon");
        items[1].MetaDescription.ShouldBe("Charmander is a Fire Pokémon.");
        items[1].Header.ShouldNotBeNull();
        items[1].Thumbnail.ShouldBeNull();
    }

    [Fact]
    public void Invoke_ThrowsInvalidOperationException_WhenStartPageIsNotFound()
    {
        // Arrange
        A.CallTo(() => _contentAccessor.GetRoot())
            .Returns(null);

        // Act
        var exception = Record.Exception(_component.Invoke);

        // Assert
        exception.ShouldBeOfType<InvalidOperationException>();
        exception.Message.ShouldContain("StartPage");
    }

    [Fact]
    public void Invoke_ThrowsInvalidOperationException_WhenStartPageCastFails()
    {
        // Arrange
        var notStartPage = A.Fake<IPublishedContent>();
        A.CallTo(() => _contentAccessor.GetRoot())
            .Returns(notStartPage);

        // Act
        var exception = Record.Exception(_component.Invoke);

        // Assert
        exception.ShouldBeOfType<InvalidOperationException>();
        exception.Message.ShouldContain("StartPage");
    }

    [Fact]
    public void Invoke_ReturnsValidViewModelWithEmptyMenuItems_WhenMenuIsNull()
    {
        // Arrange
        var startPage = A.Fake<StartPage>();

        A.CallTo(() => _contentAccessor.GetRoot()).Returns(startPage);
        A.CallTo(() => startPage.Title).Returns(_defaultStartPageTitle);
        A.CallTo(() => startPage.MainMenu).Returns(null);
        A.CallTo(() => _mapper.Map<IEnumerable<BasePageViewModel>>(null)).Returns(new List<BasePageViewModel>());
        A.CallTo(() => _umbracoPagePathService.GetPagePath(startPage)).Returns(_defaultPagePath);

        // Act
        var result = _component.Invoke();

        // Assert
        result.ShouldBeOfType<ViewViewComponentResult>();
        var viewResult = (ViewViewComponentResult)result;
        var mainMenuViewModel = (MainMenuViewModel)viewResult.ViewData.Model!;

        mainMenuViewModel.ShouldNotBeNull();
        mainMenuViewModel.StartPageTitle.ShouldBe(_defaultStartPageTitle);
        mainMenuViewModel.StartPageUrl.ShouldBe(_defaultPagePath);
        mainMenuViewModel.MenuItems.ShouldNotBeNull();

        var items = mainMenuViewModel.MenuItems.ToList();
        items.Count.ShouldBe(0);
        items.ShouldBeEmpty();
    }
}
