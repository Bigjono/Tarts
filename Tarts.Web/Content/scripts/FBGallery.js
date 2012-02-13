

(function($) {
    $.fn.resize = function(options) {
 
        var settings = $.extend({
            scale: 1,
            maxWidth: null,
			maxHeight: null
        }, options);
 
        return this.each(function() {
			
			if(this.tagName.toLowerCase() != "img") {
				// Only images can be resized
				return $(this);
			} 

			var width = this.naturalWidth;
			var height = this.naturalHeight;
			if(!width || !height) {
				// Ooops you are an IE user, let's fix it.
				var img = document.createElement('img');
				img.src = this.src;
				
				width = img.width;
				height = img.height;
			}
			
			if(settings.scale != 1) {
				width = width*settings.scale;
				height = height*settings.scale;
			}
			
			var pWidth = 1;
			if(settings.maxWidth != null) {
				pWidth = width/settings.maxWidth;
			}
			var pHeight = 1;
			if(settings.maxHeight != null) {
				pHeight = height/settings.maxHeight;
			}
			var reduce = 1;
			
			if(pWidth < pHeight) {
				reduce = pHeight;
			} else {
				reduce = pWidth;
			}
			
			if(reduce < 1) {
				reduce = 1;
			}
			
			var newWidth = width/reduce;
			var newHeight = height/reduce;
			
			return $(this)
				.attr("width", newWidth)
				.attr("height", newHeight);
			
        });
    }
})(jQuery);



