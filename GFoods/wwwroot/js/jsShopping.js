$(document).ready(function () {
    //ShowCount()
    $('body').on('click', '.btnAddToCart', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var Quantity = 1;
        var tQuantity = $('#quantity_value').text();
        if (tQuantity != '') {
            Quantity = parseInt(tQuantity);
        }
        var token = $('input[name="__RequestVerificationToken"]').val();
        $.ajax({
            url: '/customer/Products/AddToCart',
            type: 'POST',
            data: {
                ProductId: id,
                Count: Quantity,
                __RequestVerificationToken: token
            },
            success: function (response) {
                if (response.success) {
                    toastr.success(response.message);
                }
            },

        })
    })
   
});





