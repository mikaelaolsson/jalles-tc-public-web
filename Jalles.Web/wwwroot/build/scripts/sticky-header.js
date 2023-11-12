document.addEventListener("DOMContentLoaded", function () {
    const wrapper1 = document.querySelector(".extra-scroller");
    const tableWrapper = document.querySelector(".table-wrapper");
    function toggleExtraScroller() {
        wrapper1.style.display =
            tableWrapper.scrollWidth > tableWrapper.clientWidth ? "block" : "none";
    }
    if (wrapper1 && tableWrapper) {
        // Initial check and toggle
        toggleExtraScroller();
        // Update on window resize
        window.addEventListener("resize", toggleExtraScroller);
        wrapper1.addEventListener("scroll", function () {
            tableWrapper.scrollLeft = wrapper1.scrollLeft;
        });
        tableWrapper.addEventListener("scroll", function () {
            wrapper1.scrollLeft = tableWrapper.scrollLeft;
        });
    }
});
//# sourceMappingURL=sticky-header.js.map