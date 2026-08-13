
var dataTable;
$(document).ready(function () {
    cargarDatatableRma2();
});
function Customer_complaint(data) {
    $("#Customer_complaintcontrol").html(data);
}
$(document).on("click", "#btnviewlines", function () {
    let id = $(this).data("id");
    $.ajax({
        url: '/Client/Rma/RmaModal',
        type: 'GET',
        data: {
            id: id,
            mode: 'view'
        },
        success: function (html) {
            $('#viewRmaModal .modal-content').html(html);

            $('#viewRmaModal').modal('show');
        },
        error: function (xhr) {
            alert('Unable to load RMA details.');
        }
    });
});