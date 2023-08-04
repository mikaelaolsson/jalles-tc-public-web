const menuItems = document.querySelector('#menu .menu-items') as HTMLElement;
const menuToggle = document.querySelector('#menu .menu-toggle') as HTMLElement;

menuToggle?.addEventListener('click', function() {
  menuItems?.classList.toggle('show');
});
