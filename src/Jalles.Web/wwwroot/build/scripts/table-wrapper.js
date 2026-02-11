function addTableWrapper() {
  const tables = document.querySelectorAll('table');
  tables.forEach((table) => {
    wrapElement(table, 'table-wrapper');
  });
}

function wrapElement(element, wrapperClass) {
  const wrapper = document.createElement('div');
  wrapper.classList.add(wrapperClass);
  element.parentNode?.insertBefore(wrapper, element);
  wrapper.appendChild(element);
}

addTableWrapper();
