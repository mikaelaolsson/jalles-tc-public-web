const listingPageForm = document.querySelector('#listing-page-form') as HTMLFormElement;

const categoryButtons = listingPageForm.querySelectorAll('.category-button, #clear-button') as NodeListOf<HTMLInputElement>;

console.log(categoryButtons);

categoryButtons.forEach(button => {
  button.addEventListener('click', function () {
    listingPageForm.submit();
  });
});