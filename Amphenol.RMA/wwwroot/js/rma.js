var dataTable;
 
$(document).ready(function () {
    //$("#daterma").datepicker(); 
    cargarDatatableRma();

    $("#btnsubmitrma").click(function () {



         if ($("#pnnew").val() || $("#slocationsnew").children("option:selected").val() !=="0") {
             Swal.fire(
                 'Unsaved line',
                 'Unsaved line, first finish filling in the information then submit',
                 'info'
             )
         }
         else {

             let id = $("#rmaid").val();
             $.ajax({
                 url: "/Client/Rma/SubmitRMA/?id=" + id,
                 type: "GET",
                 dataType: "json",
                 async: true,
                 cache: true,
                 success: function (data) {
                     window.location.reload();
                 }
             });
         }




    });
});
function DownloadRmaFile(rmaId) {
    console.log(rmaId);
    if (rmaId === null || rmaId === undefined || rmaId === '') {
        console.error('An RMA number is required.');
        return;
    }

    const url =
        `/Client/rma/DownloadReport?rma=${encodeURIComponent(rmaId)}`;

    window.location.href = url;
}
function OnCreateOrderClicked(id) {
    $.ajax({
        "url": "/Client/rma/GetGeneratedOrderNumber",
        type: "POST",
        dataType: "json",
        data: { id: id },
        cache: true,
        async: true,
        success: function success(data) {
            if (!data.wasSuccessful) {
                $().toastmessage('showToast', {
                    text: data.message,
                    sticky: false,
                    type: 'error'
                });
                return;
            }

            const hasOrder = !!data.orderNumber?.trim();

            $("#createOrderTitle").text(
                hasOrder ? "Existing Order" : "Create Order"
            );

            $("#createOrderMessage").html(
                hasOrder
                    ? `Order #<b>${data.orderNumber}</b> has already been generated for this RMA Request`
                    : "Do you want to create a new re-shipment / credit order?"
            );

            const options = hasOrder
                ? `
                    <button class="btn btn-secondary me-3" data-bs-dismiss="modal">
                        OK
                    </button>
                  `
                : `
                    <button class="btn btn-secondary me-3" data-bs-dismiss="modal">
                        No
                    </button>
                    <button id="createorderyesbtn"
                            onclick="GenerateOrder(${id})"
                            data-bs-dismiss="modal"
                            class="btn btn-primary">
                        Yes
                    </button>
                  `;

            $("#createOrderFooter").html(options);
            $('#CreateOrderModel').modal("show");
        },
        error: function error(xhr, status, _error4) {
            $().toastmessage('showToast', {
                text: 'Error con ean ' + xhr.responseText,
                sticky: true,
                type: 'error'
            });
        }
    });
}

function GenerateOrder(id) {
    $('#tblQm').modal('hide');

    $.ajax({
        url: '/Client/rma/RMA_IdTo_OrderCreate',
        type: 'GET',
        data: { id: id },
        success: function (response) {

            var data = {
                rmaNo: response.rmaNo,
                orderDate: response.orderDate
            };

            $.ajax({
                url: '/Client/rma/CreateOrderpost',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(data),
                success: function (response) {

                    $('#NewOrderModal .modal-title').text("Order Created");
                    $('#NewOrderModal .modal-body').html(response.message);
                    $('#NewOrderModal').modal('show');

                    $('#NewOrderModal').one('hidden.bs.modal', function () {
                        location.reload();
                    });
                },
                error: function (err) {

                    console.error('Order creation error:', err);

                    $('#NewOrderModal .modal-title').text("Order Creation Failed");

                    if (err.responseJSON && err.responseJSON.message) {
                        $('#NewOrderModal .modal-body').html(err.responseJSON.message);
                    } else {
                        $('#NewOrderModal .modal-body').html('An unexpected error occurred.');
                    }

                    $('#NewOrderModal').modal('show');
                }
            });
        },
        error: function (xhr) {

            var err = xhr.responseJSON?.Error;

            console.error('Error fetching RMA data:', err);

            alert('Error: ' + err);
        }
    });
}