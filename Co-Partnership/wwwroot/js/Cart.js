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
        let titles = ["Product ", "Unit Price", "Quantity", "Total Price", "Clear"];

        for (i = 0; i < titles.length; i++) {
            let cell = document.createElement('th');
            cell.classList.add("col");
            cell.innerHTML = titles[i];
            row.appendChild(cell);
        }

        clrBtn = document.createElement('input');
        clrBtn.type = 'button';
        clrBtn.id = "clearAllButton";
        clrBtn.value = "All";
        row.lastChild.appendChild(clrBtn); // insert clearAll button at last column of header

        head.appendChild(row);

        //add table to doc
        let targ = document.getElementById("cart");
        targ.appendChild(table);
        //add head to table
        table.appendChild(head);
        //add body to table
        let tbody = document.createElement('tbody');
        tbody.id = "CartBody";
        table.appendChild(tbody);
        //add footer to table
        let tfoot = document.createElement('tfoot');
        tfoot.id = "cartFoot";
        table.appendChild(tfoot);
    };

    let addTableRow = (cartItem) => {
        //Quantity Number Input create & set properties
        let quantityInput = document.createElement('input');
        quantityInput.type = "number";
        quantityInput.value = cartItem.quantinty;
        quantityInput.name = "quantityIn";
        quantityInput.classList.add("itemQuantity");
        quantityInput.id = "quantityInput" + String(cartItem.itemId);
        quantityInput.min = 1;
        quantityInput.max = cartItem.item.stockQuantity;
        quantityInput.required = true;

        //Clear Button create & set properties
        let clearButton = document.createElement('input');
        clearButton.type = "button";
        clearButton.id = "clearButton" + String(cartItem.itemId);
        clearButton.classList.add("clearbutton");

        //Format UnitPrice
        let unitPrice = new Intl.NumberFormat('de-DE', { style: 'currency', currency: 'EUR' }).format(cartItem.item.unitPrice);

        let row;
        let td1, td2, td3, td4, td5;
        let tablebody = document.getElementById('CartBody'); //get table body
        tablebody.appendChild(row = document.createElement('tr')); //create new row
        row.id = "row" + cartItem.itemId;

        //Cells create and fill
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
        td4.classList.add("itemPrice");
        td4.id = "itemPrice" + String(cartItem.itemId);
        let price = cartItem.item.unitPrice * cartItem.quantinty;
        let itemPrice = Intl.NumberFormat('de-DE', { style: 'currency', currency: 'EUR' }).format(price);
        td4.innerHTML = itemPrice;

        row.appendChild(td5 = document.createElement('td'));
        td5.id = "clear" + String(cartItem.itemId);
        td5.appendChild(clearButton);

        //Events for Quantity Inputs
        $(`#quantityInput${cartItem.itemId}`)
            .change((e) => {
                quantitychanged(e, quantityInput, cartItem);
            })
            .keypress((e) => { //enter should not refresh 
                if (e.which === 13) {
                    quantitychanged(e, quantityInput, cartItem);
                }
            });

        //Events for Clear Buttons
        $(`#clearButton${cartItem.itemId}`).click(() => {
            clear(cartItem.itemId);
        });

        //Event for ClearAllButton
        $(`#clearAllButton`).click(() => {
            $(`.clearbutton`).trigger('click');
        });
    };

    let quantitychanged = (e, quantityInput, cartItem) => {
        e.preventDefault();
        if (!quantityInput.checkValidity()) {
            $("#error").fadeIn("slow");
            document.getElementById("error").innerHTML = quantityInput.validationMessage;
            e.target.value = cartItem.quantinty;
            $("#error").fadeOut(5000);
        }
        else {
            document.getElementById("error").innerHTML = "";
            cartItem.quantinty = Number(e.target.value);
            updateQuantity(cartItem);
        }
    };

    let makeFooter = () => {
        let footer = document.getElementById("cartFoot");

        footer.appendChild(row1 = document.createElement('tr')); //create new row Total
        row1.id = "Total";
        row1.appendChild(td13 = document.createElement('td'));
        row1.appendChild(td45 = document.createElement('td'));
        td13.innerHTML = "Total :";
        td13.colSpan = 3;
        td45.colSpan = 2;
        td45.id = "totalPrice";

        footer.appendChild(row2 = document.createElement('tr')); //create new row Total
        row2.id = "TotalVAT";
        row2.style = "font-weight:bold;";
        row2.appendChild(td13 = document.createElement('td'));
        row2.appendChild(td45 = document.createElement('td'));
        td13.innerHTML = "Total (23% VAT) :";
        td13.colSpan = 3;
        td45.colSpan = 2;
        td45.id = "totalVATprice";

        getTotals();
    };

    let getTotals = () => {
        let sum = 0;
        $(".itemPrice").each((index, element) => {
            if ($(element).innerHTML !== "") {
                let str = element.innerText;
                str = str.substring(0, str.length - 2).replace(",", ".");
                let p = Number(str);
                sum = sum + p;
            }
        });
        total = new Intl.NumberFormat('de-DE', { style: 'currency', currency: 'EUR' }).format(sum);
        totalVAT = new Intl.NumberFormat('de-DE', { style: 'currency', currency: 'EUR' }).format(sum * 1.23);

        let totalcell = document.getElementById("totalPrice");
        let totalVATcell = document.getElementById("totalVATprice");

        totalcell.innerText = total;
        totalVATcell.innerText = totalVAT;

        let numberOfItems = 0;
        $(".itemQuantity").each((index, element) => {            
                let strQ = element.value;                
                let pQ = Number(strQ);
                numberOfItems += pQ;
        });

        let cartSummary = String(numberOfItems) + " item(s) " + String(totalVAT);
        $("span.refreshCart").each((index, element) => {
            element.innerText = cartSummary;
        });            
    };

    let addButtons = () => {
        let targ = document.getElementById("cart");

        // Add CheckBox Method of Payment Always cash on delivery
        let divPayment = document.createElement('div');
        divPayment.style = "text-align: right;";
        targ.appendChild(divPayment);
        let checkbox = document.createElement('input');
        checkbox.type = "checkbox";
        checkbox.name = "payment";
        checkbox.value = "delivery";
        checkbox.id = "payOnDelivery";
        checkbox.checked = true;       
        let lbl = document.createElement('label');
        lbl.innerText = " Payment on delivery";
        lbl.style = "padding: 5px;"; 

        divPayment.appendChild(checkbox);
        divPayment.appendChild(lbl);     

        $('#payOnDelivery').click((e) => {
            e.preventDefault();
        })

        // Add Buttons
        let div = document.createElement('div');
        div.id = "buttons";
        targ.appendChild(div);

        //create and add buttons to doc
        let backBtn = document.createElement('input');
        backBtn.type = "button";
        backBtn.id = "backbtn";
        backBtn.value = "Go Back";
        div.appendChild(backBtn);

        let buyBtn = document.createElement('input');
        buyBtn.type = "button";
        buyBtn.id = "buyBtn";
        buyBtn.classList.add("ml-3");//bootstrap class for left margin
        buyBtn.value = "Continue";
        buyBtn.style = "float: right;";
        div.appendChild(buyBtn);

        let saveBtn = document.createElement('input');
        saveBtn.type = "button";
        saveBtn.id = "savebtn";
        saveBtn.value = "Save Cart & Log Out";
        saveBtn.style = "float: right;";
        div.appendChild(saveBtn);

        //add events
        $(`#buyBtn`).click(() => {
            window.location.href = "/Cart/CheckOut";
        });
        $(`#savebtn`).click(() => {
            window.location.href = "/Cart/SaveLogout";
        });
        $(`#backbtn`).click(() => {
            //window.history.back();
            let ex = document.referrer;
            if (ex.endsWith("/Cart/CheckOut")) {
                window.location.href = "/Products/1";
            }
            else {
                window.location.href = ex;
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
                    let targ = document.getElementById("validation");
                    let al = document.createElement('div');
                    al.innerHTML = "The Cart is empty..";
                    targ.appendChild(al);
                }
                else {
                    makeTable(); //mainly header of table
                    data.forEach(x => addTableRow(x)); //make body
                    makeFooter(); // make footer of table
                    addButtons(); // add buttons
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
                getTotals();

                saveCartToDB();
            }
        });
    };

    let clear = (itemId) => {
        $.ajax({
            url: "api/Cart",
            contentType: "application/json",
            method: "PUT",
            data: JSON.stringify(itemId),
            success: (data) => {
                let child = document.getElementById("row" + itemId);
                if (child !== null)
                    child.parentElement.removeChild(child);
                getTotals();
                if (document.getElementById('CartBody').childElementCount === 0) {
                    let table = document.getElementById('cartTable');
                    table.parentElement.removeChild(table);
                    let div = document.getElementById("buttons");
                    div.parentElement.removeChild(div);
                    let targ = document.getElementById("validation");
                    let al = document.createElement('div');
                    al.innerHTML = "The Cart is empty..";
                    targ.appendChild(al);

                    saveCartToDB();
                }
            }
        });

    };

    let saveCartToDB = () => {
        $.ajax({
            url: "/api/SaveCart",
            contentType: "application/json",
            method: "POST",
            success: console.log("Cart saved")
        });
    };

    getCartItems();

    //$(window)
    //    .on("beforeunload", saveCartToDB())
    //    .on("pagehide", saveCartToDB())
    //    .on("unload", saveCartToDB());
});

