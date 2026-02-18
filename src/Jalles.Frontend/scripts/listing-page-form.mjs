export default function initListingPageForm() {
  const listingPageForm = document.querySelector('#listing-page-form');
  if (!listingPageForm) return;
  const categoryButtons = listingPageForm.querySelectorAll('.category-button');
  categoryButtons.forEach(button => {
    button.addEventListener('click', function () {
      listingPageForm.submit();
    });
  });
}
