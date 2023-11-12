document.addEventListener("DOMContentLoaded", function () {
  const extraScroller = document.querySelector(".extra-scroller") as HTMLElement;
  const tableWrapper = document.querySelector(".table-wrapper") as HTMLElement;

  function toggleExtraScroller() {
    extraScroller.style.display =
      tableWrapper.scrollWidth > tableWrapper.clientWidth ? "block" : "none";
  }

  if (extraScroller && tableWrapper) {
    // Initial check and toggle
    toggleExtraScroller();

    // Update on window resize
    window.addEventListener("resize", toggleExtraScroller);

    extraScroller.addEventListener("scroll", function () {
      tableWrapper.scrollLeft = extraScroller.scrollLeft;
    });

    tableWrapper.addEventListener("scroll", function () {
      extraScroller.scrollLeft = tableWrapper.scrollLeft;
    });
  }
});
