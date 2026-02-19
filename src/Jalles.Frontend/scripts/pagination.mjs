function getPaginationForm() {
  const forms = document.querySelectorAll('form');
  for (const form of forms) {
    const pageInput = form.querySelector('#page');
    if (pageInput !== null) {
      return form;
    }
  }
  return null;
}

export default function initPagination() {
  const paginationForm = getPaginationForm();
  if (!paginationForm) return;
  const currentPage = document.querySelector('#page');
  const pageButtons = document.querySelectorAll('.pagination .page-button');

  paginationForm.addEventListener('submit', function() {
    if (currentPage) currentPage.value = '1';
  });

  const next = document.querySelector('#next');
  const previous = document.querySelector('#previous');

  pageButtons.forEach(function(pageButton) {
    pageButton.addEventListener('click', function() {
      if (currentPage) currentPage.value = pageButton.value;
      submitPaginationForm();
    });
  });

  next?.addEventListener('click', function() {
    if (currentPage) {
      const value = currentPage.value;
      currentPage.value = (parseInt(value) + 1).toString();
      submitPaginationForm();
    }
  });

  previous?.addEventListener('click', function() {
    if (currentPage) {
      const value = currentPage.value;
      currentPage.value = (parseInt(value) - 1).toString();
      submitPaginationForm();
    }
  });

  function submitPaginationForm() {
    const selectedInput = document.querySelector('#selected-input');
    const listingPageForm = document.querySelector('#listing-page-form');
    const selectedCategory = listingPageForm?.querySelector('.category-button.selected');
    if (selectedInput && selectedCategory) {
      selectedInput.value = selectedCategory.value;
    }
    paginationForm.submit();
  }
}
