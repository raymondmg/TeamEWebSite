

/* ҳ���ȡ
-----------------------------------------*/
$(document).ready(function(){


    $('.mobile-menu-icon').click(function () {
        $('.templatemo-left-nav').slideToggle();
    });

    /* ����رհ�ť�رհ�� */
    $('.templatemo-content-widget .fa-times').click(function () {
        $(this).parent().slideUp(function () {
            $(this).hide();
        });
    });
});