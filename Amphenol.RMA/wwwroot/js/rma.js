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
                    <button class="btn btn-secondary me-3" data-dismiss="modal">
                        OK
                    </button>
                  `
                : `
                    <button class="btn btn-secondary me-3" data-dismiss="modal">
                        No
                    </button>
                    <button id="createorderyesbtn"
                            onclick="CreateorderyesbtnF(${id})"
                            data-dismiss="modal"
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