const mixedListingBlock = document.querySelector('.mixed-listing-block') as HTMLElement;

const colors = [
  'color-jalles-yellow',
  'color-off-white',
  'color-vanilla',
  'color-taupe',
  'color-battleship-gray',
  'color-black'
];

if (mixedListingBlock !== null || undefined && mixedListingBlock.childElementCount > 0) {
  const backButton = document.querySelector('.back-button') as HTMLElement;
  const dateBlock = document.querySelector('.date') as HTMLElement;

  if (backButton !== null || undefined) {
    getBackgroundColor(backButton, 'first');
  }

  if (dateBlock !== null || undefined) {
    getBackgroundColor(dateBlock, 'last');
  }
}

function getBackgroundColor(sourceElement: HTMLElement, firstOrLast: string): void {
  let element = null;

  if (firstOrLast == 'last') {
    element = mixedListingBlock.children[mixedListingBlock.childElementCount - 1] as HTMLElement;
  }
  else {
    element = mixedListingBlock.children[0] as HTMLElement;
  }

  colors.forEach(color => {
    if (element.classList.contains(color)) {
      sourceElement.classList.add(color);
    }
  });
}