$(document).ready(function () {

    let makeTable = () => {
        // Make the table
        let tbl = document.createElement('table');
        tbl.classList.add("table");
        //tbl.classList.add("table-fixed");
        // Make the head
        let head = document.createElement('thead');
        let row = document.createElement('tr');
        let titles = [ "Title", "Description", "Quantity", "Price"];



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
        let targ = document.getElementById("Product-list");
        targ.appendChild(tbl);
        let tbody = document.createElement('tbody');
        tbody.id = "ProductsBody";
        tbl.appendChild(tbody);
    };

    let addTableRow = (product) => {
       


        $("table tbody").append(
            `<tr id=${product.id}>
                        <td class="sizeone" >${product.name}</td>
                        <td class="sizeone" >${product.description}</td>
                        <td class="sizeone" >${product.stockQuantity}</td>
                        <td class="sizeone" >${product.unitPrice}</td>
                    </tr>`
        );


        // Create a cell that holds two buttons               
        //let wrapcell = document.createElement('td');
        //wrapcell.classList.add("sizetwo");

        //let pro = document.createElement('button');
        //pro.classList.add("bstyle");
        //pro.innerText = "Process";
        //wrapcell.appendChild(pro);
        //pro.addEventListener("click", function () {
        //    updateOrder(order.orderId);
        //});

        // Append it to the row
        //let ttr = document.getElementById(order.orderId);
        //ttr.appendChild(wrapcell);

    };



    let getProducts = () => {
        $.ajax({
            url: "api/Products",
            contentType: "application/json",
            method: "GET",
            success: (data) => {
                if (data.length === 0) {
                    let targ = document.getElementById("Product-list");
                    let al = document.createElement('span');
                    al.innerHTML = "No products";
                    targ.appendChild(al);

                }
                else {
                    makeTable();
                    data.forEach(x => addTableRow(x));
                }

            }

        });
    };








    getProducts();
});