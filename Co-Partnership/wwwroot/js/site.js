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

    
    ApplytoButton();




});

$(function () {
    $(".card").mouseenter(function () {

        $this = $(this);
        var event2 = $this.find(".fa-heart");
        
        if (event2[0].classList.contains("grey")) {  
          
           event2.addClass("faa-pulse");
           event2.addClass("animated");
       }
    }).mouseleave(function () {

        $this = $(this);
        var event2 = $this.find(".fa-heart");

        if (event2[0].classList.contains("grey")) {
            event2.removeClass("faa-pulse");
            event2.removeClass("animated");
        }
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




/*
*
*       This handles the requests for the wishlist
*       Nickolas added stuff here BEWARE OF MERGE CONFLICTS
*
*/

//////////////API SENDING FUNCTIONS
// Toggle favor 
let PostToggle = (id) => {
    $.ajax({
        url: "/api/Wishlist/Toggle",
        contentType: "application/json",
        method: "POST",
        data: JSON.stringify({
            itemId: id
        }),
        success: () => {
            ToggleColor(id);
        },
        error: (xhr) => {
            if (xhr.status === 401) {
                var parentUrl = encodeURIComponent(window.location.href);
                window.location.href = "/Account/Login?ReturnUrl=" + parentUrl;
            }
        }
     });
};

// Toggle favor for buttons
let PostButtonToggle = (id) => {
    $.ajax({
        url: "/api/Wishlist/Toggle",
        contentType: "application/json",
        method: "POST",
        data: JSON.stringify({
            itemId: id
        }),
        success: () => {
            ToggleInner(id);
        },
        error: (xhr) => {
            if (xhr.status === 401) {
                var parentUrl = encodeURIComponent(window.location.href);
                window.location.href = "/Account/Login?ReturnUrl=" + parentUrl;
            }
        }
    });
};

//////ON SUCCESS FUNCTIONS
// Change inner html accordingly
let ToggleInner = (id) => {
    let current = document.querySelector("#" + CSS.escape(id));
    if (current.innerHTML==="Like") {
        current.innerHTML = "<span class='fa fa-heart red mr-2'></span>Liked";
    }
    else {
        current.innerHTML = "Like";
    }
};


// Change Colors accordingly
let ToggleColor = (id) => {
    let current = document.querySelector("#" + CSS.escape(id)+ " span");
    if (current.classList.contains("grey")) {
        current.classList.replace("grey", "red");
        current.classList.remove("faa-pulse");
        current.classList.remove("animated");
    }
    else
    {
        current.classList.replace("red", "grey");
        
    }
};

//////////////////EVENT LISTENERS

// Apply the event listeners to all the heart spans and give them the parent's id
document.addEventListener('DOMContentLoaded', function () {
    ApplyAll();
});

let ApplyAll = () => {
    let one = $('span.fa-heart');
    one.each((index, value) => {
        value.parentNode.addEventListener("click", function (e) {
            ToggleFavor(value.parentNode.id);
            e.preventDefault();
        });
    });
};

// Apply event listener to one button specificaly
let ApplytoButton = () => {
    $('a.likebutton').each((index, value) => {
        value.addEventListener("click", function (e) {
            ToggleButton(value.id);
            e.preventDefault();
        });
    });

};


////////////FUNCTIONS CALLED BY EVENT LISTENERS
// This function favors an item or unfavors it if it is already liked
let ToggleFavor = (id) => {
    PostToggle(id);

};


// This function does as the previous one only it applies to like buttons
let ToggleButton = (id) => {
    PostButtonToggle(id);
};