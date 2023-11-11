const listingPageForm = document.querySelector('#listing-page-form');
const categoryButtons = listingPageForm.querySelectorAll('.category-button');
categoryButtons.forEach(button => {
    button.addEventListener('click', function () {
        listingPageForm.submit();
    });
});
//# sourceMappingURL=listing-page-form.js.map