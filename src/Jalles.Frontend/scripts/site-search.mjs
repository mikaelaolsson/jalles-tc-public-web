const OVERLAY_SELECTOR = '.site-search-overlay';
const RESULTS_SELECTOR = '.search-results-container';
const MIN_SEARCH_LENGTH = 3;
const DEBOUNCE_MS = 500;
const PAGE_SIZE = 10;

let debounceTimer = null;
let controller = new AbortController();
let currentSearchTerm = '';
let totalNumberOfItems = 0;
let isFetchingMore = false;

export default function initSiteSearch() {
  const overlay = document.querySelector(OVERLAY_SELECTOR);
  if (!overlay) return;

  initOpenButton();
  initCloseButton(overlay);
  initSearchInput(overlay);
}

function initOpenButton() {
  const menu = document.querySelector('.menu');
  if (!menu) return;

  menu.addEventListener('click', event => {
    if (event.target.closest('.open-search-button')) {
      openOverlay();
    }
  });
}

function initCloseButton(overlay) {
  const closeBtn = overlay.querySelector('.close-search');
  closeBtn?.addEventListener('click', () => closeOverlay());
  overlay.addEventListener('cancel', () => closeOverlay());
}

function initSearchInput(overlay) {
  const input = overlay.querySelector('input[type="search"]');
  if (!input) return;

  input.addEventListener('input', () => {
    const term = input.value;

    clearDebounce();
    abortFetch();
    getResultsContainer().innerHTML = '';
    totalNumberOfItems = 0;

    if (term.length >= MIN_SEARCH_LENGTH) {
      debounceTimer = setTimeout(() => fetchResults(term, 0), DEBOUNCE_MS);
    }
  });

  input.addEventListener('keydown', event => {
    if (event.key === 'Enter') input.blur();
  });
}

function openOverlay() {
  const overlay = document.querySelector(OVERLAY_SELECTOR);
  const input = overlay?.querySelector('input[type="search"]');
  overlay?.showModal();
  document.body.classList.add('no-scroll');
  if (input) {
    input.value = '';
    getResultsContainer().innerHTML = '';
  }
  input?.focus();
}

function closeOverlay() {
  const overlay = document.querySelector(OVERLAY_SELECTOR);
  overlay?.close();
  document.body.classList.remove('no-scroll');
  clearDebounce();
  abortFetch();
}

function clearDebounce() {
  if (debounceTimer !== null) {
    clearTimeout(debounceTimer);
    debounceTimer = null;
  }
}

function abortFetch() {
  controller.abort();
  controller = new AbortController();
}

function getResultsContainer() {
  return document.querySelector(RESULTS_SELECTOR);
}

function setLoading(loading) {
  getResultsContainer()?.classList.toggle('loading', loading);
}

function fetchResults(term, skip) {
  currentSearchTerm = term;
  const encoded = encodeURIComponent(term);
  setLoading(true);

  fetch(`/api/search?searchTerm=${encoded}&skip=${skip}&take=${PAGE_SIZE}&culture=sv-SE`, {
    signal: controller.signal
  })
    .then(r => r.json())
    .then(data => {
      if (skip === 0) {
        renderResults(data);
      } else {
        appendResults(data.searchResultItems);
      }
    })
    .catch(err => {
      if (err.name !== 'AbortError') {
        console.error('Search error:', err);
      }
    })
    .finally(() => {
      setLoading(false);
      isFetchingMore = false;
    });
}

function renderResults(data) {
  const container = getResultsContainer();
  container.innerHTML = '';
  totalNumberOfItems = data.totalNumberOfItems;

  const countEl = document.createElement('span');
  countEl.setAttribute('role', 'status');
  countEl.textContent = `${totalNumberOfItems} träffar`;
  container.appendChild(countEl);

  const list = createResultsList(data.searchResultItems);
  container.appendChild(list);
}

function appendResults(items) {
  const list = getResultsContainer()?.querySelector('.search-results-list');
  if (!list) return;

  items.forEach(item => {
    const li = createResultItem(item);
    if (li) list.appendChild(li);
  });
}

function createResultsList(items) {
  const list = document.createElement('ul');
  list.classList.add('search-results-list');

  items.forEach(item => {
    const li = createResultItem(item);
    if (li) list.appendChild(li);
  });

  list.addEventListener('scroll', () => {
    const nearBottom = list.scrollTop + list.clientHeight >= list.scrollHeight - 100;
    if (nearBottom && !isFetchingMore) {
      const loaded = list.children.length;
      if (loaded < totalNumberOfItems) {
        isFetchingMore = true;
        fetchResults(currentSearchTerm, loaded);
      }
    }
  });

  return list;
}

function createResultItem(item) {
  const { title, uriPath, text, contentTypeTagName, updateDate } = item;
  if (!title || !uriPath) return null;

  const li = document.createElement('li');
  const a = document.createElement('a');
  const hgroup = document.createElement('hgroup');

  if (contentTypeTagName) {
    const tag = document.createElement('span');
    tag.textContent = contentTypeTagName;
    hgroup.appendChild(tag);
  }

  const h3 = document.createElement('h3');
  h3.textContent = title;
  hgroup.appendChild(h3);

  if (text) {
    const p = document.createElement('p');
    p.textContent = text;
    hgroup.appendChild(p);
  }

  if (updateDate) {
    const time = document.createElement('time');
    time.dateTime = updateDate;
    time.textContent = formatDate(updateDate);
    hgroup.appendChild(time);
  }

  a.href = uriPath;
  a.appendChild(hgroup);
  li.appendChild(a);
  return li;
}

function formatDate(dateString) {
  return new Date(dateString).toLocaleDateString('sv-SE', {
    year: 'numeric',
    month: 'short',
    day: 'numeric',
  });
}
