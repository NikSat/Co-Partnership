$(document).ready(function () {


  

    // AJAX CALLS FOR WISHLIST
  
    // This function gets the favorite items
    let GetFavoriteSummary = () => {
        $.ajax({
            url: `/api/Wishlist/Summary`,
            contentType: "application/json",
            method: "POST",
            success: (data) => {
                if (data.length === 0) {
                    $(".wishtable").append(
                        `<th colspan="5">No items in Wishlist</th>`
                    );
                }
                else {
                    data.forEach(x => AppendFavorite(x));
                }
            }
        });
    };


    // AJAX CALLS FOR HISTORY
    let GetHistorySummary = () => {
        $.ajax({
            url: `/api/Finance/PurchaseHistory`,
            contentType: "application/json",
            method: "POST",
            success: (data) => {
                if (data.length === 0) {
                    $(".historytable").append(
                        `<th colspan="3">No previous purchases</th>`
                    );
                }
                else {
                    data.forEach(x => AppendHistory(x));
                }
            }
        });

    };


    // ANOTHER API TO REMOVE FROM HISTORY
    let PostToggle = (id) => {
        $.ajax({
            url: "/api/Wishlist/Toggle",
            contentType: "application/json",
            method: "POST",
            data: JSON.stringify({
                itemId: id
            }),
            success: () => {
                RemoveWish(id);
            },
            error: (xhr) => {
                if (xhr.status === 401) {
                    var parentUrl = encodeURIComponent(window.location.href);
                    window.location.href = "/Account/Login?ReturnUrl=" + parentUrl;
                }
            }
        });
    };


    // Making a list of favorite objects
    let CreateFavoriteTable = () => {
        $("#history").append(
            `<table class="table historytable">
    <tr>
        <th>Order</th>
        <th>Date</th>
        <th>Price</th>
    </tr>
    
  </table>
    `
        );

    };


    // Getting all the favorite items
    let GetFavorites = () => {
        GetFavoriteSummary();


    };


    let AppendFavorite = (x) => {
        let Live;
        let Buy;
        if (x.isLive) {
            Live = "Available!";
            Buy = `<a class="btn btn-secondary w-100" href="/Cart/AddToCart?itemID=7&amp;returnUrl=%2FProducts%2F1">Add to <span class="fa fa-shopping-cart "></span></a>`;
        }
        else {
            Live = " Currently Unavailable!";
            Buy = "";
        }

        $(".wishtable tbody").append(`
                  <tr id=${x.id}>
                            <td>${x.name}</th>
                            <td>${x.category}</td>
                            <td>${Live}</td>
                            <td><a id=${x.id}><span class="fa fa-heart red" aria-hidden="true" data-toggle="tooltip" title="Remove form Wishlist"></span></a></td>
                            <td>${Buy}</td>
                </tr>
            `
        );

        // Apply an event listener to remove from wishlist
        let eva = $(`a[id=${x.id}]`)[0];
        eva.addEventListener('click', function (event) {
            PostToggle(x.id);
            event.preventDefault();
        });

    };

    //Remove the row
    RemoveWish = (id) => {
        $(`tr[id=${id}]`).remove();
    };




    let CreateHistoryTable = () => {

        $("#wishes").append(
            `<table class="table wishtable">
    <tr>
        <th>Item</th>
        <th>Category</th>
        <th>Available</th>
        <th>Like</th>
        <th>Buy</th>
    </tr>
    
  </table>
    `
        );

    };


    let GetHistory = () => {

        GetHistorySummary();
    };


    let AppendHistory = (x) => {
        $(".wishtable tbody").append(`
                  <tr id=${x.id}>
                            <td>${x.id}</th>
                            <td>${x.date}</td>
                            <td>${x.price}</td>
                 </tr>
            `
        );

    };


    //ApplyEvents();
  
    CreateFavoriteTable();
    GetFavorites();
    CreateHistoryTable();
    GetHistory();


});