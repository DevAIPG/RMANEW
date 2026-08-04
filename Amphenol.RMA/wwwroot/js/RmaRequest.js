$(document).ready(function () {
    LoadCustomerTable();
});


$(document).on("click", ".btn-add-rma-line", function () {

    let index = $("#LineTable tbody tr").length;

    $.get("/Client/Rma/AddLineRow", function (html) {

        $("#emptyLineRow").remove();

        html = html.replace(/__INDEX__/g, index);

        $("#LineTable tbody").append(html);
    });
});
$(document).on("click", ".btn-remove-rma-line", function () {
    $(this).closest("tr").remove();
    reindexRows();
    if ($("#LineTable tbody tr").length === 0) {
        $("#LineTable tbody").append(`
            <tr id="emptyLineRow">
                <td colspan="11" class="text-center text-muted py-4">
                    <div class="d-flex justify-content-center align-items-center">
                        <h5 class="font-weight-bold mb-0 mr-3">Add new lines</h5>
                        <button type="button" class="btn btn-sm btn-primary action-btn btn-add-rma-line">
                            <i class="fa fa-plus"></i>
                        </button>
                    </div>
                </td>
           </tr>
    `);
    }
});
function reindexRows() {
    $("#GeneratedRmaTable tbody tr").each(function (index) {
        $(this).find("input, select").each(function () {
            let name = $(this).attr("name");
            if (name) {
                name = name.replace(
                    /Data\.ItemLines\[\d+\]/,
                    `Data.ItemLines[${index}]`
                );

                $(this).attr("name", name);
            }
        });
    });
}
function LoadCustomerTable() {
    $("#CustomerTable").DataTable({
        autoWidth: false,
        ajax: {
            url: "/client/corrective/GetAllA",
            type: "GET",
            dataType: "json"
        },
        columns: [
            {
                data: "cus_no",
                width: "80px",
                render: function (data, type, row) {
                    return `<a data-dismiss='modal'
                           class='link-opacity-100-hover'
                           onClick="SetCustomerModalResult(${row.cus_no})">
                           ${row.cus_no}
                        </a>`;
                }
            },
            { data: "cus_name", width: "200px" },
            { data: "curr_cd", width: "70px" },
            { data: "rateExchange", width: "90px" }
        ]
    });
}
function SetCustomerModalResult(selectedCustomerNumber) {
    var table = $("#CustomerTable").DataTable();
    table.search('').draw();
    $("#inputCustomer").val(selectedCustomerNumber);
    $("#CustomerModal").modal("hide");
}
$(document).on("input", "#inputCustomer", function () {
    const customerId = $(this).val();

    if (!customerId || customerId.trim() === "") {
        $("#inputCustomer").removeClass("is-valid").addClass("is-invalid");

        ClearContactInfomration();
        $(".customer-info").addClass("d-none");

        return;
    }

    SearchCustomer(customerId);
});
function SearchCustomer(clientId) {

    if (!clientId || clientId.trim() === "") {

        $("#inputCustomer").removeClass("is-valid").addClass("is-invalid");

        $(".shipto-section").addClass("d-none");

        ClearContactInfomration();

        $(".customer-info").addClass("d-none");

        return;
    }

    $.ajax({
        url: "/client/corrective/Getclientscodigo",
        type: "GET",
        dataType: "json",
        data: {
            term: clientId
        },
        success: function (data) {

            const datalist = $("#customerOptions");
            datalist.empty();

            $.each(data.result, function (index, item) {
                datalist.append(
                    `<option value="${item}" label="${item}"></option>`
                );
            });

            const exactMatches = data.result.filter(x => x === clientId);

            if (exactMatches.length === 1) {

                $("#inputCustomer").removeClass("is-invalid").addClass("is-valid");

                LoadCustomerDetails(clientId);
                LoadInvoiceTable(clientId);
                $(".shipto-section").removeClass("d-none");
            }
            else {

                $("#inputCustomer").removeClass("is-valid").addClass("is-invalid");
                $(".shipto-section").addClass("d-none");

                ClearContactInfomration();
                $(".customer-info").addClass("d-none");
            }
            $.get(
                "/client/corrective/GetAllSt/?client=" + clientId,
                function (data) {
                    LoadShipToTable(data);
                }
            );
        }
    });
}
function LoadCustomerDetails(customerID, preserveValues = false) {

    $.ajax({
        url: "/client/corrective/GetAllSt/?client=" + customerID,
        type: "GET",
        dataType: "json",
        cache: true,
        async: true,

        success: function (data) {
            $("#inputCustomer").removeClass("is-invalid").addClass("is-valid");

            LoadShipToTable(data);

            if (data.length === 1) {

                const row = data[0];

                $("#inputContact").val(row.contact_1);
                $("#inputShipTo").val(row.cus_alt_adr_cd);
                $("#inputContactEmail").val(row.email_address);
                $("#inputCompanyEmail").val(row.cmp_e_mail);
                $("#inputPhone").val(row.phone_no);
                $("#inputExtension").val(row.phone_ext);
                $("#inputFax").val(row.fax_no);
                $(".customer-info").removeClass("d-none");
                $("#inputShipTo").trigger("input");
            }
            else {

                if (!preserveValues) {
                    ClearContactInfomration();
                    $(".customer-info").addClass("d-none");
                    $(".line-section").addClass("d-none");
                }
            }
        },

        error: function (xhr) {

            $("#inputCustomer").removeClass("is-valid").addClass("is-invalid");

            $().toastmessage("showToast", {
                text: "Error con ean " + xhr.responseText,
                sticky: true,
                type: "error"
            });
        }
    });
}
function ClearContactInfomration() {
    $("#inputContact").val("");
    $("#inputShipTo").val("");
    $("#inputContactEmail").val("");
    $("#inputCompanyEmail").val("");
    $("#inputPhone").val("");
    $("#inputExtension").val("");
}

