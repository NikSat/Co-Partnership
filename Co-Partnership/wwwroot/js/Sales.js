

$(document).ready(function () {




    // AJAX CALLS FOR DATE DEPENDENT SALES
    let getSales = (id, start, end) => {
        $.ajax({
            url: `/api/Finance/PerDate`,
            contentType: "application/json",
            method: "POST",
            data: JSON.stringify({
                Id: id,
                Date: start,
                DateProcessed: end
            }),
            success: (data) => {
                if (data.length === 0) {
                    $("#salestable tbody").empty();
                    $("#salestable tbody").append(
                        `<tr><td colspan="4">No transactions took place during that period</td></tr>`
                    );
                }
                else {

                    $("#salestable tbody").empty();
                    data.forEach(x => PopulateRows(x));

                }
            }
        });

    };

    // AJAX CALLS FOR TOTAL MONEY
    let GetMoney = () => {
        $.ajax({
            url: `/api/Finance/TotalFounds`,
            contentType: "application/json",
            method: "PUT",
            success: (data) => {
                document.getElementsByClassName("alertviewforaward")[0].id = data.memberShare;
                $("#foundsummary").empty();
                $("#foundsummary").append(
                    `<h5>Company Account Balance</h5></br>
                    <h6>Total funds:  ${(data.memberShare + data.coOpShare).toFixed(2)}</h6>
                    <h6>Member share fund:  ${data.memberShare.toFixed(2)}</h6>
                    <h6>Company repository:  ${data.coOpShare.toFixed(2)}</h6>
                    `
                );
            }
        });
    };



    // AJAX CALLS FOR OFFERS
    let GetNewOffers = () => {
        $.ajax({
            url: `/api/Finance/OrderSummary`,
            contentType: "application/json",
            method: "POST",
            data: JSON.stringify({
                Id: 2
            }),
            success: (data) => {
                $("#newoffersummary").empty();
                $("#newoffersummary").append(
                    `<h5>Offers Summary</h5></br>
                  `);
                if (data === undefined) {
                    $("#newoffersummary").append(
                        `<h6>No new offers</h6>`
                    );
                } else {
                    $("#newoffersummary").append(
                        `
                    <h6>Total new offers:  ${data.number}</h6>
                    <h6>Total items in offers:  ${data.totalItems}</h6>
                    <h6>Total cost:  ${data.totalPrice.toFixed(2)}</h6>  
                        
                        `
                    );

                }

            }
        });
    };

    // AJAX CALLS FOR ORDERS
    let GetNewOrders = () => {        
        $.ajax({
            url: `/api/Finance/OrderSummary`,
            contentType: "application/json",
            method: "POST",
            data: JSON.stringify({
                Id: 1
            }),
            success: (data) => {
                $("#newordersummary").empty();
                $("#newordersummary").append(
                    `<h5>Orders Summary</h5></br>
                  `);
                if (data===undefined) {
                    $("#newordersummary").append(
                        `<h6>No new orders</h6>`
                    );
                } else {
                    $("#newordersummary").append(
                        `
                    <h6>Total new orders:  ${data.number}</h6>
                    <h6>Total items in orders:  ${data.totalItems}</h6>
                    <h6>Total profit:  ${data.totalPrice.toFixed(2)}</h6>  
                        
                        `
                    );

                }
                   
            }
        });
    };



    // AJAX CALLS FOR MEMBERS
    let GetAllMemberSummary = () => {
        $.ajax({
            url: `/api/Finance/TotalMembers`,
            contentType: "application/json",
            method: "POST",
            success: (data) => {
                $("#dividsummary").empty();
                $("#dividsummary").append(
                    `   <h5>Member share fund: ${data.founds.toFixed(2)}</h5>
                        <h5>Total members: ${data.number}</h5>
                        <h5>Divident per member: ${(data.founds / data.number).toFixed(2)}</h5>
                  `);
            }            
        });
    };



    //AJAX CALLS FOR AWARDING DIVIDENTS
    let Award = () => {
        $.ajax({
            url: `/api/Finance/AwardDividents`,
            contentType: "application/json",
            method: "POST",
            success: (data) => {
                $("#alertview").append(
                    `
            <div class="alert alert-success">
                Unable to process, no funds in member share account. 
            </div>
                `
                );
            }
        });
    };




    // When the document is loaded create a table
    let createSalesTable = () => {
        $("#salestable").append(
            `
            <table class="table">
            <thead>
                <tr>
                    <th>Id</th>
                    <th>Date</th>
                    <th>Number of goods</th>
                <th>Price</th>
                </tr>
            </thead>
                <tbody></tbody>
            </table>
                        `
        );
    };


    PopulateRows = (x) => {
        $("#salestable tbody").append(
            `
                <tr>
                      <td> ${x.transId}</td>
                        <td>${x.transDate}</td>
                      <td>  ${x.transGoods}</td>
                       <td> ${x.transPrice}</td>
                </tr>
            `
        );
    };




    let setDates = () => {
       
        
        document.getElementById('eday').value = new Date().toISOString().slice(0, 10);
        let startdate = new Date();
        startdate.setMonth(startdate.getMonth() - 1);


        document.getElementById('sday').value = startdate.toISOString().slice(0, 10);
    };


    let CheckSend = () => {
        let type = document.getElementById('types').value;
        let start = document.getElementById('sday').value;
        let end = document.getElementById('eday').value;
        let today = new Date().toISOString().slice(0, 10);
        if (!start || !end || !type) {//|| !$("#messageText").val()) {
            let error = "Error: both dates need to be filled";

            $("#salestable").prepend(
                `
            <div class="alert alert-warning">
                ${error} 
              </div>
                `
            );
            $(".alert").delay(4000).slideUp(200, function () {
                $(this).alert('close');
            });
            return;
        }
        else if (start > end) {
            let error = "Starting date cannot be later than finishing date";

            $("#salestable").prepend(
                `
            <div class="alert alert-warning">
                ${error} 
              </div>
                `
            );
            $(".alert").delay(4000).slideUp(200, function () {
                $(this).alert('close');
            });
            return;
        }
        else if (end > today || start>today) {
            let error = "Final date must not be later than today";

            $("#salestable").prepend(
                `
            <div class="alert alert-warning">
                ${error} 
              </div>
                `
            );
            $(".alert").delay(4000).slideUp(200, function () {
                $(this).alert('close');
            });
            return;
        }
        else {
            // Increase it by one day so it will get the final days transactions
            var myDate = new Date(end);
            myDate.setDate(myDate.getDate() + 1);
            end = myDate.toISOString().slice(0, 10);
            getSales(type, start, end);
        }
    };

    // Check before you award the dividents
    CheckAward = () => {
        let amount = document.getElementsByClassName("alertviewforaward")[0].id;

        // First check if there are enough money 
        if (amount === 0) {
            $("#alertview").append(
                `
            <div class="alert alert-warning">
                Unable to process, no funds in member share account. 
            </div>
                `
            );
            $(".alert").delay(4000).slideUp(200, function () {
                $(this).alert('close');
            });
        }
        else {
            if (confirm('Award dividents? (Member fund will be depleted)')) {
                Award();
            }
        }


    };



    // Create the reports 

    // Total money
    let UpdateMoney = () => {
        GetMoney();
    };

    // New orders
    UpdateOrders = () => {
        GetNewOrders();

    };


    // New offers
    UpdateOffers = () => {
        GetNewOffers();

    };


    // Members 
    GetMemberSummary = () => {
        GetAllMemberSummary();

    };


    //Add event listener
    let AddEvents = () => {
        document.getElementById("submitable").addEventListener('click', function (event) {
            event.preventDefault();
            CheckSend();
        });

        document.getElementById("awarddividend").addEventListener('click', function (event) {
            event.preventDefault();
            CheckAward();
        });
        
    };
    


    // Make the table 
    createSalesTable();
    // Set the dates to default
    setDates();
    UpdateMoney();
    UpdateOrders();
    UpdateOffers();
    GetMemberSummary();
    AddEvents();


});