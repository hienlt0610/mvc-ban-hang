//chỉ cho phép nhập số
$('.number').keypress(function (event) {
    if (event.which < 46 || event.which >= 58 || event.which == 47) {
        event.preventDefault();
    }

    if (event.which == 46 && $(this).val().indexOf('.') != -1) {
        this.value = '';
    }
});