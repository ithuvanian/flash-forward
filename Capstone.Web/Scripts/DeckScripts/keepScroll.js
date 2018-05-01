$(window).scroll(function () {
    sessionStorage.scrollTop = $(this).scrollTop();
});

window.onload(function () {
    $(window).scrollTop(sessionStorage.scrollTop);
});