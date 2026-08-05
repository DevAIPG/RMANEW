var currentCustomerId;
let invoiceTable = null;
let sequenceTable = null;
let selectedLineRow = null;
let invoices = [];

$(document).ready(function () {
    LoadCustomerTable();
    LoadPartNumberTable();
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
    CalculateTotalRmaValue();
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
    currentCustomerId = selectedCustomerNumber;
    $("#inputCustomer").val(currentCustomerId);
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
                currentCustomerId = clientId;

                LoadCustomerDetails(clientId);
                LoadInvoiceTable();

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
    currentCustomerId = "";
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

    $("#ShipToModal").modal("hide");
}
function LoadInvoiceTable() {
    if ($.fn.DataTable.isDataTable("#InvoiceTable")) {
        $("#InvoiceTable").DataTable().destroy();

        $("#InvoiceTable tbody").empty();
    }
    invoiceTable = $("#InvoiceTable").DataTable({
        "autoWidth": false,
        "order": [[3, "desc"]],
        "ajax": {
            "url": "/client/corrective/GetAllI2/?filtro=" + currentCustomerId,
            "type": "GET",
            "dataType": "json"
        },
        "columns": [{
            "data": "invNo",
            width: "80px",
            "render": function render(data, type, row) {
                return `<a class='link-opacity-100-hover' onClick="SetInvoiceModalResult('${row.invNo}')">${row.invNo}</a>`;
            }
        },
        {
            width: "80px",
            data: "ordNo"
        },
        {
            width: "50px",
            data: "ordType"
        },
        {
            width: "80px",
            data: "ordDt",
            render: function (data) {
                return data ? new Date(data).toLocaleDateString('en-US') : '';
            }
        },
        {
            width: "200px",
            data: "cusAltAdrCd"
        },
        {
            width: "80px",
            data: "currTrxRt"
        },
        {
            width: "50px",
            data: "currCd"
        }],
        "language": {
            "emptyTable": "No records found"
        }
    });
}
$(document).on('click', '.btn-search-invoice', function () {
    selectedLineRow = $(this).closest('tr');
    $("#InvoiceModal").modal("show");
});
$(document).on('input', '.invoice-input', function () {
    selectedLineRow = $(this).closest('tr');
    if (!selectedLineRow) {
        return;
    }
    var invoiceInput = selectedLineRow.find('.invoice-input');
    var selectedInvoice = invoiceInput.val();

    if (!selectedInvoice || selectedInvoice.trim() == '') {
        invoiceInput.addClass('bg-warning');
    } else {
        invoiceInput.removeClass('bg-warning');
    }
    LoadSequenceTable(selectedInvoice);
});
function SetInvoiceModalResult(invoice) {
    var table = $("#InvoiceTable").DataTable();
    table.search('').draw();

    if (!selectedLineRow) {
        return;
    }

    var invoiceInput = selectedLineRow.find('.invoice-input');
    var sequenceInput = selectedLineRow.find('.sequence-input');
    var partnumberInput = selectedLineRow.find('.partnumber-input');
    var priceInput = selectedLineRow.find('.price-input');
    var unitcostInput = selectedLineRow.find('.unitcost-input');

    invoiceInput.val(invoice).trigger("input");
    sequenceInput.val("").trigger("input");
    partnumberInput.val("").trigger("input");
    priceInput.val("").trigger("input");
    unitcostInput.val("").trigger("input");

    $("#InvoiceModal").modal("hide");
}
function LoadSequenceTable(invoice) {
    if ($.fn.DataTable.isDataTable("#SequenceTable")) {
        $("#SequenceTable").DataTable().destroy();

        $("#SequenceTable tbody").empty();
    }
    $.ajax({
        url: "/client/corrective/getSecuencias/",
        type: "GET",
        data: { factura: invoice },
        dataType: "json",
        cache: true,
        async: true,
        success: function success(data) {
            sequenceTable = $("#SequenceTable").DataTable({
                destroy: true,
                data: data,
                columns: [
                    {
                        "data": "line_seq_no",
                        render: function (data, type, row) {
                            return `<a class='link-opacity-100-hover' onClick="SetSequenceModalResult('${row.line_seq_no}')">${row.line_seq_no}</a>`;
                        }
                    },
                    { "data": "ord_type" },
                    { "data": "ord_no" },
                    { "data": "item_no" }
                ]
            });
        },
        error: function error(xhr, status) {
        }
    });
}
$(document).on('click', '.btn-search-sequence', function () {
    selectedLineRow = $(this).closest('tr');
    var invoice = selectedLineRow.find('.invoice-input').val();
    if (!invoice || invoice.trim() == '') {
        return;
    }
    LoadSequenceTable(invoice);
    $("#SequenceModal").modal("show");
});
function SetSequenceModalResult(sequenceNumber) {
    var table = $("#SequenceTable").DataTable();
    table.search('').draw();

    if (!selectedLineRow) {
        return;
    }

    selectedLineRow.find('.sequence-input').val(sequenceNumber).trigger("input");
    var invoice = selectedLineRow.find('.invoice-input').val();
    SetValuesBasedOnInvoice(invoice);
    $("#SequenceModal").modal("hide");
}
$(document).on('input', '.sequence-input', function () {
    selectedLineRow = $(this).closest('tr');
    var sequenceInput = selectedLineRow.find('.sequence-input');
    var selectedSequence = sequenceInput.val();

    if (!selectedSequence || selectedSequence.trim() == '') {
        sequenceInput.addClass('bg-warning');
    } else {
        sequenceInput.removeClass('bg-warning');
    }
});
function SetValuesBasedOnInvoice(invoice) {
    console.log(invoice)
    if (!invoice || invoice.trim() == '') {
        return;
    }
    $.ajax({
        url: "/Client/Corrective/GetPricesByInvoice",
        type: "GET",
        data: {
            invoicenumber: invoice,
            loc: "D"
        },
        dataType: "json",
        async: true,
        cache: true,
        success: function (data) {
            console.log(data);
            if (data[0] !== undefined) {

                //$("#msgloc" + linea).fadeOut();
                //$("#creer").prop("disabled", false);
                //$(".price" + linea).val(parseFloat(data[0].price).toFixed(2));
                //$("#cost" + linea).val(parseFloat(data[0].std));
                var unitPrice = parseFloat(data[0].price).toFixed(2);
                var unitCost = parseFloat(data[0].std);

                var unitPriceInput = selectedLineRow.find(".price-input");
                var unitCostInput = selectedLineRow.find(".unitcost-input");

                unitPriceInput.val(unitPrice);
                unitCostInput.val(unitCost);

                unitPriceInput.trigger("input");
                unitCostInput.trigger("input");

                invoices.push(data[0].loc);
            }
        }
    });
}
function LoadPartNumberTable() {
    $.ajax({
        url: "/client/corrective/GetPartNumbers/",
        type: "GET",
        dataType: "json",
        cache: true,
        async: true,
        success: function success(data) {
            var table = $("#PartNumberTable").DataTable({
                destroy: true,
                deferRender: true,
                data: data.data,
                columns: [
                    {
                        "data": "id",
                        "width": "80px",
                        render: function (data, type, row) {
                            return `<a class='link-opacity-100-hover' onClick="SetPartNumberModalResult('${row.item_no}')">${row.id}</a>`;
                        }
                    },
                    {
                        "data": "item_no",
                        "width": "100px",
                    },
                    {
                        "data": "item_desc_1",
                        "width": "100px",
                    }
                ]
            });
        },
        error: function error(xhr, status) {
        }
    });
}
$(document).on('click', '.btn-search-partnumber', function () {
    selectedLineRow = $(this).closest('tr');
    var invoice = selectedLineRow.find('.invoice-input').val();
    var sequence = selectedLineRow.find('.sequence-input').val();
    if ((!invoice || invoice.trim() == '') || (!sequence || sequence.trim() == '')) {
        return;
    }
    $("#PartNumberModal").modal("show");
});
function SetPartNumberModalResult(partnumber) {
    var table = $("#PartNumberTable").DataTable();
    table.search('').draw();

    if (!selectedLineRow) {
        return;
    }

    selectedLineRow.find('.partnumber-input').val(partnumber).trigger("input");

    $("#PartNumberModal").modal("hide");
}
$(document).on('input', '.partnumber-input', function () {
    selectedLineRow = $(this).closest('tr');
    var partnumberInput = selectedLineRow.find('.partnumber-input');

    var partnumber = partnumberInput.val().trim();

    if (partnumber === '') {
        partnumberInput.addClass('bg-warning');
    } else {
        partnumberInput.removeClass('bg-warning');
    }

});
$(document).on('input', '.quantity-input', function () {
    selectedLineRow = $(this).closest('tr');
    var quantityInput = selectedLineRow.find('.quantity-input');
    var quantity = quantityInput.val().trim();

    if (quantity === '' || parseInt(quantity) === 0) {
        quantityInput.addClass('bg-warning');
        return;
    }

    quantityInput.removeClass('bg-warning');
    CalculateTotalRmaValue();
});
$(document).on('input', '.price-input, .unitcost-input', function () {
    selectedLineRow = $(this).closest('tr');

    var unitPriceInput = selectedLineRow.find('.price-input');
    var unitCostInput = selectedLineRow.find('.unitcost-input');

    var unitPrice = parseFloat(unitPriceInput.val());
    var unitCost = parseFloat(unitCostInput.val());

    if (isNaN(unitPrice) || isNaN(unitCost)) {
        return;
    }

    var unitPrice = parseFloat(unitPriceInput.val()).toFixed(2);
    var unitCost = parseFloat(unitCostInput.val());

    if (unitPrice > unitCost) {
        unitPriceInput.removeClass("bg-warning");
        unitCostInput.removeClass("bg-warning");
        CalculateTotalRmaValue();
    }
    else {
        unitPriceInput.addClass("bg-warning")

        Swal.fire({
            title: 'Price must be higher than unit cost. Do you want to continue?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#d33',
            cancelButtonColor: '#95a5a6',
            confirmButtonText: 'Yes',
            cancelButtonText: 'No'
        });
    }
});
function CalculateTotalRmaValue() {
    var total = 0;

    $('#LineTable tbody tr').each(function () {
        var quantity = parseInt($(this).find('.quantity-input').val()) || 0;
        var price = parseFloat($(this).find('.price-input').val()) || 0;

        total += quantity * price;
    });

    $('.totalrmavalue-label').text(total.toFixed(2));
}
function LoadReturnCodeTable() {

}