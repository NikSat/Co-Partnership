$(document).ready(function () {
    var $window = $(window);

    $(".dropdown").hover(
        function () {/*$(expr, context)
                        Is just equivalent to use the find method:
                             $(context).find(expr)*/
            /*The first true variable in .stop(true, true); is called clearQueue. This true clears all queued animations, so they don't pile up and continue animating even after your user is done interacting with the element.
            The second true variable in .stop(true, true); is a bool that tells jQuery whether or not to jump to the end of the queue and just do the last animation that your user initiated.*/
            $('.dropdown-menu', this).not('.in .dropdown-menu').stop(true, true).show();
            $(this).addClass('open');

        },
        function () {
            if (window.innerWidth >= 768) {/*gia na mhn kleinei  se <768*/
                $('.dropdown-menu', this).not('.in .dropdown-menu').stop(true, true).hide();
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





