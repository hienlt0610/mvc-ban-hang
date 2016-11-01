//chỉ cho phép nhập số
$('.number').keypress(function (event) {
    if (event.which < 46 || event.which >= 58 || event.which == 47) {
        event.preventDefault();
    }

    if (event.which == 46 && $(this).val().indexOf('.') != -1) {
        this.value = '';
    }
});

$.fn.removeClassRegex = function (name) {
    return this.removeClass(function (index, css) {
        return (css.match(new RegExp('\\b(' + name + ')\\b', 'g')) || []).join(' ');
    });
};

function addThousandsSeparator(input) {
    var output = input
    if (parseFloat(input)) {
        input = new String(input); // so you can perform string operations
        var parts = input.split("."); // remove the decimal part
        parts[0] = parts[0].split("").reverse().join("").replace(/(\d{3})(?!$)/g, "$1,").split("").reverse().join("");
        output = parts.join(".");
    }
    return output;
}

//Tắt focus vào button khi click
$(".btn").click(function (event) {
    // Removes focus of the button.
    $(this).blur();
});