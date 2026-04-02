export default function initExtraSideScroller() {
  const extraScroller = document.querySelector('.extra-scroller');
  const extraScrollerInner = extraScroller?.querySelector('div');
  const tableWrapper = document.querySelector('.table-wrapper');
  const table = tableWrapper?.querySelector('table');

  if (!extraScroller || !extraScrollerInner || !tableWrapper || !table) return;

  function syncInnerWidth() {
    extraScrollerInner.style.width = `${table.scrollWidth}px`;
  }

  function toggleExtraScroller() {
    extraScroller.style.display =
      tableWrapper.scrollWidth > tableWrapper.clientWidth ? 'block' : 'none';
  }

  const resizeObserver = new ResizeObserver(() => {
    syncInnerWidth();
    toggleExtraScroller();
  });

  resizeObserver.observe(table);

  let syncingFromExtra = false;
  let syncingFromTable = false;

  extraScroller.addEventListener('scroll', () => {
    if (syncingFromTable) return;
    syncingFromExtra = true;
    tableWrapper.scrollLeft = extraScroller.scrollLeft;
    syncingFromExtra = false;
  });

  tableWrapper.addEventListener('scroll', () => {
    if (syncingFromExtra) return;
    syncingFromTable = true;
    extraScroller.scrollLeft = tableWrapper.scrollLeft;
    syncingFromTable = false;
  });

  syncInnerWidth();
  toggleExtraScroller();
}
