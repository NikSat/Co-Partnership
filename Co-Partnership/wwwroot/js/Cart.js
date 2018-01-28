$(document).ready(function () {

    let makeTable = () => {
        // Make the table
        let tbl = document.createElement('table');
        tbl.classList.add("table");
        tbl.classList.add("table-fixed");
        // Make the head
        let head = document.createElement('thead');
        let row = document.createElement('tr');
        let titles = ["Product ", "Unit Price", "Quantity", "Total Price"];

        for (i = 0; i < titles.length; i++) {
            let cell = document.createElement('th');
            cell.classList.add("col-3");
            cell.innerHTML = titles[i];
            row.appendChild(cell);
        }

        head.appendChild(row);

        tbl.appendChild(head);
        let targ = document.getElementById("cart");
        targ.appendChild(tbl);
        let tbody = document.createElement('tbody');
        tbody.id = "CartBody";
        tbl.appendChild(tbody);
    };

    let addTableRow = (cartItem) => {
        
        let itemPrice = cartItem.item.unitPrice * cartItem.quantinty;
        $("table tbody").append(
            `<tr>
                <td class="col-3">${cartItem.item.name}</td>
                <td class="col-3">${cartItem.item.unitPrice}</td>
                <td class="col-3">${cartItem.quantinty}</td>
                <td class="col-3">${itemPrice}</td>
            </tr>`
        );

    };

    let getCartItems = () => {
        $.ajax({
            url: "api/Cart",
            contentType: "application/json",
            method: "GET",
            success: (data) => {
                if (data.length === 0) {
                    let targ = document.getElementById("cart");
                    let al = document.createElement('span');
                    al.innerHTML = "The Cart is empty..";
                    targ.appendChild(al);
                }
                else {
                    makeTable();
                    data.forEach(x => addTableRow(x));
                }
            }
        });
    };

    getCartItems();
});