const imageGalleries = document.querySelectorAll('.image-gallery');
if (imageGalleries.length > 0) {
    imageGalleries.forEach(imageGallery => {
        const modalImageLinks = imageGallery.querySelectorAll('.image-gallery img, .image-gallery .overlay');
        modalImageLinks.forEach(button => {
            button.addEventListener('click', function (element) {
                openImageGalleryModal(element, imageGallery.id);
            });
        });
    });
}
function openImageGalleryModal(element, id) {
    const imageGalleryModal = document.getElementById('image-gallery-modal-' + id);
    const srcElement = element.target;
    imageGalleryModal.classList.add('open');
    const imageToBeDisplayed = document.getElementById(srcElement.name);
    if (imageToBeDisplayed !== null && imageToBeDisplayed !== undefined) {
        imageToBeDisplayed.scrollIntoView({
            behavior: 'smooth',
            block: 'center'
        });
    }
    const closeModal = imageGalleryModal.querySelector('.close-button');
    closeModal.addEventListener('click', function () {
        imageGalleryModal.classList.remove('open');
    });
    imageGalleryModal.addEventListener('click', function (event) {
        const imageGalleryModalImages = imageGalleryModal.querySelectorAll('img');
        let isImage = false;
        imageGalleryModalImages.forEach(image => {
            if (event.target === image) {
                isImage = true;
            }
        });
        if (!isImage) {
            imageGalleryModal.classList.remove('open');
        }
    });
}
//# sourceMappingURL=image-gallery-modal.js.map