$(document).ready(function () {
    var _URL = window.URL || window.webkitURL;

    $("#file").change(function (e) {
        var image, file;
        if ((file = this.files[0])) {
            image = new Image();
            image.onload = function () {
                src = this.src;
                $('#uploadPreview').html('<img src="' + src + '"></div>');
                e.preventDefault();
            }
        };
        image.src = _URL.createObjectURL(file);
    });
});