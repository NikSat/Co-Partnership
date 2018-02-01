$(document).ready(function () {
    var $window = $(window);

    $(".dropdown").hover(
        function () {/*$(expr, context)
                        Is just equivalent to use the find method:
                             $(context).find(expr)*/
            
            $('.dropdown-menu', this).not('.in .dropdown-menu').show();
            $(this).addClass('open');

        },
        function () {
            if (window.innerWidth >= 768) {/*gia na mhn kleinei  se <768*/
                $('.dropdown-menu', this).not('.in .dropdown-menu').hide();
                $(this).removeClass('open');
            }
        }
    );
    //search overlay
    $(".search-trigger").click(
        function () {
            document.getElementById("overlay1").style.width = "100%";
        }
    );

    $("#close").click(
        function () {
            document.getElementById("overlay1").style.width = "0%";
        }
    );


    //$("#exampleFormControlSelect1").change(function (e) {
        
    //    var baseUrl = 'Products';
    //    location.href = baseUrl + '?sortOrder=' + $(this).val();
    //    e.preventDefault();
    //}
    //);


    ApplyAll();


});

$(function () {
    $(".card").mouseenter(function () {
        $this = $(this);
        var event2 = $this.find(".custom-position-right");
        event2.addClass("faa-pulse");
        event2.addClass("animated");
    }).mouseleave(function () {
        $this = $(this);
        var event2 = $this.find(".custom-position-right");
        event2.removeClass("faa-pulse");
        event2.removeClass("animated");

    });
});


(function ($) {
    var $window = $(window);

    function resize() {
        if (window.innerWidth < 768) {/*h innerwidth epistrefei panta  swsto apotelesma se anti8esh me th width() ths jquery*/
            $(".dropdown-menu").show();/*gia na fainetai to dropdownmenu stis mikres o8ones*/
        }
        else {
            $(".dropdown-menu").hide();/*gia na kruvetai to dropdownmenu apo md kai panw*/
        }


    }

    $window
        .resize(resize)
        .trigger('resize');
})(jQuery);


//$(window).on('load', function() {
//    var heights1 = $(".equal-height1").map(function() {
//        return $(this).height();
//    }).get(),

//    maxHeight1 = Math.max.apply(null, heights1);

//    $(".equal-height1").height(maxHeight1);
//});




/*
*
*       Nikolas added stuff here BEWARE OF MERGE CONFLICT
*
*
*/



// Put this item in the wishlist
let Favor = (id) => {
    $.ajax({
        url: "api/Wishlist/",
        contentType: "application/json",
        method: "POST",
        data: JSON.stringify({
            itemId: id
        }),
        success:  ()  => {
            ToggleColor(id)
        }
    });
}  


// Remove this item from the wishlist
let UnFavor = (id) => {
    $.ajax({
        url: `api/Wishlist/${id}`,
        contentType: "application/json",
        method: "DELETE",
        success: () => {
            ToggleColor(id)
        }
    });
    
}

// Change Colors accordingly
let ToggleColor = (id) => {
    $("#id span").toggleClass('red grey');
}

// Apply the event listeners to all the heart spans and give them the parent's id
let ApplyAll = () => {
    let one = document.querySelector(".fa-heart");
    one.addEventListener("click", ToggleFavor(document.querySelector("span.fa-heart").parentNode.id));        
}


// This function favors an item or unfavors it if it is already liked
let ToggleFavor = (id) => {
    if ($(`#${id} span`).hasClass("grey")) {
        Favor(id);
    }
    else {
        UnFavor(id);
    }
}