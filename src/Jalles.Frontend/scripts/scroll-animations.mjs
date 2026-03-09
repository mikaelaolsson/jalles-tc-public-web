function animate(element, animation, delay = 0) {
  if (element === null) return;

  if (delay === 0) {
    element.classList.add(animation);
    return;
  }

  element.style.visibility = 'hidden';
  setTimeout(() => {
    element.style.visibility = 'visible';
    element.classList.add(animation);
  }, delay);
}

function setupEnterScreenAnimations(selectors) {
  const elements = document.querySelectorAll(selectors.join(', '));

  if (!elements.length) return;

  elements.forEach(el => {
    el.style.visibility = 'hidden';
  });

  const observer = new IntersectionObserver((entries) => {
    entries.forEach(entry => {
      if (entry.isIntersecting) {
        entry.target.style.visibility = 'visible';
        entry.target.classList.add('fade-up');
        observer.unobserve(entry.target);
      }
    });
  });

  elements.forEach(el => observer.observe(el));
}

function initHeaderAnimations() {
  const header = document.querySelector('.header');
  if (header === null) return;

  const heading = header.querySelector('.heading');
  const subHeading = header.querySelector('.sub-heading');
  const logoLink = header.querySelector('.startpage-header-link');

  animate(heading, 'clip-right', 100);
  animate(subHeading, 'clip-right', 100);
  animate(logoLink, 'clip-right', 100);
}

export default function initScrollAnimations() {
  const prefersReducedMotion = window.matchMedia('(prefers-reduced-motion: reduce)').matches;
  if (prefersReducedMotion) return;

  initHeaderAnimations();

  setupEnterScreenAnimations([
    '.content-block .media-container',
    '.content-block .content',
    '.highlight-listing-block .highlight',
    '.video-listing-block .videos',
    '.pin-this-block .wrapper',
    '.image-gallery-container .image-gallery',
    '.listing-page .listing',
    '.listing-page .listing-categories',
    '.footer .content',
    // TODO: add when redesigned:
    // '.text-block-container'
    // '.data-block-container'
    // '.excel-block'
    // '.our-sponsor-block'
  ]);
}
