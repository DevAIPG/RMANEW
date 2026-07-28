function DismissRmaModal() {
    $('#meditrma').modal('hide');
}
function OpenCustomerModal() {
    $("#meditrma").one("hidden.bs.modal", function () {
        $("#CustomerModal").modal("show");
    });
    $("#CustomerModal").one("hidden.bs.modal", function () {
        $("#meditrma").modal("show");
    });

    $("#meditrma").modal("hide");
}

function SetCustomerModalResult(customerId) {
    if (customerId && customerId.toString().trim() !== '') {

        $('#inputCustomer')
            .val(customerId)
            .trigger('input');
    }
}
function SearchCustomer(clientId) {

    if (!clientId || clientId.trim() === "") {

        $("#inputCustomer")
            .removeClass("is-valid")
            .addClass("is-invalid");

        ClearContactInfomration();
        $(".contact-info").hide();

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

            const exactMatches =
                data.result.filter(x => x === clientId);

            if (exactMatches.length === 1) {

                $("#inputCustomer")
                    .removeClass("is-invalid")
                    .addClass("is-valid");

                LoadCustomerDetails(clientId);
                LoadInvoiceTable(clientId);
            }
            else {

                $("#inputCustomer")
                    .removeClass("is-valid")
                    .addClass("is-invalid");

                ClearContactInfomration();
                $(".contact-info").hide();
            }
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
            $("#inputCustomer")
                .removeClass("is-invalid")
                .addClass("is-valid");

            InitializeShipToTable(data);

            if (data.length === 1) {

                const row = data[0];

                $("#inputContact").val(row.contact_1);
                $("#inputShipTo").val(row.cus_alt_adr_cd);
                $("#inputContactEmail").val(row.email_address);
                $("#inputCompanyEmail").val(row.cmp_e_mail);
                $("#inputPhone").val(row.phone_no);
                $("#inputExtension").val(row.phone_ext);
                $("#inputFax").val(row.fax_no);

                $(".contact-info").show();
            }
            else {

                if (!preserveValues) {
                    ClearContactInfomration();
                    $(".contact-info").hide();
                }
            }
        },

        error: function (xhr) {

            $("#inputCustomer")
                .removeClass("is-valid")
                .addClass("is-invalid");

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

    $("#meditrma").one("hidden.bs.modal", function () {
        $("#ShipToModal").modal("show");
    });

    $("#ShipToModal").one("hidden.bs.modal", function () {
        $("#meditrma").modal("show");
    });

    $("#meditrma").modal("hide");
}
function SetShipToModalResult(data) {
    $("#inputContact").val(data.contact_1);
    $("#inputShipTo").val(data.cus_alt_adr_cd);
    $("#inputContactEmail").val(data.email_address);
    $("#inputCompanyEmail").val(data.cmp_e_mail);
    $("#inputPhone").val(data.phone_no);
    $("#inputExtension").val(data.phone_ext);
    $("#inputFax").val(data.fax_no);

    $(".contact-info").show();

    $("#ShipToModal").modal("hide");
}
function OpenInvoiceModal() {
    $("#meditrma").one("hidden.bs.modal", function () {
        $("#InvoiceModal").modal("show");
    });

    $("#InvoiceModal").one("hidden.bs.modal", function () {
        $("#meditrma").modal("show");
    });

    $("#meditrma").modal("hide");
}
function OpenSequenceModal() {

}
function OpenPartNumberModal() {
    "rma"
}
function OpenReturnCodeModal() {

}
$(document).ready(function () {
    LoadCustomerTable();
});
$(document).on("input", "#inputCustomer", function () {

    const customerId = $(this).val();

    if (!customerId || customerId.trim() === "") {
        $("#inputCustomer")
            .removeClass("is-valid")
            .addClass("is-invalid");

        ClearContactInfomration();
        $(".contact-info").hide();

        return;
    }

    SearchCustomer(customerId);
});
$(document).on("change", "#inputCustomer", function () {
    const customerId = $(this).val();
    if (!customerId || customerId.trim() === "") {

        $("#inputCustomer").addClass("is-invalid");

        ClearContactInfomration();

        $(".contact-info").hide();

        return;
    }

    const exists = $("#customerOptions option").filter(function () {
        return this.value === customerId;
    }).length > 0;

    if (exists) {
        $("#inputCustomer")
            .removeClass("is-invalid")
            .addClass("is-valid");
    }
    else {
        $("#inputCustomer")
            .removeClass("is-valid")
            .addClass("is-invalid");

        ClearContactInfomration();
        $(".contact-info").hide();
    }
});
$(document).on('click', '.shipto-select', function () {
    const row = $("#ShipToTable")
        .DataTable()
        .row($(this).closest('tr'))
        .data();
    SetShipToModalResult(row);
});
$(document).on('click', '.btn-search-invoice', function () {
    const customerId = $("#inputCustomer").val();
    const row = $("#LineTable")
        .DataTable()
        .row($(this).closest('tr'))
        .data();

    OpenInvoiceModal();
});
function InitializeShipToTable(source) {
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
$(document).on("click", ".btn-add-rma-line", function () {

    let index = $("#GeneratedRmaTable tbody tr").length;

    $.get("/Client/Rma/AddLineRow", function (html) {

        html = html.replace(/__INDEX__/g, index);

        $("#GeneratedRmaTable tbody").append(html);
    });
});
$(document).on("click", ".btn-remove-rma-line", function () {
    $(this).closest("tr").remove();
    reindexRows();
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