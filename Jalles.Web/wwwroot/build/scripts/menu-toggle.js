const menuItems = document.querySelector('.menu .menu-items');
const menuToggle = document.querySelector('.menu .open-menu-button');
menuToggle === null || menuToggle === void 0 ? void 0 : menuToggle.addEventListener('click', function () {
    menuItems === null || menuItems === void 0 ? void 0 : menuItems.classList.toggle('show');
});
initCurrentPageHighlight();
function initCurrentPageHighlight() {
    const currentPageUrl = cleanUrl(window.location.pathname);
    const menuItemLinks = document.querySelectorAll('.menu-items li a');
    menuItemLinks.forEach((menuItemLink) => {
        var _a;
        const menuItemUrl = cleanUrl((_a = menuItemLink.getAttribute('href')) !== null && _a !== void 0 ? _a : '');
        const isStartPage = currentPageUrl === '';
        const isCurrentPage = currentPageUrl === menuItemUrl;
        if (!isStartPage && isCurrentPage) {
            // Because of the way the way that different menu items need to be highlighted,
            // we need to do some extra checks to see what item to highlight.
            menuItemLink.classList.add('current');
        }
    });
}
function cleanUrl(url) {
    const cleanUrlPattern = /\/$/;
    return url.replace(cleanUrlPattern, '');
}
//# sourceMappingURL=menu-toggle.js.map