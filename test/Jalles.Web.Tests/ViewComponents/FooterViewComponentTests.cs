using AutoMapper;
using Jalles.Core.Contracts;
using Jalles.Core.Models.Content;
using Jalles.Core.ViewModels;
using Jalles.Web.Views.Components.Footer;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Jalles.Web.Tests.ViewComponents;

public class FooterViewComponentTests
{
    private readonly IMapper _mapper;
    private readonly IContentAccessor _contentAccessor;
    private readonly IUmbracoPagePathService _umbracoPagePathService;
    private readonly FooterViewComponent _component;

    public FooterViewComponentTests()
    {
        _mapper = A.Fake<IMapper>();
        _contentAccessor = A.Fake<IContentAccessor>();
        _umbracoPagePathService = A.Fake<IUmbracoPagePathService>();
        var nullLogger = NullLogger<FooterViewComponent>.Instance;

        _component = new FooterViewComponent(
            _mapper,
            _contentAccessor,
            nullLogger);
    }

    [Fact]
    public void Invoke_ThrowsNullReferenceException_WhenStartPageIsNotFound()
    {
        // Arrange
        A.CallTo(() => _contentAccessor.GetRoot())
            .Returns(null);

        // Act & Assert
        var act = () => _component.Invoke();

        act.ShouldThrow<NullReferenceException>()
            .Message.ShouldContain(nameof(StartPage));
    }

    [Fact]
    public void Invoke_ThrowsNullReferenceException_WhenStartPageCastFails()
    {
        // Arrange
        var notAStartPage = A.Fake<IPublishedContent>();

        A.CallTo(() => _contentAccessor.GetRoot())
            .Returns(notAStartPage);

        // Act & Assert
        var act = () => _component.Invoke();

        act.ShouldThrow<NullReferenceException>()
            .Message.ShouldContain(nameof(StartPage));
    }

    [Fact]
    public void Invoke_ReturnsValidViewModel_WhenFooterIsNull()
    {
        // Arrange
        var startPage = A.Fake<StartPage>();
        A.CallTo(() => _contentAccessor.GetRoot()).Returns(startPage);
        A.CallTo(() => startPage.Footer).Returns(null);

        // Act
        var result = _component.Invoke();

        // Assert
        result.ShouldBeOfType<ViewViewComponentResult>();
        var viewResult = (ViewViewComponentResult)result;
        var actualViewModel = (FooterViewModel)viewResult.ViewData.Model;
        actualViewModel.ShouldNotBeNull();
        actualViewModel.FooterText.ShouldBeNull();
        actualViewModel.UmemaranLogoSource.ShouldBe(string.Empty);
        actualViewModel.Sponsors.ShouldNotBeNull();
        actualViewModel.Sponsors.ShouldBeEmpty();
    }
}
