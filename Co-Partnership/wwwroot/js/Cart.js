$(document).ready(function () {

    let makeTable = () => {
        // Make the table
        let table = document.createElement('table');
        table.classList.add("table");
        table.classList.add("table-fixed");
        table.id = "cartTable";
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

        table.appendChild(head);
        let targ = document.getElementById("cart");
        targ.appendChild(table);
        let tbody = document.createElement('tbody');
        tbody.id = "CartBody";
        table.appendChild(tbody);
    };

    let addTableRow = (cartItem) => {

        let quantityInput = document.createElement('input');
        quantityInput.type = "number";
        quantityInput.value = cartItem.quantinty;
        quantityInput.name = "quantity";
        quantityInput.id = "quantityInput" + String(cartItem.itemId);
        //quantityInput.disabled = true;
        quantityInput.min = 1;
        quantityInput.max = cartItem.item.stockQuantity;
        quantityInput.required = true;




        let unitPrice = new Intl.NumberFormat('de-DE', { style: 'currency', currency: 'EUR' }).format(cartItem.item.unitPrice);
        let table = document.getElementById('cartTable');
        table.appendChild(row = document.createElement('tr'));
        row.id = "row" + cartItem.itemId;

        row.appendChild(td1 = document.createElement('td'));
        td1.id = "itemName" + String(cartItem.itemId);
        td1.innerHTML = cartItem.item.name;

        row.appendChild(td2 = document.createElement('td'));
        td2.id = "unitPrice" + String(cartItem.itemId);
        td2.innerHTML = unitPrice;

        row.appendChild(td3 = document.createElement('td'));
        td3.id = "quantity" + String(cartItem.itemId);
        td3.appendChild(quantityInput);

        row.appendChild(td4 = document.createElement('td'));
        td4.id = "itemPrice" + String(cartItem.itemId);
        let price = cartItem.item.unitPrice * cartItem.quantinty;
        let itemPrice = Intl.NumberFormat('de-DE', { style: 'currency', currency: 'EUR' }).format(price);
        td4.innerHTML = itemPrice;


        let previous;

        $(`#quantityInput${cartItem.itemId}`).focus((e) => {
            previous = Number(e.target.value);
        }).change((e) => {
            if (!quantityInput.checkValidity()) {
                document.getElementById("error").innerHTML = quantityInput.validationMessage;
                e.target.value = previous;
                cartItem.quantinty = Number(e.target.value);
            }
            else {
                document.getElementById("error").innerHTML = "";
                cartItem.quantinty = Number(e.target.value);
                updateQuantity(cartItem);
            }
        });
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

    let updateQuantity = (cartItem) => {
        let item = {
            "ItemId": cartItem.itemId,
            "Quantinty": cartItem.quantinty
        };
        $.ajax({
            url: "api/Cart",
            contentType: "application/json",
            method: "POST",
            data: JSON.stringify(item),
            success: (data) => {
                let p = document.getElementById("itemPrice" + String(cartItem.itemId));
                //let q = Number(document.getElementById("quantity" + String(cartItem.itemId)).firstChild.nodeValue);
                let price = cartItem.item.unitPrice * cartItem.quantinty;
                let itemPrice = Intl.NumberFormat('de-DE', { style: 'currency', currency: 'EUR' }).format(price);
                p.innerHTML = itemPrice;
            }
        });
    }

    getCartItems();
});