(function($) {
    $.fn.fbGallery = function(options) {
 
        var settings = $.extend({
            autoResize: true
        }, options);

		var FBGallery = {
	PopupStatus: 0,
	GalleryItems: [],
	NextImage: undefined,
	PrevImage: undefined,
	FirstImage: undefined,
	MaxImageHeight: 600,
	MaxImageWidth: 600,
	PrevItem: undefined,
	LastItem: undefined,
    AttachButtons: function () {
		 $(".changeGroupName").keypress(function (event) {
            if (event.which == 13) {
                BrandsController.ChangeGroupName();
                return false;
            }
        });
		$("#fbGalleryBackground").click(function(){  FBGallery.DisablePopup();  });  
		$("#fbGalleryNext").click(function(){  FBGallery.ShowNext();  }); 
		$("#fbGalleryPrev").click(function(){  FBGallery.ShowPrev();  }); 
		$(document).keydown(function(e){
			var code = (e.keyCode ? e.keyCode : e.which);
			if(code==27 && FBGallery.PopupStatus==1){
				FBGallery.DisablePopup();//Press Escape event!  
			}
			if(code==37 && FBGallery.PopupStatus==1){
				FBGallery.ShowPrev();//Press Escape event!  
			}
			if(code==39 && FBGallery.PopupStatus==1){
				FBGallery.ShowNext();//Press Escape event!  
				
			}
		}); 
    },
	InsertGalleryHTML: function() {
        var fbGalleryHtml = "<div id='fbGalleryBackground'></div>";
        fbGalleryHtml += "<div id='fbGallery'>";
        fbGalleryHtml += "<div id='fbGalleryTop'>";
        fbGalleryHtml += "<image id='fbGalleryPrev' src='/content/images/previousimage.png'/>";
        fbGalleryHtml += "<image id='fbGalleryNext' src='/content/images/nextimage.png'/>";
        fbGalleryHtml += "<div id='fbGalleryImageContainer'></div>";
        fbGalleryHtml += "</div>";
        fbGalleryHtml += "<div id='fb-root-container'>";
        fbGalleryHtml += "<div id='fb-root'>";
        fbGalleryHtml += "<fb:comments href='' num_posts='10' width='500'></fb:comments>";
        fbGalleryHtml += "</div>";
        fbGalleryHtml += "</div>";
        fbGalleryHtml += "</div>";
        $('body').append(fbGalleryHtml);
    },
	PostitionImage: function (element) {
        var w = $(element).width();
        var h = $(element).height();
        var img = $(element).find('img');
		
        var imgH = img.height();
        var imgW = img.width();
		
        if(imgW > w)
        {
            var moveLeft = ((imgW - w) / 2);
            moveLeft = moveLeft - (moveLeft * 2);
            $(img).css({  "left": moveLeft  });  
        }
        if(imgW > w)
        {
            var movetop = ((imgH - h) / 2);
            movetop = movetop - (movetop * 2);
            $(img).css({  "top": movetop  });  
        }
        
    },
	LoadInitialState: function () {
		FBGallery.InsertGalleryHTML();
		prevItem = undefined;
		lastItem = undefined;
		$(".fbGallery li").each(function() { 
			
			
		});
		//FBGallery.LastImage = lastItem;
	},
	LoadPopup: function (galleryItem){  
		
		//loads popup only if it is disabled  
		if(FBGallery.PopupStatus==0){  
			$("#fbGalleryBackground").css({  "opacity": "0.7"  });  
			$("#fbGalleryBackground").fadeIn(200);  
			$("#fbGallery").fadeIn(200);  
			FBGallery.PopupStatus = 1;  
			FBGallery.MaxImageHeight = screen.height - 450;
			FBGallery.MaxImageWidth = screen.width - 200;
			if(FBGallery.MaxImageHeight > 600) {FBGallery.MaxImageHeight = 600}
			if(FBGallery.MaxImageWidth > 800) {FBGallery.MaxImageWidth = 800}
			if(FBGallery.MaxImageHeight < 300) {FBGallery.MaxImageHeight = 300}
			if(FBGallery.MaxImageWidth < 400) {FBGallery.MaxImageWidth = 400}
			var galleryTopHeight = FBGallery.MaxImageHeight + 50;
			var galleryWidth = FBGallery.MaxImageWidth + 100;
			$("#fbGalleryTop").css({ "height":galleryTopHeight});
			$("#fbGallery").css({ "width":galleryWidth});
			
		}  
		FBGallery.LoadImage(galleryItem);
	},
	DisablePopup: function (){  
		//loads popup only if it is disabled  
		if(FBGallery.PopupStatus==1){  
			$("#fbGalleryBackground").fadeOut(200);  
			$("#fbGallery").fadeOut(200);  
			FBGallery.PopupStatus = 0; 
			location.hash = "";
			FBGallery.PrevItem = undefined;
			FBGallery.LastItem = undefined;
		}  
	},
	CenterPopup: function (){  
		//request data for centering  
		var windowWidth = document.documentElement.clientWidth;  
		var windowHeight = document.documentElement.clientHeight;  
		
		var popupHeight = $("#fbGallery").height();  
		var popupWidth = $("#fbGallery").width();  
		//alert(window.offsetHeight);
		var y = (window.offsetHeight / 2) - ($("#fbGallery").offsetHeight / 2); 
		var top = windowHeight/2-popupHeight/2;
		top = top + $(window).scrollTop() + 20;
		if (top < 5){ top = 10;}
		//centering  
		$("#fbGallery").css({  
		"position": "absolute",  
		"top": top,  
		"left": windowWidth/2-popupWidth/2  
		}); 
		//only need force for IE6  
		  
		$("#fbGalleryBackground").css({  
		"height": windowHeight  
		});  
	},
	ShowNext: function() {
		FBGallery.LoadImage(FBGallery.NextImage);
	},
	ShowPrev: function() {
		FBGallery.LoadImage(FBGallery.PrevImage);
	},
	LoadImage: function(galleryItem) {
		$('#fbGalleryImageContainer').empty();
		


		var img = new Image();  
		$(img)
		.load(function () {   
		  $(this).hide();
		  $('#fbGalleryImageContainer')
			.append(this);
		  $(this).show();
		  if ($(this).height() > FBGallery.MaxImageHeight) {
			$(this).resize({maxHeight: FBGallery.MaxImageHeight});
		  }
		})
		.error(function () {
		  // notify the user that the image could not be loaded
		})
		.css("display", "inline")
		.attr('src', galleryItem.ImageUrl);
		
		$(img).click(function () {
			FBGallery.ShowNext();
			return false;
		});

		location.hash = "galleryImage=" + galleryItem.UID
		if(galleryItem.PrevImg == undefined) {
			if(FBGallery.LastItem == undefined){
				$("#fbGalleryPrev").hide();
			}
			else
			{
			
				FBGallery.PrevImage = FBGallery.LastItem;
			}
		}
		else {
			$("#fbGalleryPrev").show();
			FBGallery.PrevImage = galleryItem.PrevImg;
		}
		if(galleryItem.NextImg == undefined) {
			if(FBGallery.FirstImage == undefined){
				$("#fbGalleryNext").hide();
			}
			else
			{
				FBGallery.NextImage = FBGallery.FirstImage;
			}
		}
		else {
			$("#fbGalleryNext").show();
			FBGallery.NextImage = galleryItem.NextImg;
		}
		
		var fbConent = "<div id='fb-root'><fb:comments href='" + galleryItem.ImageUrl + "#galleryImage=" + galleryItem.UID + "' num_posts='10' width='650'></fb:comments></div>"
		$("#fb-root-container").empty().append(fbConent);
			FB.XFBML.parse();
	}
	
	
}

		FBGallery.LoadInitialState();
		FBGallery.AttachButtons();
 
        return this.each(function() {
			
			FBGallery.PostitionImage(this);
			var itm = new Object;
			itm.ImageUrl = $(this).find('a').attr("href");
			itm.UID = $(this).find('a').attr("rel");
			itm.PrevImg = FBGallery.PrevItem;
			if(FBGallery.PrevItem != undefined) {
				FBGallery.PrevItem.NextImg = itm;
			}
			FBGallery.PrevItem = itm;
			FBGallery.LastItem = itm;
			if(FBGallery.FirstImage == undefined) {
				FBGallery.FirstImage = itm;
			}
			
			$($(this)).click(function () {
				FBGallery.LoadPopup(itm);
				FBGallery.CenterPopup();
				return false;
			});
			FBGallery.GalleryItems.push(itm);
				
		});
		
			
			
       
    }
})(jQuery);
