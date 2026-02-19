export default function initHighlightColumn() {
  const tableWrapper = document.querySelector('.table-wrapper');

  if (tableWrapper) {
    tableWrapper.addEventListener('mouseover', function(event) {
      const target = event.target;

      if (target.tagName === 'TD') {
        const colIndex = target.cellIndex;
        const cells = document.querySelectorAll(`.table-wrapper tbody td:nth-child(${colIndex + 1})`);

        cells.forEach((cell) => {
          cell.classList.add('highlight-column');
        });
      }
    });

    tableWrapper.addEventListener('mouseout', function() {
      const cells = document.querySelectorAll('.table-wrapper tbody td');
      cells.forEach((cell) => {
        cell.classList.remove('highlight-column');
      });
    });
  }
}
