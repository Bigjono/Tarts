
$(document).ready(function () {
    EditGallery.AttachButtons();
});

var EditGallery = {
    AttachButtons: function () {
        $(".galleryitem").click(function () {
            EditGallery.SetAsDefaultImage(this);
            return false;
        });
    },
    SetAsDefaultImage: function (anchor) {
        var selected = $(anchor);

        $("#DefaultImageID").val(selected.attr("id"));
        $("#defImg").attr("src", selected.attr("rel"));
    }
};