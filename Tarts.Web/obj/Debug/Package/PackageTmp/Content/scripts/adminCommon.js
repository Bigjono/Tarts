
$(document).ready(function () {
    Admin.Bind();
});

var Admin = {
    CurrentPopupID: undefined,
    Bind: function () {
        $('body').append("<div id='popupbackground'></div>");
        $(".close").click(function () {
            $(this).parent().hide("1000");
            return false;
        });
        $("#popupbackground").click(function () {
            Admin.ClosePopup();
        });
        $(".showpopup").click(function () {
            var divToShow = $(this).attr("rel");
            $(divToShow).fadeIn(200);
            $("#popupbackground").fadeIn(200);
            Admin.CurrentPopupID = divToShow;
            Admin.CenterPopup(divToShow);
            return false;
        });
        $(".datepicker").datepicker({ dateFormat: 'dd/mm/yy', inline: true });
    },
    ClosePopup: function () {
        $("#popupbackground").fadeOut(200);
        if (Admin.CurrentPopupID != undefined) {
            $(Admin.CurrentPopupID).fadeOut(200);
        }
    },
    CenterPopup: function (divToShow) {
        var windowWidth = document.documentElement.clientWidth;
        var windowHeight = document.documentElement.clientHeight;
        var popupHeight = $(divToShow).height();
        var popupWidth = $(divToShow).width();
        var y = (window.offsetHeight / 2) - ($(divToShow).offsetHeight / 2);
        var top = windowHeight / 2 - popupHeight / 2;
        top = top + $(window).scrollTop() + 20;
        if (top < 5) { top = 10; }
        $(divToShow).css({ "position": "absolute", "top": top, "left": windowWidth / 2 - popupWidth / 2 });
        $("#popupbackground").css({ "height": windowHeight }); //only need force for IE6  
    }

}; 