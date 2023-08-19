const listingPageForm = document.querySelector('#listing-page-form') as HTMLFormElement;

const categoryButtons = listingPageForm.querySelectorAll('.category-button') as NodeListOf<HTMLInputElement>;

categoryButtons.forEach(button => {
  button.addEventListener('click', function () {
    listingPageForm.submit();
  });
});