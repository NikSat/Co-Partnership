let selectChanged = (currentValue) => {
    let submit = document.getElementById("productSubmit")

    if (currentValue !== "start") {
        if (submit.hasAttribute('disabled')) {
            submit.removeAttribute('disabled');
        }
    }
    else {
        if (!submit.hasAttribute('disabled')) {
            submit.addAttribute('disabled');
        }
    }
    console.log("selectChanged");
};

let quantityChanged = (quantity) => {

    let str = document.getElementById("UnitPrice").innerText;
    let price = str.substring(0, str.length - 2).replace(",", ".");  
    
    let totalPrice = Number(quantity) * Number(price);
    if (totalPrice < 0) {
        totalPrice = 0;
    }

    let total = document.getElementById("TotalPrice");
    total.innerText = new Intl.NumberFormat('de-DE', { style: 'currency', currency: 'EUR' }).format(totalPrice);
};

let preventSubmit = (e) => {
    if (e.which === 13) {
        e.preventDefault();
    }
};