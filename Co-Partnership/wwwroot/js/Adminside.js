$(document).ready(function () {

    /*
    $("#sidebar").mCustomScrollbar({
        theme: "minimal"
    });

    $('#sidebarCollapse').on('click', function () {
        $('#sidebar, #content').toggleClass('active');
        $('.collapse.in').toggleClass('in');
        $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    });

    
    let makeTable = () =>
    {
        let body = document.getElementsByTagName('body')[0];
        var tbl = document.createElement('table');
        tbl.className += "table table-fixed";


    }
    */
    let makeTable = () => {
        // Make the table
        let tbl = document.createElement('table');
        tbl.classList.add("table");
        tbl.classList.add("table-fixed");
        // Make the head
        let head = document.createElement('thead');
        let row = document.createElement('tr');
        let titles = ["Id", "Sender", "Price", "Date"];



        for (i = 0; i < titles.length; i++) {
            let cell = document.createElement('th');
            cell.classList.add("col-3");
            cell.innerHTML = titles[i];
            row.appendChild(cell);
        }
        head.appendChild(row);

        tbl.appendChild(head);
        let targ = document.getElementById("finannce-order");
        targ.appendChild(tbl);
        let tbody = document.createElement('tbody');
        tbody.id = "OrderBody";
        tbl.appendChild(tbody);
    };

    let addTableRow = (order) => {
        $("table tbody").append(
            `<tr>
                        <td class="col-3">${order.id}</td>
                        <td class="col-3">${order.ownerId}</td>
                        <td class="col-3">${order.price}</td>
                        <td class="col-3">${order.date}</td>

                    </tr>`
        );

    };



    let getOrders = () => {
        $.ajax({
            url: "api/Finance",
            contentType: "application/json",
            method: "GET",
            success: (data) => {
                if (data.length === 0) {
                    let targ = document.getElementById("finannce-order");
                    let al = document.createElement('span');
                    al.innerHTML = "No new orders";
                    targ.appendChild(al);

                }
                else {
                    makeTable();
                    data.forEach(x => addTableRow(x));
                }

            }

        });
    };


    getOrders();



});