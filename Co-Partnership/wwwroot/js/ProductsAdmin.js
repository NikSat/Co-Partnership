$(document).ready(function () {

    let makeTable = () => {
        // Make the table
        let tbl = document.createElement('table');
        tbl.classList.add("table");
        //tbl.classList.add("table-fixed");
        // Make the head
        let head = document.createElement('thead');
        let row = document.createElement('tr');
       
        let titles = ["Title", "Description", "Category", "Image", "Quantity", "IsLive", "Price","Type"];



        for (i = 0; i < titles.length; i++) {
            let cell = document.createElement('th');
            cell.classList.add("sizeone");
            cell.classList.add("row0");
            cell.id = i;
            cell.innerHTML = titles[i];
            row.appendChild(cell);
        }
        //// Create a cell that holds two buttons               
        let wrapcell = document.createElement('th');
        ////wrapcell.classList.add("sizetwo");

        //let maspro = document.createElement('button');
        //maspro.classList.add("bstyle");
        //maspro.id = "addNew";
        //maspro.innerText = "Add New ";
        //wrapcell.appendChild(maspro);
        row.appendChild(wrapcell);


        head.appendChild(row);

        tbl.appendChild(head);
        let targ = document.getElementById("Product-list");
        targ.appendChild(tbl);
        let tbody = document.createElement('tbody');
        tbody.id = "ProductsBody";
        tbl.appendChild(tbody);

        $(".row0")
            .click((e) => {
                console.log(e.target.id);
                sortTable(e.target.id);
            })
        
    };

    let addTableRow = (product) => {
       
        

        $("#Product-list tbody").append(
            `<tr id=${product.id}>
                        <td class="sizeone" >${product.name}</td>
                        <td class="sizeone" >${product.description}</td>
                         <td class="sizeone" >${product.category}</td>
                        <td class="sizeone" >${product.image}</td>
                        <td class="sizeone" >${product.stockQuantity}</td>
                        <td class="sizeone" >${product.isLive}</td>
                        <td class="sizeone" >${new Intl.NumberFormat('de-DE', { style: 'currency', currency: 'EUR' }).format(product.unitPrice)}</td>
                        <td class="sizeone" >${product.unitType}</td>
                        <td class="sizeone" ><a class="btn btn-outline-info" href="EditProduct/${product.id}" >Edit</a></td>
                    </tr>`

        );


    };

    //let addNewProduct(item) => {

    //}

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

    $("#deleteBtn").on("click", function () {
        let e = $(".checkbox").find("input");
        $(e).each(function (index, value) {
            //alert(index + " " + value);
            if (value.checked) {
                console.log("a");
                clear(this.closest("tr").id);
            };

        })
        
    });

    let clear = (id) => {
        $.ajax({
            type: "POST",
            url: "DeleteProduct",          
            data: { id: id },
            success: (data) => { 
                var trEl = document.getElementById(id);
                trEl.remove();
            },
            error: function () {
                alert("Error while deleting data " + id);
            }
        });

    };

    function sortTable(n) {
        var table, rows, switching, i, x, y, shouldSwitch, dir, switchcount = 0;
        table = document.getElementById("Product-list").firstChild;
        switching = true;
        //Set the sorting direction to ascending:
        dir = "asc";
        /*Make a loop that will continue until
        no switching has been done:*/
        while (switching) {
            //start by saying: no switching is done:
            switching = false;
            rows = table.getElementsByTagName("TR");
            /*Loop through all table rows (except the
            first, which contains table headers):*/
            for (i = 1; i < (rows.length - 1); i++) {
                //start by saying there should be no switching:
                shouldSwitch = false;
                /*Get the two elements you want to compare,
                one from current row and one from the next:*/
                x = rows[i].getElementsByTagName("TD")[n];
                y = rows[i + 1].getElementsByTagName("TD")[n];
                /*check if the two rows should switch place,
                based on the direction, asc or desc:*/
                if (dir === "asc") {
                    if (x.innerHTML.toLowerCase() > y.innerHTML.toLowerCase()) {
                        //if so, mark as a switch and break the loop:
                        shouldSwitch = true;
                        break;
                    }
                } else if (dir === "desc") {
                    if (x.innerHTML.toLowerCase() < y.innerHTML.toLowerCase()) {
                        //if so, mark as a switch and break the loop:
                        shouldSwitch = true;
                        break;
                    }
                }
            }
            if (shouldSwitch) {
                /*If a switch has been marked, make the switch
                and mark that a switch has been done:*/
                rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);
                switching = true;
                //Each time a switch is done, increase this count by 1:
                switchcount++;
            } else {
                /*If no switching has been done AND the direction is "asc",
                set the direction to "desc" and run the while loop again.*/
                if (switchcount === 0 && dir === "asc") {
                    dir = "desc";
                    switching = true;
                }
            }
        }
    }






    getProducts();
});