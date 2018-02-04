$(document).ready(function () {



    // AJAX CALLS HERE
    // Get all the messages
    let GetAll = () => {
        $.ajax({
            url: "/api/Wishlist",
            contentType: "application/json",
            method: "GET",
            success: (data) => {
                // To manipulate the data

            }


        });

    };

    // Get messages from specific date
    let GetAllByDate = (start,end) => {
        $.ajax({
            url: "/api/Wishlist/Date/",
            contentType: "application/json",
            method: "GET",
            success: (data) => {
                // To manipulate the data

            }


        });

    };









    // CONSTRUCTION FUNCTIONS HERE
    // This function creates the message table with all the messages
    let MakeMessageListBody = () => {
        $("#messageBody").append("<h2>Hello</h2>");
            /*
            `<div class="EmailHead">
                    <h2>Inbox</h2>
            </div>
            <div class="subBody">
                <div class="bodyHead">
                    <div class="checkAll btn-small">
                            <input type="checkbox" name="SelectAll" value="All"><a>All</a>   
                    </div>
                        <a id="markRead" class="btn-small">Mark Read</a>
                        <a id="delete" class="btn-small">Delete</a>
                </div>
                <div class="mailList">
            </div>`
        );
        */
    };


    let MakeMessageList = () => {


    };
 



    MakeMessageListBody();


});