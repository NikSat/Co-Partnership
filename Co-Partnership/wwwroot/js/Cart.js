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
        quantityInput.classList.add("quantityInput");
        //quantityInput.disabled = true;
        quantityInput.min = 1;
        quantityInput.max = cartItem.item.stockQuantity;
        quantityInput.required = true;
        let previous;

        $('.quantityInput').focus(() => {
            previous = this.value;
        }).change((e) => {
            if (!quantityInput.checkValidity()) {
                document.getElementById("error").innerHTML = quantityInput.validationMessage;
                this.value = previous;
            }
            else {
                cartItem.quantinty = this.value;
                cartItem = updateQuantity(cartItem);
            }
        });

        let unitPrice = new Intl.NumberFormat('de-DE', { style: 'currency', currency: 'EUR' }).format(cartItem.item.unitPrice);
        let table = document.getElementById('cartTable');
        table.appendChild(row = document.createElement('tr'));
        row.id = cartItem.id;

        row.appendChild(td1 = document.createElement('td'));
        td1.id = "itemName" + String(cartItem.id);
        td1.innerHTML = cartItem.item.name;

        row.appendChild(td2 = document.createElement('td'));
        td2.id = "unitPrice" + String(cartItem.id);
        td2.innerHTML = unitPrice;

        row.appendChild(td3 = document.createElement('td'));
        td3.id = "quantity" + String(cartItem.item.id);
        td3.appendChild(quantityInput);

        row.appendChild(td4 = document.createElement('td'));
        td1.id = "itemPrice" + String(cartItem.id);
        td4.innerHTML = itemPrice(cartItem);
    };

    let itemPrice = (cartItem) => {
        let price = cartItem.item.unitPrice * cartItem.quantinty;
        return new Intl.NumberFormat('de-DE', { style: 'currency', currency: 'EUR' }).format(price);
    }

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
        $.ajax({
            url: "api/Cart",
            contentType: "application/json",
            method: "POST",
            data: JSON.stringify({
                itemId: cartItem.itemId,
                quantinty: cartItem.quantinty
            }),
            success: (data) => {
                let p = document.getElementById("itemPrice" + String(cartItem.id));
                p.innerHTML = itemPrice(cartItem);
            }
        });
    }

    getCartItems();
});