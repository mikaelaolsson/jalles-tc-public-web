"use strict";
const menuItems = document.querySelector('#menu .menu-items');
const menuToggle = document.querySelector('#menu .menu-toggle');
menuToggle === null || menuToggle === void 0 ? void 0 : menuToggle.addEventListener('click', function () {
    menuItems === null || menuItems === void 0 ? void 0 : menuItems.classList.toggle('show');
});
