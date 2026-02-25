const PAGE_SIZE = 10;

export default function initCategoryPaginationFilter() {
  const listingCategories = document.querySelector('#listing-categories');
  if (!listingCategories) return;

  const listings = document.querySelector('.listings');
  const pagination = listings.querySelector('.pagination');
  const prevBtn = pagination.querySelector('#previous');
  const nextBtn = pagination.querySelector('#next');
  const pageButtonsEl = pagination.querySelector('.page-buttons');
  const noResultsEl = listings.querySelector('.no-results');

  const allItems = [...listings.querySelectorAll('.listing[data-categories]')];
  const categoryButtons = [...listingCategories.querySelectorAll('.category-button')];

  const params = new URLSearchParams(window.location.search);
  let currentCategory = params.get('category') ?? 'Alla';
  let currentPage = Math.max(1, parseInt(params.get('page') ?? '1', 10));

  categoryButtons.forEach(btn => {
    btn.addEventListener('click', () => {
      currentCategory = btn.value;
      currentPage = 1;
      render();
      updateUrl();
    });
  });

  prevBtn.onclick = () => { currentPage--; render(); updateUrl(); scrollToTop(); };
  nextBtn.onclick = () => { currentPage++; render(); updateUrl(); scrollToTop(); };

  function getFiltered() {
    if (currentCategory === 'Alla') return allItems;
    return allItems.filter(item => {
      const cats = item.dataset.categories.split(',').map(c => c.trim());
      return cats.includes(currentCategory);
    });
  }

  function render() {
    const filtered = getFiltered();
    const totalPages = Math.max(1, Math.ceil(filtered.length / PAGE_SIZE));
    if (currentPage > totalPages) currentPage = totalPages;

    const start = (currentPage - 1) * PAGE_SIZE;
    const visible = new Set(filtered.slice(start, start + PAGE_SIZE));

    allItems.forEach(item => { item.hidden = !visible.has(item); });
    noResultsEl.hidden = filtered.length !== 0;

    categoryButtons.forEach(btn => {
      btn.classList.toggle('selected', btn.value === currentCategory);
    });

    renderPagination(totalPages);
  }

  function renderPagination(totalPages) {
    pagination.hidden = totalPages <= 1;

    prevBtn.disabled = currentPage === 1;
    nextBtn.disabled = currentPage === totalPages;

    const pages = getDisplayedPages(currentPage, totalPages);
    pageButtonsEl.innerHTML = pages
      .map(p => `<button class="page-button${p === currentPage ? ' active' : ''}" data-page="${p}">${p}</button>`)
      .join('');

    pageButtonsEl.querySelectorAll('.page-button').forEach(btn => {
      btn.onclick = () => {
        currentPage = parseInt(btn.dataset.page, 10);
        render();
        updateUrl();
        scrollToTop();
      };
    });
  }

  function getDisplayedPages(current, total) {
    if (total <= 5) return Array.from({ length: total }, (_, i) => i + 1);
    if (current <= 3) return [1, 2, 3, 4, 5];
    if (current >= total - 2) return [total - 4, total - 3, total - 2, total - 1, total];
    return [current - 2, current - 1, current, current + 1, current + 2];
  }

  function updateUrl() {
    const next = new URLSearchParams();
    if (currentCategory !== 'Alla') next.set('category', currentCategory);
    if (currentPage > 1) next.set('page', currentPage.toString());
    const qs = next.toString();
    history.pushState({}, '', qs ? `${window.location.pathname}?${qs}` : window.location.pathname);
  }

  function scrollToTop() {
    listingCategories.scrollIntoView({ behavior: 'smooth', block: 'start' });
  }

  render();
}
