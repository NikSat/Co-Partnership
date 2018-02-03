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
    let GetAll = (start,end) => {
        $.ajax({
            url: "/api/Wishlist/Date/",
            contentType: "application/json",
            method: "GET",
            success: (data) => {
                // To manipulate the data

            }


        });

    };








    // This function makes a test table 
    let CreateTestTable(){
    
    }






});