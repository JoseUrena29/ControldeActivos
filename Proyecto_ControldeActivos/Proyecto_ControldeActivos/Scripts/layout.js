$(document).ready(function () {
    // Start progress bar when the page starts loading
    NProgress.start();

    // Complete progress bar when page is fully loaded
    $(window).on("load", function () {
        NProgress.done();
    });

    $(window).on("beforeunload", function (event) {
        NProgress.start(); // Start loading bar when user attempts to leave
    });

    // Simulate AJAX navigation
    $(document).on("click", "a", function (e) {
        const href = $(this).attr("href");
        if (href && href !== "#") {
            NProgress.start();
        }
    });
});