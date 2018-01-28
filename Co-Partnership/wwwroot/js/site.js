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





