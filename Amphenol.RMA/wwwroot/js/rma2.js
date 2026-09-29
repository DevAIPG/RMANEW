var dataTable;
$(document).ready(function () {
    cargarDatatableRma2();
});
function Customer_complaint(data) {
    $("#Customer_complaintcontrol").html(data);
}
$(document).on("click", "#btnviewlines", function () {
    console.log("Here");
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

$(document).on(
    "submit",
    "#frmaprobar, #frmrechazo",
    function (event) {
        const form = this;
        const $form = $(form);
        const $button = $form.find('button[type="submit"]');

        // Block repeated submissions
        if ($form.data("submitted")) {
            event.preventDefault();
            return false;
        }

        // Preserve browser validation
        if (!form.checkValidity()) {
            return;
        }

        // Preserve jQuery unobtrusive validation, if configured
        if ($form.data("validator") && !$form.valid()) {
            return;
        }

        $form.data("submitted", true);
        $button.prop("disabled", true);

        if (form.id === "frmaprobar") {
            $button.html(
                '<span class="spinner-border spinner-border-sm mr-1"></span>' +
                "Approving..."
            );
        } else {
            $button.html(
                '<span class="spinner-border spinner-border-sm mr-1"></span>' +
                "Rejecting..."
            );
        }
    }
);