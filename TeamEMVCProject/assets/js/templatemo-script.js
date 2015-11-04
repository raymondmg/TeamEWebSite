

/* 页面读取
-----------------------------------------*/
$(document).ready(function(){


    $('.mobile-menu-icon').click(function () {
        $('.templatemo-left-nav').slideToggle();
    });

    /* 点击关闭按钮关闭板块 */
    $('.templatemo-content-widget .fa-times').click(function () {
        $(this).parent().slideUp(function () {
            $(this).hide();
        });
    });
});