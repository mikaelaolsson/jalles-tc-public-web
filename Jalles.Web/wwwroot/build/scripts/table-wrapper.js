function addTableWrapper() {
    const tables = document.querySelectorAll('table');
    tables.forEach((table) => {
        wrapElement(table, 'table-wrapper');
    });
}
function wrapElement(element, wrapperClass) {
    var _a;
    const wrapper = document.createElement('div');
    wrapper.classList.add(wrapperClass);
    (_a = element.parentNode) === null || _a === void 0 ? void 0 : _a.insertBefore(wrapper, element);
    wrapper.appendChild(element);
}
addTableWrapper();
//# sourceMappingURL=table-wrapper.js.map