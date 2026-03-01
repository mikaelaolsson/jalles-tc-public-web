
export default function initBackgroundColor() {
  const mixedListingBlock = document.querySelector('.mixed-listing-block');
  const colors = [
    'color-jalles-yellow',
    'color-pine',
    'color-mossy-rock',
    'color-granola',
    'color-oat-milk',
    'color-black'
  ];

  if (mixedListingBlock && mixedListingBlock.childElementCount > 0) {
    const backButton = document.querySelector('.back-button');
    const dateBlock = document.querySelector('.date');

    if (backButton) {
      getBackgroundColor(backButton, 'first');
    }

    if (dateBlock) {
      getBackgroundColor(dateBlock, 'last');
    }
  }

  function getBackgroundColor(sourceElement, firstOrLast) {
    let element = null;

    if (firstOrLast === 'last') {
      element = mixedListingBlock.children[mixedListingBlock.childElementCount - 1];
    } else {
      element = mixedListingBlock.children[0];
    }

    colors.forEach(color => {
      if (element.classList.contains(color)) {
        sourceElement.classList.add(color);
      }
    });
  }
}
