/*
*
*       This handles the requests for the wishlist
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
        success: () => {
            ToggleColor(id);
        }
    });
};


// Remove this item from the wishlist
let UnFavor = (id) => {
    $.ajax({
        url: `api/Wishlist/${id}`,
        contentType: "application/json",
        method: "DELETE",
        success: () => {
            ToggleColor(id);
        }
    });

};

// Change Colors accordingly
let ToggleColor = (id) => {
    $("#id span").toggleClass('red grey');
};


// Apply to one
let AppEv = (index, value) => {

    value.parentNode.addEventListener("click", ToggleFavor(value.parentNode.id));

};


// Apply the event listeners to all the heart spans and give them the parent's id
let ApplyAll = () => {
    $('span.fa-heart').each(ApplEv(index,value));   
};



// This function favors an item or unfavors it if it is already liked
let ToggleFavor = (id) => {
    if ($(`#${id} span`).hasClass("grey")) {
        Favor(id);
    }
    else {
        UnFavor(id);
    }
};