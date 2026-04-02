export default function initImageGalleryModal() {
  const imageGalleries = document.querySelectorAll('.image-gallery');

  imageGalleries.forEach(imageGallery => {
    const modal = document.getElementById('image-gallery-modal-' + imageGallery.id);
    if (!modal) return;

    const slidesContainer = modal.querySelector('.modal-slides');
    const counterEl = modal.querySelector('.modal-counter');
    const closeBtn = modal.querySelector('.close-button');
    const prevBtn = modal.querySelector('.modal-prev');
    const nextBtn = modal.querySelector('.modal-next');
    const slides = modal.querySelectorAll('.modal-slide');
    const totalImages = slides.length;
    let currentIndex = 0;

    function openModal(index) {
      modal.showModal();
      document.body.style.overflow = 'hidden';

      requestAnimationFrame(() => {
        requestAnimationFrame(() => {
          goToSlide(index, 'instant');
        });
      });
    }

    function closeModal() {
      modal.close();
      document.body.style.overflow = '';
    }

    function goToSlide(index, behavior = 'smooth') {
      currentIndex = index;
      slidesContainer.scrollTo({ left: index * slidesContainer.clientWidth, behavior });
      updateCounter(index);
      updateArrows(index);
    }

    function updateCounter(index) {
      if (counterEl) counterEl.textContent = `${index + 1} / ${totalImages}`;
    }

    function updateArrows(index) {
      if (prevBtn) prevBtn.disabled = index === 0;
      if (nextBtn) nextBtn.disabled = index === totalImages - 1;
    }

    imageGallery.querySelectorAll('[data-index]').forEach(el => {
      el.addEventListener('click', () => {
        openModal(parseInt(el.dataset.index, 10));
      });
    });

    closeBtn.addEventListener('click', closeModal);

    slidesContainer.addEventListener('click', e => {
      if (e.target === slidesContainer || e.target.classList.contains('modal-slide')) {
        closeModal();
      }
    });

    prevBtn?.addEventListener('click', e => {
      e.stopPropagation();
      goToSlide(Math.max(0, currentIndex - 1));
    });

    nextBtn?.addEventListener('click', e => {
      e.stopPropagation();
      goToSlide(Math.min(totalImages - 1, currentIndex + 1));
    });

    modal.addEventListener('keydown', e => {
      if (e.key === 'ArrowLeft') goToSlide(Math.max(0, currentIndex - 1));
      if (e.key === 'ArrowRight') goToSlide(Math.min(totalImages - 1, currentIndex + 1));
    });

    slidesContainer.addEventListener('scroll', () => {
      const index = Math.round(slidesContainer.scrollLeft / slidesContainer.clientWidth);
      if (index !== currentIndex) {
        currentIndex = index;
        updateCounter(index);
        updateArrows(index);
      }
    }, { passive: true });

    modal.addEventListener('close', () => {
      document.body.style.overflow = '';
    });
  });
}
