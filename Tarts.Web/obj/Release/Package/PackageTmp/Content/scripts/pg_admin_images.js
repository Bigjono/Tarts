
$(document).ready(function () {
    Images.AttachButtons();
});

var Images = {
    AttachButtons: function () {
        $(".img").click(function () {
            Images.SetAsEditingImage(this);
            return false;
        });
    },
    SetAsEditingImage: function (anchor) {
        var selected = $(anchor);
        $("#imageDetails").html(selected.find(".editImage").html());
        $("#deleteImage").attr("href", "/TartsAdmin/Images/Destroy/" + $(anchor).attr("id"));
    }
};