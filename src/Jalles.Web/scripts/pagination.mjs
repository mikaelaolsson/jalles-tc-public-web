const paginationForm = getPaginationForm();
const currentPage = document.querySelector('#page');

// Add event listeners for numbers
const pageButtons = document.querySelectorAll('.pagination .page-button');

paginationForm?.addEventListener('submit', function () {
  currentPage.value = '1';
});

const next = document.querySelector('#next');
const previous = document.querySelector('#previous');

pageButtons.forEach(function (pageButton) {
  pageButton.addEventListener('click', function () {
    currentPage.value = pageButton.value;
    submitPaginationForm();
  });
});

next?.addEventListener('click', function () {
  const value = currentPage.value;
  currentPage.value = (parseInt(value) + 1).toString();
  submitPaginationForm();
});

previous?.addEventListener('click', function () {
  const value = currentPage.value;
  currentPage.value = (parseInt(value) - 1).toString();
  submitPaginationForm();
});

function submitPaginationForm() {
  const selectedInput = document.querySelector('#selected-input');
  const selectedCategory = listingPageForm.querySelector('.category-button.selected');

  if (selectedInput?.value !== undefined || null || 'Alla') {
    selectedInput.value = selectedCategory.value;
  }

  paginationForm?.submit();
}

function getPaginationForm() {
  const forms = document.querySelectorAll('form');

  for (const form of forms) {
    const pageInput = form.querySelector('#page');

    if (pageInput !== undefined) {
      return form;
    }
  }

  return null;
}
