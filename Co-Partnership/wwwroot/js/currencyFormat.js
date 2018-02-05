$(document).ready(() => {
    let formatCurrency = (number) => {
        return formated = new Intl.NumberFormat('de-DE', { style: 'currency', currency: 'EUR' }).format(number);
    };

    $('.currency').each((index, element) => {
        element.innerText = formatCurrency(element.innerText);
    }); 
});