const menuItems = document.querySelector('.menu .menu-items') as HTMLElement;
const menuToggle = document.querySelector('.menu .open-menu-button') as HTMLElement;

menuToggle?.addEventListener('click', function() {
  menuItems?.classList.toggle('show');
});

initCurrentPageHighlight();

function initCurrentPageHighlight(): void {
  const currentPageUrl = cleanUrl(window.location.pathname);
  const menuItemLinks = document.querySelectorAll('.menu-items li a') as NodeListOf<HTMLAnchorElement>;

  menuItemLinks.forEach((menuItemLink) => {
    const menuItemUrl = '/' + cleanUrl(menuItemLink.getAttribute('href') ?? '');

    const isStartPage = currentPageUrl === '';
    const isCurrentPage = currentPageUrl === menuItemUrl;

    if(!isStartPage && isCurrentPage) {
      // Because of the way the way that different menu items need to be highlighted,
      // we need to do some extra checks to see what item to highlight.

      menuItemLink.classList.add('current');
    }
  });
}

function cleanUrl(url: string): string {
  const cleanUrlPattern = /\/$/;
  return url.replace(cleanUrlPattern, '');
}