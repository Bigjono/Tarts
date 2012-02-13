
$(document).ready(function () {
    tarts.Bind();
});

var tarts = {
    CurrentPopupID: undefined,
    Bind: function () {
        
        $('body').append("<div id='popupbackground'></div>");
        $("#popupbackground").click(function () {
            tarts.ClosePopup();
        });
        $(".showpopup").click(function () {
            var divToShow = $(this).attr("rel");
            $(divToShow).fadeIn(200);
            $("#popupbackground").fadeIn(200);
            tarts.CurrentPopupID = divToShow;
            tarts.CenterPopup(divToShow);
            return false;
        });

        try 
        {
            $('#tartsSlider').coinslider({ hoverPause: true, delay: 8000, effect: 'rain', sDelay: 2, sph: 1, sph: 2, width: 942, height: 240  });
        } catch(e) {

        } 
    },
    ClosePopup: function () {
        $("#popupbackground").fadeOut(200);
        if (tarts.CurrentPopupID != undefined) {
            $(tarts.CurrentPopupID).fadeOut(200);
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