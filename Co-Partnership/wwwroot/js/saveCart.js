$(document).ready(function () {

    let saveCartToDB = () => {
        $.ajax({
            url: "/api/SaveCart",
            contentType: "application/json",
            method: "POST",
            success: console.log("Cart saved")
        });
    };

    $(window)
        .on("beforeunload", saveCartToDB());
    //.on("pagehide", saveCartToDB());
    //.on("unload", saveCartToDB());
});