function OpenShipToModal() {
    const customerId = $("#inputCustomer").val();

    if (!customerId ||
        $("#inputCustomer").hasClass("is-invalid")) {

        $().toastmessage("showToast", {
            text: "Please select a valid customer first.",
            sticky: false,
            type: "warning"
        });

        $("#inputCustomer")
            .removeClass("is-valid")
            .addClass("is-invalid");

        return;
    }

    $("#ShipToModal").modal("show");
}
function LoadShipToTable(source) {
    if ($.fn.DataTable.isDataTable("#ShipToTable")) {
        $("#ShipToTable").DataTable().destroy();
    }
    $("#ShipToTable tbody").empty();

    $("#ShipToTable").DataTable({
        "autoWidth": false,
        data: source,
        "columns": [{
            "data": "id",
            "render": function render(data, type, row) {
                return `<a class="link-opacity-100-hover shipto-select">${row.id}</a>`;
            }
        },
        {
            data: "contact_1"
        },
        {
            data: "cus_alt_adr_cd"
        }
        ],

        "language": {
            "emptyTable": "No records found"
        }
    });
}
$(document).on('click', '.shipto-select', function () {
    const row = $("#ShipToTable")
        .DataTable()
        .row($(this).closest('tr'))
        .data();
    SetShipToModalResult(row);
});
$(document).on('input', '#inputShipTo', function () {
    const shiptTo = $(this).val();

    if (!shiptTo || shiptTo.trim() === "") {
        ClearContactInfomration();
        $(".customer-info").addClass("d-none");
        $(".line-section").addClass("d-none");
        return;
    }

    $(".customer-info").removeClass("d-none");
    $(".line-section").removeClass("d-none");
});
function SetShipToModalResult(data) {
    var table = $("#ShipToTable").DataTable();
    table.search('').draw();
    $("#inputContact").val(data.contact_1);
    $("#inputShipTo").val(data.cus_alt_adr_cd);
    $("#inputContactEmail").val(data.email_address);
    $("#inputCompanyEmail").val(data.cmp_e_mail);
    $("#inputPhone").val(data.phone_no);
    $("#inputExtension").val(data.phone_ext);
    $("#inputFax").val(data.fax_no);

    $("#inputShipTo").trigger("focus");
    $("#inputShipTo").trigger("input");

    //$(".customer-info").removeClass("d-none");

    $("#ShipToModal").modal("hide");
}