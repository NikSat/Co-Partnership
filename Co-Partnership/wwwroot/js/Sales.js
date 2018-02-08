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



    //Add event listener
    let AddEvents = () => {
        document.getElementById("submitable").addEventListener('click', function (event) {
            event.preventDefault();
            CheckSend();
        });
    };







    // Make the table 
    createSalesTable();
    // Set the dates to default
    setDates();
    AddEvents();


});