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



    let Delete = (id) => {

        let elem = document.getElementById(id);

        elem.parentNode.removeChild(elem);


    };





    let updateOrder = (id) => {
        $.ajax({
            url: `api/Finance/Order/Update`,
            contentType: "application/json",
            method: "POST",
            data: id.toString(),
            success: () => {

                window.setTimeout(Delete(id), 500);

            }
        });
    };






    let makeTable = () => {
        // Make the table
        let tbl = document.createElement('table');
        tbl.classList.add("table");
        //tbl.classList.add("table-fixed");
        // Make the head
        let head = document.createElement('thead');
        let row = document.createElement('tr');
        let titles = ["Id", "Sender", "Price", "Date"];



        for (i = 0; i < titles.length; i++) {
            let cell = document.createElement('th');
            cell.classList.add("sizeone");
            cell.innerHTML = titles[i];
            row.appendChild(cell);
        }
        // Create a cell that holds two buttons               
        let wrapcell = document.createElement('th');
        wrapcell.classList.add("sizetwo");

        let maspro = document.createElement('button');
        maspro.classList.add("bstyle");
        maspro.innerText = "Process All";
        wrapcell.appendChild(maspro);
        row.appendChild(wrapcell);


        head.appendChild(row);

        tbl.appendChild(head);
        let targ = document.getElementById("finannce-order");
        targ.appendChild(tbl);
        let tbody = document.createElement('tbody');
        tbody.id = "OrderBody";
        tbl.appendChild(tbody);
    };

    let addTableRow = (order) => {
        let fullName;
        // Check fullname
        if (order.senderName === null) {
            fullName ="Unknown";
        } else {
            fullName = order.senderName;
        }

                       
        $("table tbody").append(
            `<tr id=${order.orderId}>
                        <td class="sizeone" >${order.orderId}</td>
                        <td class="sizeone" >${fullName}</td>
                        <td class="sizeone" >${order.orderPrice}</td>
                        <td class="sizeone">${order.orderDate}</td>
                    </tr>`
        );


        // Create a cell that holds two buttons               
        let wrapcell = document.createElement('td');
        wrapcell.classList.add("sizetwo");

        let pro = document.createElement('button');
        pro.classList.add("bstyle");
        pro.innerText = "Process";
        wrapcell.appendChild(pro);
        pro.addEventListener("click", function () {
            updateOrder(order.orderId);
        });

        // Append it to the row
        let ttr = document.getElementById(order.orderId);
        ttr.appendChild(wrapcell);
        
    };



    let getOrders = () => {
        $.ajax({
            url: "api/Finance/Order",
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