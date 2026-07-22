var persona = "";
var dataTable;
var arrlinea = [];
var arrRmaId = [];
var arrRCode = [];
$.ajax({
    "url": "/Client/corrective/GetAllpersona",
    type: "GET",
    dataType: "json",
    cache: true,
    async: true,
    success: function success(data) {
        console.log(data);
        persona = data.data;
        console.log(persona);
    },
    error: function error(xhr, status, _error4) {
        $().toastmessage('showToast', {
            text: 'Error con ean ' + xhr.responseText,
            sticky: true,
            type: 'error'
        });
    }
});
var usuario = "";
var usuario_car = "";

$.ajax({
    url: "/Client/Tasks/GetAlluserlogin",
    type: "GET",
    dataType: "json",
    cache: true,
    async: true,
    success: function success(data) {
        console.log(data);
        usuario = data.nombre;
        usuario_car = data.fname;
    },
    error: function error(xhr, status, _error2) {
        $().toastmessage('showToast', {
            text: 'Error con ean ' + xhr.responseText,
            sticky: true,
            type: 'error'
        });
    }
});
//RMA
function cargarDatatableRma() {
    $.ajax({
        url: "/Client/rma/GetAllrma",
        type: "GET",
        dataType: "json",
        async: true,
        cache: true,
        success: function (data) {
            var username = parseInt(data.resid);
            dataTable = $("#GeneratedRmaTable").DataTable({
                destroy: true,
                autoWidth: false,
                scrollX: true,
                pageLength: 10,
                lengthMenu: [10, 25, 50, 100],
                "order": [[1, "desc"]],
                data: data.data,
                columns: [
                    {
                        "data": "id",
                        "width": "120px",
                        "render": function render(data, type, row) {
                            const complaintButton = `
                                    <a class="btn btn-light btn-sm action-btn d-flex align-items-center gap-1" data-toggle="modal" data-target="#Customer_complaint"
                                            onclick="Customer_complaint('${row.customercomplait}')" data-bs-toggle="tooltip" data-bs-placement="right" title="View Customer Complaint">
                                        <i class="fa fa-comments"></i>
                                     </a>`;
                            if (row.status != "Approved" && parseInt(row.res_id) == username) {
                                const editButton = `
                                    <a class="btn btn-light btn-sm action-btn d-flex align-items-center justify-content-center" id="btneditlines"
                                              data-id="${row.id}" data-bs-toggle="tooltip" data-bs-placement="right" title="Edit RMA">
                                        <i class="fa fa-pen"></i>
                                    </a>`;
                                if (row.status == "Rejected") {
                                    return `
                                        <div class="d-flex gap-1">
                                            ${editButton}
                                            ${complaintButton}
                                            <a class="btn btn-primary btn-sm  d-flex align-items-center justify-content-center" id="btnresubmit"
                                                      data-id="${row.id}" data-bs-toggle="tooltip" data-bs-placement="right" title=" Re-submit RMA" >
                                                <i class="fa fa-redo"></i>
                                            </a>
                                        </div>`;
                                }
                                else {
                                    if (row.status == "Remark") {
                                        return `
                                            <div class="d-flex gap-1">
                                             ${editButton}
                                             ${complaintButton}
                                            <a class="btn btn-primary btn-sm d-flex align-items-center justify-content-center gap-1" 
                                               id="btndone" data-id="${row.id}">
                                              <i class="fa fa-retweet"></i> Done
                                            </a>
                                          </div>`;
                                    }

                                    return `
                                    <div class="d-flex gap-1">
                                        ${editButton}
                                        ${complaintButton}
                                    </div>`;
                                }
                            }
                            else {
                                const viewButton = `
                                    <a class="btn btn-light btn-sm action-btn d-flex align-items-center" id="btnviewlines" data-id="${row.id}" data-bs-toggle="tooltip" data-bs-placement="right" title="View RMA">
                                        <i class="fa fa-eye"></i>
                                    </a>`;

                                if (row.sumbit === "Submitted" && row.status === "Approved") {

                                    const hasOrder = !!row.orderNumber?.trim();

                                    const generateOrderButtonColor = !row.canGenerateOrder
                                        ? 'btn-warning'
                                        : hasOrder ? 'btn-success' : 'btn-primary';

                                    const generateOrderButtonModal = row.canGenerateOrder ? `OnClick="OnCreateOrderClicked(${row.id})"` : '';

                                    const generateOrderButtonTitle = !row.canGenerateOrder
                                        ? 'Awaiting returned products'
                                        : hasOrder ? `Order #${row.orderNumber} Generated` : 'Generate New Order';

                                    return `
                                    <div class="d-flex gap-1">
                                        ${viewButton}
                                        ${complaintButton}
                                        <a class="btn ${generateOrderButtonColor} btn-sm action-btn d-flex align-items-center justify-content-center gap-1" id="btncreateorder"
                                            ${generateOrderButtonModal} data-id="${row.id}" data-bs-toggle="tooltip" data-bs-placement="right" title="${generateOrderButtonTitle}">
                                            <i class="fa fa-file-invoice"></i>
                                        </a>
                                    </div>`;
                                }

                                return `
                                <div class="d-flex gap-1">
                                    ${viewButton}
                                    ${complaintButton}
                                </div>`;
                            }
                        }
                    },
                    {
                        "width": "110px",
                        "data": "rmarequest"
                    },
                    {
                        "width": "110px",
                        "data": "turno",
                    },
                    {
                        "width": "130px",
                        "data": "preparado"
                    },
                    {
                        "width": "110px",
                        "data": "date"
                    },
                    {
                        "width": "170px",
                        "data": "sumbit",
                        render: function (data, type, row) {
                            if (data == "Submitted") {
                                return `<span class="badge bg-success text-white px-2 py-1" style="font-size: 0.85rem; border-radius: 0.25rem;">Submitted</span>`;
                            }
                            else {
                                return `<span class="badge bg-secondary text-white px-2 py-1" style="font-size: 0.85rem; border-radius: 0.25rem;">Not Submitted</span>`;
                            }
                        }
                    },
                    {
                        "width": "170px",
                        "data": "status",
                        render: function (data, type, row) {
                            if (data == "Approved") {
                                return `<span class="badge bg-success text-white px-2 py-1" style="font-size: 0.85rem; border-radius: 0.25rem;">Approved</span>`;
                            }
                            if (data == "Pending") {
                                return `<span class="badge bg-warning text-dark px-2 py-1" style="font-size: 0.85rem; border-radius: 0.25rem;">Pending</span>`;

                            }
                            else {
                                if (data == "Rejected") {
                                    return `<span class="badge bg-danger text-white px-2 py-1" style="font-size: 0.85rem; border-radius: 0.25rem;">Rejected</span>`;
                                } else {
                                    if (data == "Remark") {
                                        return `<span class="badge bg-primary text-white px-2 py-1" style="font-size: 0.85rem; border-radius: 0.25rem;">Remark</span>`;
                                    }
                                    else {
                                        return `<span class="badge bg-secondary text-white px-2 py-1" style="font-size: 0.85rem; border-radius: 0.25rem;">Created</span>`;
                                    }
                                }
                            }
                        }
                    },
                    {
                        "width": "200px",
                        "data": "customerpartno"
                    },
                    {
                        "width": "200px",
                        "data": "description"
                    },
                    {
                        "width": "100px",
                        "data": "qty"
                    },
                    {
                        "width": "200px",
                        "data": "rmatypeofrequest"
                    },
                    {
                        "width": "120px",
                        "data": "totalrmavalues",
                        render: function (data) {

                            return data.toFixed(4);
                        }
                    },
                    {
                        "width": "100px",
                        "data": "wherebuilt"
                    },
                    {
                        "width": "130px",
                        "data": "approver"
                    },
                    {
                        "width": "140px",
                        "data": "customerpo"
                    },
                    {
                        "width": "110px",
                        "data": "parts",
                    },
                    {
                        "width": "150px",
                        "data": "formated_date_approved"
                    }
                ],
                drawCallback: function () {
                    $('[data-bs-toggle="tooltip"]').each(function () {
                        new bootstrap.Tooltip(this);
                    });
                },
                "language": {
                    "emptyTable": "No records found"
                }
            });

            $(document).on("click", "#btnresubmit", function () {
                let id = $(this).data("id");
                $.ajax({
                    url: "/Client/Rma/resubmitrechazo?id=" + id,
                    type: "GET",
                    dataType: "json",
                    cache: true,
                    async: true,
                    success: function success(data) {
                        cargarDatatableRma();
                    }
                });
            });

            $("#btnclosemedit").click(function () {
                //#meditrma 
                //$("#meditrma").modal("hide");
                $('#meditrma').fadeOut();
                window.location.reload();
            });

            $(document).on("click", "#btndone", function () {
                let id = $(this).data("id");
                $.ajax({
                    url: "/Client/rma/DoneRemark",
                    dataType: "json",
                    type: "GET",
                    data: { id: id },
                    async: true,
                    cache: true,
                    success: function (result) {
                        if (result == true) {
                            cargarDatatableRma();
                        }
                    }

                })
            });

            //$(document).on("click", "#btncreateorder", function () {
            //    let id = $(this).data("id");

            //    $("#createorderyesbtn").attr("data-id", id);

            //    //console.log(id);
            //});


        }
    });

    function openinvoices() {
        const client = document.getElementById("client").value;
        $.ajax({
            url: "/client/corrective/GetAllI2/?filtro=" + client.trim(),
            type: "GET",
            datatype: "json",
            async: true,
            cache: true,

            success: function (data) {
                console.log("facturas");
                $("#tblinvoices").DataTable({
                    destroy: true,
                    data: data,
                    columns: [
                        {
                            data: 'invNo',
                            render: function (data, type, row) {
                                return '<a class="text-primary"  id="linkinvno" data-no=' + row.invNo + '>' + row.invNo + '</a>';
                            }
                        },
                        { data: 'ordNo' },
                        { data: 'cusNo' },
                        { data: 'ordType' },
                        { data: 'ordDt' },
                        { data: 'cusAltAdrCd' },
                        { data: 'currTrxRt' },
                        { data: 'currCd' }
                    ]
                });
                //console.log(data); 
            },
            error: function (xhr, status, error) {
                $().toastmessage("showToast", {
                    text: "Error con ean " + xhr.responseText,
                    sticky: true,
                    type: "error",
                });
            },
        });
        //linkinvno
        $(document).on("click", "#tblinvoices", function () {
            $("#abrir1").val($(this).data("no"));
            $("#minvoices").modal("hide");
        });
        $(document).on("click", "#linkinvno", function () {
            //alert($(this).text()); 
            $(".invoiceenew").val($(this).text());
            LoadSeq($(this).text());
            $("#minvoices").modal("hide");
            //LoadSecuencias($(this).text());

        });

    }
    function reloadLineas() {

        $.ajax({
            url: "/Client/Rma/GetRmaInfo/?id=" + $("#rmaid").val(),
            type: "GET",
            dataType: "json",
            async: true,
            cache: true,
            success: function (data) {


                var tableLineas = $("#tbllineas").DataTable({
                    destroy: true,
                    dom: "btpi",
                    paging: 5,
                    data: data.lines,
                    responsive: true,

                    columns: [

                        {
                            data: null,
                            render: function (data, type, row) {
                                return ` 
                                <div class="d-flex"> 
                                    <div class="custom-file">
                                        <input id="invoicee" readonly="readonly"  name="invoice" class="inv t invoice form-control form-control-sm float-left" value=${row.invoice} />
                                    </div> 
 
                                </div>`;
                            }
                        },
                        {
                            data: null,
                            render: function (data, type, row) {
                                return ` 
                                <div class="row">

<div class="col-12">

<div class="input-group d-flex">
                                    <div class="custom-file">
                                      <input readonly="readonly"   name="seq" class="t2 t seq form-control form-control-sm seq"  value=${row.seq}  /> 
                                    </div> 
                                </div>
</div>
</div>
`;
                            }
                        },
                        {
                            data: null,
                            render: function (data, type, row) {
                                return ` 
                                <div class="input-group d-flex">
                                    <div class="custom-file">
                                     <input onkeyup="javascript:this.value=this.value.toUpperCase();"  name="coustumer" readonly   type="text" class="form-control form-control-sm cos t  coustumer" value=${row.coustumer} id="inputGroupFile02">
                                    </div> 
                                </div>`;
                            }
                        },
                        {
                            data: "action",
                            render: function (data, type, row) {

                                if (data !== "C") {
                                    return "<select class='form-control form-control-sm action-" + row.id + "' id='action' disabled> <option value=" + data + ">" + data + "</option> <option value='C'>C</option> </select>";

                                } else {
                                    return "<select class='form-control form-control-sm action-" + row.id + "'   id='action' disabled> <option value=" + data + ">" + data + "</option> </option> <option value='R'>R</option> </select>";

                                }

                            }
                        },
                        {
                            data: "loc",
                            render: function (data, type, row) {


                                return `<select disabled onchange='cambioloc(this);' data-id='${row.id}' data-loc='${data}'  id='slocations-${row.id}'  class="loc form-control form-control-sm locationsadd" name="loc">
                                    <option value=${data}>${data}</option>
                                    <option value="ARM">ARM</option>
                                    <option value="ERN">ERN</option>
                                    <option value="MRM">MRM</option>
                                    </select>`;



                            }
                        },
                        {
                            data: "qty",
                            render: function (data, type, row) {
                                return "<input type='number' data-qty=" + row.qty + " data-id=" + row.id + " onchange='cambioqty(this);' onkeyup='cambioqty(this)' id='qty-" + row.id + "' class='form-control form-control-sm' readonly  value=" + data + " /> ";
                            }
                        },
                        {
                            data: "unit",
                            render: function (data, type, row) {
                                return "<input type='number' data-id=" + row.id + " data-price=" + data + " onchange='cambioprice(this);' onkeyup='cambioprice(this);' readonly class='form-control form-control-sm' id='txtprice-" + row.id + "' value=" + data.toFixed(4) + " /> ";
                            }
                        },
                        {
                            data: "cost",
                            render: function (data, type, row) {
                                return '<input type="number" onkeyup="cambiounit(this);" onchange="cambiounit(this);" data-id=' + row.id + ' data-unit=' + data + ' readonly class="form-control form-control-sm" id="txtcost-' + row.id + '" value=' + data.toFixed(4) + ' /> ';
                            }
                        },
                        {
                            data: "car",
                            render: function (data, type, row) {
                                let check = '';
                                if (data == true) {
                                    check = 'checked';
                                }
                                return "<input  id='car-" + row.id + "' data-car=" + data + " data-id=" + row.id + " onchange='cambiocar(this);'  class='w-100 text-center mt-2' type='checkbox' " + check + "  disabled />";
                            }
                        },
                        {
                            data: "retur",
                            render: function (data, type, row) {
                                return `<div class='input-group mb-3'>
<input class='text-center form-control form-control-sm rcodeup'  data-id='${row.id}' data-rcode='${data}' type='text' id='rcode-${row.id}'  readonly  value=${data} />
<div class='input-group-append'>
    <button type='button' class='btn btn-sm btn-primary' id='btncodes2' data-c="${data}"> <i class='fa fa-search'> </i> </button>
</div>
</div>`;
                            }
                        },
                        {
                            data: null,
                            render: function (data, type, row) {
                                return `
  <div class=" gap-1">
    <a class="btn mb-1 btn-primary" id="btneditlinea" data-id="752">
      <svg class="svg-inline--fa fa-pen fa-w-16" aria-hidden="true" focusable="false" data-prefix="fa" data-icon="pen" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512" data-fa-i2svg=""><path fill="currentColor" d="M290.74 93.24l128.02 128.02-277.99 277.99-114.14 12.6C11.35 513.54-1.56 500.62.14 485.34l12.7-114.22 277.9-277.88zm207.2-19.06l-60.11-60.11c-18.75-18.75-49.16-18.75-67.91 0l-56.55 56.55 128.02 128.02 56.55-56.55c18.75-18.76 18.75-49.16 0-67.91z"></path></svg><!-- <i class="fa fa-pen "></i> --> Edit
    </a>
    <a class="btn mb-1 btn-success  btnsavelinea" id="btnsavelinea-752" data-id="752" disabled="">
      <svg class="svg-inline--fa fa-check-circle fa-w-16" aria-hidden="true" focusable="false" data-prefix="fa" data-icon="check-circle" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512" data-fa-i2svg=""><path fill="currentColor" d="M504 256c0 136.967-111.033 248-248 248S8 392.967 8 256 119.033 8 256 8s248 111.033 248 248zM227.314 387.314l184-184c6.248-6.248 6.248-16.379 0-22.627l-22.627-22.627c-6.248-6.249-16.379-6.249-22.628 0L216 308.118l-70.059-70.059c-6.248-6.248-16.379-6.248-22.628 0l-22.627 22.627c-6.248 6.248-6.248 16.379 0 22.627l104 104c6.249 6.249 16.379 6.249 22.628.001z"></path></svg><!-- <i class="fa fa-check-circle "></i> --> Save
    </a>
    <a class="btn  btn-danger" id="btnDeleteRow" data-id="752">
      <svg class="svg-inline--fa fa-trash fa-w-14" aria-hidden="true" focusable="false" data-prefix="fa" data-icon="trash" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 448 512" data-fa-i2svg=""><path fill="currentColor" d="M432 32H312l-9.4-18.7A24 24 0 0 0 281.1 0H166.8a23.72 23.72 0 0 0-21.4 13.3L136 32H16A16 16 0 0 0 0 48v32a16 16 0 0 0 16 16h416a16 16 0 0 0 16-16V48a16 16 0 0 0-16-16zM53.2 467a48 48 0 0 0 47.9 45h245.8a48 48 0 0 0 47.9-45L416 128H32z"></path></svg><!-- <i class="fa fa-trash "></i> --> Remove
    </a>
  </div>
`;
                            }
                        },
                    ]
                });

            }

        });
    }

    $(document).on("click", "#btncodes2", function () {
        loadcodes2();
        arrRCode = [];
        arrRCode.push($(this).data("c"));
        $("#mcodes2").modal("show");
    });
    $(document).on("click", "#btnviewlines", function () {
        let id = $(this).data("id");
        $.ajax({
            url: "/Client/Rma/GetRmaInfo/?id=" + id,
            type: "GET",
            dataType: "json",
            async: true,
            cache: true,
            success: function (data) {

                var rma = data.rma;
                $("#rmaid").val(rma.id);
                $("#rmastatus").val(rma.status);
                $("#rmanumber").val(rma.rmarequest);
                $("#whereb").append("<option value=" + rma.wherebuilt + ">" + rma.wherebuilt + "</option>");

                if (rma.wherebuilt == "Nogales") {
                    $("#whereb").append("<option value='Endicot'>Endicot</option>");
                } else {
                    $("#whereb").append("<option value='Nogales'>Nogales</option>");
                }

                $('#sreason option:selected').text(rma.reason);
                $("#sreason")
                $("#sreason").trigger("change");

                $("#desc").val(rma.description);
                $("#client").val(rma.customer);
                $("#po").val(rma.customerpo);
                $("#totalrma").val(rma.totalrmavalues);
                $("#shipto").val(rma.ship_To);
                $("#contactrma").val(rma.contact);
                $("#phone").val(rma.phone);
                $("#ext").val(rma.ext);
                $("#selectrmatype").val(rma.rmatypeofrequest);
                $("#email").val(rma.email);
                $("#emailc").val(rma.company);
                $("#comments").val(rma.comment);

                $("#daterma").val(rma.date);

                if (rma.status == "Pending") {
                    $("#txtstatus").addClass("badge-warning");
                }
                if (rma.status == "Approved") {
                    $("#txtstatus").addClass("badge-success");
                } else {
                    $("#txtstatus").addClass("badge-secondary");
                }
                $("#txtstatus").text(rma.status);
                if (rma.sumbit == "Submitted") {
                    $("#txtsub").addClass("badge-success");
                    $("#btnsubmitrma").prop("disabled", true);
                } else {
                    $("#txtsub").addClass("badge-secondary");
                    $("#btnsubmitrma").prop("disabled", false);
                }
                $("#txtsub").text(rma.sumbit);

                console.log("LINEAS DE RMA");
                console.log(data.lines);

                reloadLineas();


                $("#btncodes").click(function () {

                    loadcodes();
                    $("#mcodes").modal("show");
                });
                $("#imgpn").click(function () {
                    LoadPartNumbers();
                    $("#mPN").modal("show");
                });


                //add row 
                $("#btnAddRow").click(function () {

                    let invoice = $("#invoicee").val();
                    let seq = $("#seqnew").val();
                    let pn = $("#pnnew").val();
                    let loc = $("#slocationsnew option:selected").val();
                    let qty = $("#qtynew").val();
                    let price = $("#txtprice").val();
                    let cost = $("#txtcost").val();
                    let car = $("#carnew").is(":checked") ? true : false;
                    let rcode = $("#rcodenew").val();
                    let id = $("#rmaid").val();
                    var actionselected = $("#sactions option:selected").val();
                    alert(actionselected);
                    let linea = {
                        Invoice: invoice,
                        Coustumer: pn,
                        Loc: loc,
                        Qty: qty,
                        Cost: price,
                        Seq: seq,
                        Action: actionselected,
                        Unit: cost,
                        Retur: rcode,
                        RmaId: id,
                        Car: car
                    };
                    var linedata = JSON.stringify(linea);

                    $.ajax({
                        url: "/Client/Rma/AddNewLine/?line=" + linedata,
                        type: "GET",
                        dataType: "json",
                        async: true,
                        cache: true,
                        success: function (data) {

                            if (data == true) {
                                $("#frmnewline").trigger("reset");
                                reloadLineas();
                            }
                        }
                    });

                });
                //delete row
                $(document).on("click", "#btnDeleteRow", function () {
                    let id = $(this).data("id");


                    Swal.fire({
                        title: 'Are you sure to delete line #' + id + '?',
                        text: "You won't be able to revert this!",
                        icon: 'warning',
                        showCancelButton: true,
                        confirmButtonColor: '#3085d6',
                        cancelButtonColor: '#d33',
                        confirmButtonText: 'Yes, delete it!'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            $.ajax({
                                url: "/Client/Rma/DeleteLinea/?id=" + id,
                                type: "GET",
                                dataType: "json",
                                async: true,
                                cache: true,
                                success: function (data) {

                                    if (data == true) {
                                        reloadLineas();
                                        Swal.fire(
                                            'Deleted!',
                                            'Your line has been deleted.',
                                            'success'
                                        );
                                    }
                                    else {
                                        Swal.fire({
                                            icon: 'error',
                                            title: 'Oops...',
                                            text: 'Something went wrong!'
                                        })
                                    }
                                }
                            });

                        }
                    })


                });

                $(document).on("click", "#selectlinea", function () {
                    let id = $(this).data("id");
                    if ($(this).is(":checked")) {
                    }
                    else {
                    }
                    //alert("eliminar linea"+id);
                });


                $("#btnshowline").click(function () {
                    $("#rowadd").fadeToggle("fast");
                });
                $(document).on("click", "#bntabrirfact", function () {
                    //abrirfacturas();
                    openinvoices();
                    $("#minvoices").modal("show");
                });
                $(document).on("click", "#btnopenseq", function () {
                    LoadSeq($(".invoiceenew").val());
                    $("#mseq").modal("show");
                });
                $("#btncloseedit").click(function () {
                    $("#meditrma").modal("hide");
                });
                $(document).on("click", "#btneditlinea", function () {

                    let id = $(this).data("id");
                    arrRmaId = [];
                    arrRmaId.push(id);

                    $("#slocations-" + id).prop("disabled", false);


                    $("#qty-" + id).prop("readonly", false);
                    $("#qty-" + id).focus();

                    //$("#rcode-" + id).prop("readonly", false);


                    $("#txtprice-" + id).prop("readonly", false);
                    $("#txtcost-" + id).prop("readonly", false);
                    $(".action-" + id).prop("disabled", false);
                    $("#car-" + id).prop("disabled", false);
                });


                $(document).on("click", ".btnsavelinea", function () {
                    let id = $(this).data("id");
                    alert(id);
                });


                $("#tblfiles").DataTable({
                    destroy: true,
                    dom: "btpi",
                    data: data.files,
                    columns: [
                        { data: "id" },
                        {
                            data: "documento",
                            render: function (data, type, row) {
                                return "<a class='btn-link open_file_control' id='btnopenfile' >" + data + "</a>  <a> <i data-fname=" + data + " data-id=" + row.id + " class='fa fa-trash text-danger'> </i>  </a>";
                            }
                        }

                    ]

                });


                $(document).on("click", "#btnopenfile", function () {
                    let rma = $("#rmanumber").val();
                    let fname = $(this).text();

                    alert(rma);
                    alert(fname);

                    $(this).prop("href", "/Client/rma/OpenFileRMA?rmano=" + rma + "&filename=" + fname);




                });
            }
        });
        $("#frmheader input").attr("readonly", true);
        $("#frmheader select").prop("disabled", true);
        $("#frmheader textarea").prop("readonly", true);
        $("#btnshowline").prop("disabled", true);
        $("#btnupdaterma").prop("disabled", true);
        $("#frmlines input").prop("disabled", true);
        $("#frmlines table .btn").prop("disabled", true);
        $("#btnsubmitrma").fadeOut();
        $("#meditrma").modal("show");
    });
    $(document).on("click", "#btneditlines", function () {

        let id = $(this).data("id");

        $.ajax({
            url: "/Client/Rma/GetRmaInfo/?id=" + id,
            type: "GET",
            dataType: "json",
            async: true,
            cache: true,
            success: function (data) {

                var rma = data.rma;
                $("#rmaid").val(rma.id);
                $("#rmanumber").val(rma.rmarequest);
                $("#rmastatus").val(rma.status);
                $("#rmasubmit").val(rma.sumbit);
                $("#rma500").val(rma.rma500);
                $("#whereb").append("<option value=" + rma.wherebuilt + ">" + rma.wherebuilt + "</option>");

                $('#sreason option:selected').text(rma.reason);

                $("#sreason").trigger("change");

                if (rma.wherebuilt == "Nogales") {
                    $("#whereb").append("<option value='Endicot'>Endicot</option>");
                } else {
                    $("#whereb").append("<option value='Nogales'>Nogales</option>");
                }
                $("#desc").val(rma.description);
                $("#client").val(rma.customer);
                $("#po").val(rma.customerpo);
                $("#totalrma").val(rma.totalrmavalues);
                $("#shipto").val(rma.ship_To);
                $("#contactrma").val(rma.contact);
                $("#phone").val(rma.phone);
                $("#ext").val(rma.ext);
                $("#selectrmatype").val(rma.rmatypeofrequest);
                $("#email").val(rma.email);
                $("#emailc").val(rma.company);
                $("#comments").val(rma.comment);

                $("#daterma").val(rma.date);
                if (rma.status == "Pending") {
                    $("#txtstatus").addClass("badge-warning");
                }
                if (rma.status == "Approved") {
                    $("#txtstatus").addClass("badge-success");
                } else {
                    $("#txtstatus").addClass("badge-secondary");
                }
                $("#txtstatus").text(rma.status);
                if (rma.sumbit == "Submitted") {
                    $("#txtsub").addClass("badge-success");
                    $("#btnsubmitrma").prop("disabled", true);
                } else {
                    $("#txtsub").addClass("badge-secondary");
                    $("#btnsubmitrma").prop("disabled", false);
                }
                $("#txtsub").text(rma.sumbit);

                console.log("LINEAS DE RMA");
                console.log(data.lines);

                reloadLineas();


                $("#btncodes").click(function () {
                    loadcodes();
                    $("#mcodes").modal("show");
                });
                $("#imgpn").click(function () {
                    LoadPartNumbers();
                    $("#mPN").modal("show");
                });


                //add row 
                $("#btnAddRow").click(function () {

                    let invoice = $("#invoicee").val();
                    let seq = $("#seqnew").val();
                    let pn = $("#pnnew").val();
                    let loc = $("#slocationsnew option:selected").val();
                    let qty = $("#qtynew").val();
                    let price = $("#txtprice").val();
                    let cost = $("#txtcost").val();
                    let car = $("#carnew").is(":checked") ? true : false;
                    let rcode = $("#rcodenew").val();
                    let id = $("#rmaid").val();
                    var actionselected = $("#sactions option:selected").val();

                    let linea = {
                        Invoice: invoice,
                        Coustumer: pn,
                        Loc: loc,
                        Qty: qty,
                        Cost: price,
                        Seq: seq,
                        Action: actionselected,
                        Unit: cost,
                        Retur: rcode,
                        RmaId: id,
                        Car: car
                    };
                    var linedata = JSON.stringify(linea);

                    $.ajax({
                        url: "/Client/Rma/AddNewLine/?line=" + linedata,
                        type: "GET",
                        dataType: "json",
                        async: true,
                        cache: true,
                        success: function (data) {

                            if (data == true) {
                                $("#frmnewline").trigger("reset");
                                reloadLineas();
                            }
                        }
                    });

                });
                //delete row
                $(document).on("click", "#btnDeleteRow", function () {
                    let id = $(this).data("id");


                    Swal.fire({
                        title: 'Are you sure to delete line #' + id + '?',
                        text: "You won't be able to revert this!",
                        icon: 'warning',
                        showCancelButton: true,
                        confirmButtonColor: '#3085d6',
                        cancelButtonColor: '#d33',
                        confirmButtonText: 'Yes, delete it!'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            $.ajax({
                                url: "/Client/Rma/DeleteLinea/?id=" + id,
                                type: "GET",
                                dataType: "json",
                                async: true,
                                cache: true,
                                success: function (data) {

                                    if (data == true) {
                                        reloadLineas();
                                        Swal.fire(
                                            'Deleted!',
                                            'Your line has been deleted.',
                                            'success'
                                        );
                                    }
                                    else {
                                        Swal.fire({
                                            icon: 'error',
                                            title: 'Oops...',
                                            text: 'Something went wrong!'
                                        })
                                    }
                                }
                            });

                        }
                    })


                });

                $(document).on("click", "#selectlinea", function () {
                    let id = $(this).data("id");
                    if ($(this).is(":checked")) {
                    }
                    else {
                    }
                    //alert("eliminar linea"+id);
                });


                $("#btnshowline").click(function () {
                    $("#rowadd").fadeToggle("fast");
                });
                $(document).on("click", "#bntabrirfact", function () {
                    //abrirfacturas();
                    openinvoices();
                    $("#minvoices").modal("show");
                });
                $(document).on("click", "#btnopenseq", function () {
                    LoadSeq($(".invoiceenew").val());
                    $("#mseq").modal("show");
                });
                $("#btncloseedit").click(function () {
                    $("#meditrma").modal("hide");
                });
                //aquii
                $(document).on("click", "#btneditlinea", function () {

                    let id = $(this).data("id");
                    arrRmaId = [];
                    arrRmaId.push(id);
                    $("#slocations-" + id).prop("disabled", false);

                    $("#qty-" + id).prop("readonly", false);
                    $("#qty-" + id).focus();

                    //$("#rcode-" + id).prop("readonly", false);


                    $("#txtprice-" + id).prop("readonly", false);
                    $("#txtcost-" + id).prop("readonly", false);
                    $(".action-" + id).prop("disabled", false);

                    $("#car-" + id).prop("disabled", false);
                });


                $(document).on("click", ".btnsavelinea", function () {
                    let id = $(this).data("id");
                    let loc = $("#slocations-" + id).children("option:selected").val();
                    let qty = $("#qty-" + id).val();
                    let price = $("#txtprice-" + id).val();
                    let cost = $("#txtcost-" + id).val();
                    let code = $("#rcode-" + id).val();
                    let car_val = $("#car-" + id).val();
                    let actionn = $("#action option:selected").val();


                    $.ajax({
                        url: "/client/Rma/UpdateLinea?id=" + id + "&actionn=" + actionn + "&loc=" + loc + "&qty=" + qty + "&price=" + price + "&unitcost=" + cost + "&rcode=" + code + "&car=" + car_val,
                        type: "GET",
                        dataType: "json",
                        cache: true,
                        async: true,
                        success: function success(data) {
                            if (data == true) {
                                reloadLineas();
                            }
                        }

                    });

                });


                reloadFilesEdit();

                $("#btnnewfile").click(function () {
                    let file = $("#txtfile")[0].files[0];
                    let rma_no = $("#rmanumber").val();

                    const formData = new FormData();
                    formData.append("file", file);
                    formData.append("rma", rma_no)
                    $.ajax({
                        url: '/Client/rma/SaveFileRMA/',
                        type: 'POST',
                        dataType: "json",
                        contentType: false,
                        processData: false,
                        async: true,
                        data: formData,
                        success: function (data) {
                            reloadFilesEdit();
                            $("#txtfile").val("");

                        }
                    })


                });


                //open edit
                $(document).on("click", "#btnopenfile_edit", function () {


                    let rma = $("#rmanumber").val();
                    let fname = $(this).text();


                    $(this).prop("href", "/Client/rma/OpenFileRMA?rmano=" + rma + "&filename=" + fname);

                    //$.ajax({
                    //    url: "/client/rma/OpenFileRMA/",
                    //    type: "GET",
                    //    data: { rmano: rma,filename:fname },
                    //    dataType: "json",
                    //    cache: true,
                    //    async: true,
                    //    success: function success(data) {
                    //        return data;
                    //    }  
                    //    });

                });

            }
        });

        $("#meditrma").modal("show");
    });
}

function reloadFilesEdit() {


    var idrma = $("#rmaid").val();

    $.ajax({
        url: "/Client/rma/LoadFilesByRMA",
        data: { rmaid: idrma },
        dataType: "json",
        type: "GET",
        async: true,
        cache: true,
        success: function (files) {
            $("#tblfiles").DataTable({
                destroy: true,
                dom: "btpi",
                data: files,
                pageLength: 5,
                columns: [
                    { data: "id" },
                    {
                        data: "documento",
                        render: function (data, type, row) {
                            return "<a class='btn-link' id='btnopenfile_edit'>" + data + "</a>  <a > <i id='btndelete_file' data-fname=" + data + " data-id=" + row.id + " class='fa fa-trash text-danger' > </i> </a>";
                        }
                    }

                ]

            });

            $(document).on("click", "#btndelete_file", function () {
                let fname = $(this).data("fname");
                let rmano = $("#rmanumber").val();
                let id = $(this).data("id");



                Swal.fire({
                    title: 'Are you sure?',
                    text: "You won't be able to revert this!",
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#3085d6',
                    cancelButtonColor: '#d33',
                    confirmButtonText: 'Yes, delete it!'
                }).then((result) => {
                    if (result.isConfirmed) {

                        $.ajax({
                            url: "/Client/rma/DeleteFileRMA",
                            data: { id: id, rma: rmano, filename: fname },
                            dataType: "json",
                            type: "GET",
                            async: true,
                            cache: true,
                            success: function (deleted) {


                                reloadFilesEdit();

                                Swal.fire(
                                    'Deleted!',
                                    'Your file has been deleted.',
                                    'success'
                                )

                            }
                        })



                    }
                })

            });




        }
    });




}


function LoadSeq(fact) {
    $.ajax({
        url: "/client/corrective/getSecuencias/",
        type: "GET",
        data: { factura: fact },
        dataType: "json",
        cache: true,
        async: true,
        success: function success(data) {

            var table = $("#tblseq").DataTable({
                destroy: true,
                data: data,
                columns: [
                    {
                        "data": "line_seq_no",
                        render: function (data, type, row) {
                            return '<a class="text-primary"  data-item=' + row.item_no + ' id="linkseq2" data-seq=' + row.line_seq_no + '>' + row.line_seq_no + '</a>';
                        }
                    },
                    { "data": "ord_type" },
                    { "data": "ord_no" },
                    { "data": "item_no" }
                ]
            });



        },
        complete: function () {
            $('body').loadingModal('hide');
            $('body').loadingModal('destroy');
        },
        error: function error(xhr, status, _error17) {
            console.log(xhr.responseText);
        },
    });
    $(document).on("click", "#linkseq2", function () {
        $("#seqnew").val($(this).text());
        let item = $(this).data("item");
        $("#pnnew").val(item);
        $("#pnnew").prop("readonly", true);
        $("#imgpn").prop("disabled", true);
        arrfactura = [];
        $.ajax({
            url: "/Client/Corrective/GetPricesByInvoice/?invoicenumber=" + $(".invoice").val() + "&loc=D",
            type: "GET",
            dataType: "json",
            async: true,
            cache: true,
            success: function (data) {

                if (data[0] !== undefined) {
                    $("#msgloc").fadeOut();
                    $("#btnAddRow").prop("disabled", false);
                    $("#txtprice").val(parseFloat(data[0].price).toFixed(4));
                    $("#txtcost").val(parseFloat(data[0].std).toFixed(4));
                    arrfactura.push(data[0].loc);
                }


            }
        });
        $("#mseq").modal("hide");

    });
}


function cambioqty(inputqty) {
    let id = $(inputqty).data("id");
    let qty = $(inputqty).data("qty");
    if (!$(inputqty).val()) {
        $("#btnsavelinea-" + id).prop("disabled", true);
    }
    else {
        if ($(inputqty).val() == 0) {
            $("#btnsavelinea-" + id).prop("disabled", true);
        }
        else {
            if ($(inputqty).val() !== qty) {
                $("#btnsavelinea-" + id).prop("disabled", false);
            }
            if ($(inputqty).val() == qty) {
                $("#btnsavelinea-" + id).prop("disabled", true);
            }
        }
    }

}
function cambioprice(inputqty) {
    let id = $(inputqty).data("id");
    let qty = $(inputqty).data("qty");
    if (!$(inputqty).val()) {
        $("#btnsavelinea-" + id).prop("disabled", true);
    }
    else {
        if ($(inputqty).val() == 0) {
            $("#btnsavelinea-" + id).prop("disabled", true);
        }
        else {
            if ($(inputqty).val() !== qty) {
                $("#btnsavelinea-" + id).prop("disabled", false);
            }
            if ($(inputqty).val() == qty) {
                $("#btnsavelinea-" + id).prop("disabled", true);
            }
        }
    }

}
function cambiounit(inputqty) {
    let id = $(inputqty).data("id");
    let qty = $(inputqty).data("unit");
    if (!$(inputqty).val()) {
        $("#btnsavelinea-" + id).prop("disabled", true);
    }
    else {
        if ($(inputqty).val() == 0) {
            $("#btnsavelinea-" + id).prop("disabled", true);
        }
        else {
            if ($(inputqty).val() !== qty) {
                $("#btnsavelinea-" + id).prop("disabled", false);
            }
            if ($(inputqty).val() == qty) {
                $("#btnsavelinea-" + id).prop("disabled", true);
            }
        }
    }



}
function cambiocar(inputqty) {
    let id = $(inputqty).data("id");
    let check = $(inputqty).data("car");

    if (!$(inputqty).val()) {
        $("#btnsavelinea-" + id).prop("disabled", true);
    }
    else {
        if ($(inputqty).val() == false) {
            $("#btnsavelinea-" + id).prop("disabled", true);
        }
        else {
            if ($(inputqty).val() !== check) {
                $("#btnsavelinea-" + id).prop("disabled", false);
            }
            if ($(inputqty).val() == check) {
                $("#btnsavelinea-" + id).prop("disabled", true);
            }
        }
    }

}
function cambioloc(inputqty) {
    let id = $(inputqty).data("id");
    let qty = $(inputqty).data("loc");
    if (!$(inputqty).children("option:selected").val()) {
        $("#btnsavelinea-" + id).prop("disabled", true);
    }
    else {
        if ($(inputqty).children("option:selected").val() == 0) {
            $("#btnsavelinea-" + id).prop("disabled", true);
        }
        else {

            if ($(inputqty).children("option:selected").val() !== qty) {
                $("#btnsavelinea-" + id).prop("disabled", false);
            }
            if ($(inputqty).children("option:selected").val() == qty) {
                $("#btnsavelinea-" + id).prop("disabled", true);
            }
        }
    }

}

function openinvoicesControl() {
    const client = document.getElementById("client").value;
    $.ajax({
        url: "/client/corrective/GetAllI2/?filtro=" + client.trim(),
        type: "GET",
        datatype: "json",
        async: true,
        cache: true,

        success: function (data) {
            console.log("facturas");
            $("#tblinvoices").DataTable({
                destroy: true,
                data: data,
                columns: [
                    {
                        data: 'invNo',
                        render: function (data, type, row) {
                            return '<a class="text-primary"  id="linkinvno" data-no=' + row.invNo + '>' + row.invNo + '</a>';
                        }
                    },
                    { data: 'ordNo' },
                    { data: 'cusNo' },
                    { data: 'ordType' },
                    { data: 'ordDt' },
                    { data: 'cusAltAdrCd' },
                    { data: 'currTrxRt' },
                    { data: 'currCd' }
                ]
            });
            //console.log(data); 
        },
        error: function (xhr, status, error) {
            $().toastmessage("showToast", {
                text: "Error con ean " + xhr.responseText,
                sticky: true,
                type: "error",
            });
        },
    });
    //linkinvno
    $(document).on("click", "#tblinvoices", function () {
        $("#abrir1").val($(this).data("no"));
        $("#minvoices").modal("hide");
    });
    $(document).on("click", "#linkinvno", function () {
        //alert($(this).text()); 
        $(".invoiceenew").val($(this).text());
        LoadSeq($(this).text());
        $("#minvoices").modal("hide");
        //LoadSecuencias($(this).text());

    });

}


function reloadLineasControl() {

    $.ajax({
        url: "/Client/Rma/GetRmaInfo/?id=" + $("#rmaid").val(),
        type: "GET",
        dataType: "json",
        async: true,
        cache: true,
        success: function (data) {


            var tableLineas = $("#tbllineas").DataTable({
                destroy: true,
                dom: "btpi",
                paging: 5,
                data: data.lines,

                columns: [

                    {
                        data: null,
                        render: function (data, type, row) {
                            return ` 
                                <div class="d-flex"> 
                                    <div class="custom-file">
                                        <input id="invoicee" readonly="readonly"  name="invoice" class="inv t invoice form-control form-control-sm float-left" value=${row.invoice} />
                                    </div> 
 
                                </div>`;
                        }
                    },
                    {
                        data: null,
                        render: function (data, type, row) {
                            return ` 
                                <div class="row">

<div class="col-12">

<div class="input-group d-flex">
                                    <div class="custom-file">
                                      <input readonly="readonly"   name="seq" class="t2 t seq form-control form-control-sm seq"  value=${row.seq}  /> 
                                    </div> 
                                </div>
</div>
</div>
`;
                        }
                    },
                    {
                        data: null,
                        render: function (data, type, row) {
                            return ` 
                                <div class="input-group d-flex">
                                    <div class="custom-file">
                                     <input onkeyup="javascript:this.value=this.value.toUpperCase();"  name="coustumer" readonly   type="text" class="form-control form-control-sm cos t  coustumer" value=${row.coustumer} id="inputGroupFile02">
                                    </div> 
                                </div>`;
                        }
                    },
                    {
                        data: "action",
                        render: function (data, type, row) {

                            return "<input class='form-control form-control-sm action-" + row.id + "'  type='text' data-id=" + row.id + " value=" + data + " id='action'  readonly />";
                        }
                    },
                    {
                        data: "loc",
                        render: function (data, type, row) {


                            return `<select disabled onchange='cambioloc(this);' onkeyup='cambioloc(this);'  id='slocations-${row.id}' data-loc='${data}'  class="loc form-control form-control-sm locationsadd" name="loc">
                                    <option value=${data}>${data}</option>
                                    <option value="ARM">ARM</option>
                                    <option value="ERN">ERN</option>
                                    <option value="MRM">MRM</option>
                                    </select>`;



                        }
                    },
                    {
                        data: "qty",
                        render: function (data, type, row) {
                            return "<input type='number' data-qty=" + row.qty + " data-id=" + row.id + " onchange='cambioqty(this);' onkeyup='cambioqty(this)' id='qty-" + row.id + "' class='form-control form-control-sm' readonly  value=" + data + " /> ";
                        }
                    },
                    {
                        data: "cost",
                        render: function (data, type, row) {
                            return "<input type='number' onchange='cambioprice(this);' onkeyup='cambioprice();' readonly data-id=" + row.id + " data-price=" + data + " class='form-control form-control-sm' id='txtprice-" + row.id + "' value=" + data + " /> ";
                        }
                    },
                    {
                        data: "unit",
                        render: function (data, type, row) {
                            return '<input type="number" readonly class="form-control form-control-sm" id="txtcost-' + row.id + '" value=' + data + ' /> ';
                        }
                    },
                    {
                        data: "car",
                        render: function (data, type, row) {
                            let check = '';
                            if (data == true) {
                                check = 'checked';
                            }
                            return "<input id='car-" + row.id + "' class='w-100 text-center mt-2' type='checkbox' " + check + "  disabled />";
                        }
                    },
                    {
                        data: "retur",
                        render: function (data, type, row) {
                            return `<div class='input-group mb-3'>
<input class='text-center form-control form-control-sm' type='text' id='rcode-${row.id}'  readonly  value=${data} />
<div class='input-group-append'>
    <button type='button' class='btn btn-sm btn-primary'> <i class='fa fa-search'> </i> </button>
</div>
</div>`;
                        }
                    },
                    {
                        data: null,
                        render: function (data, type, row) {
                            return `<div class=" gap-1">
                                <a class="btn mb-1 btn-primary" id="btneditlinea" data-id="752">
                                    <svg class="svg-inline--fa fa-pen fa-w-16" aria-hidden="true" focusable="false" data-prefix="fa" data-icon="pen" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512" data-fa-i2svg=""><path fill="currentColor" d="M290.74 93.24l128.02 128.02-277.99 277.99-114.14 12.6C11.35 513.54-1.56 500.62.14 485.34l12.7-114.22 277.9-277.88zm207.2-19.06l-60.11-60.11c-18.75-18.75-49.16-18.75-67.91 0l-56.55 56.55 128.02 128.02 56.55-56.55c18.75-18.76 18.75-49.16 0-67.91z"></path></svg><!-- <i class="fa fa-pen "></i> --> Edit
                                </a>
                                <a class="btn mb-1 btn-success  btnsavelinea" id="btnsavelinea-752" data-id="752" disabled="">
                                    <svg class="svg-inline--fa fa-check-circle fa-w-16" aria-hidden="true" focusable="false" data-prefix="fa" data-icon="check-circle" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512" data-fa-i2svg=""><path fill="currentColor" d="M504 256c0 136.967-111.033 248-248 248S8 392.967 8 256 119.033 8 256 8s248 111.033 248 248zM227.314 387.314l184-184c6.248-6.248 6.248-16.379 0-22.627l-22.627-22.627c-6.248-6.249-16.379-6.249-22.628 0L216 308.118l-70.059-70.059c-6.248-6.248-16.379-6.248-22.628 0l-22.627 22.627c-6.248 6.248-6.248 16.379 0 22.627l104 104c6.249 6.249 16.379 6.249 22.628.001z"></path></svg><!-- <i class="fa fa-check-circle "></i> --> Save
                                </a>
                                <a class="btn  btn-danger" id="btnDeleteRow" data-id="752">
                                    <svg class="svg-inline--fa fa-trash fa-w-14" aria-hidden="true" focusable="false" data-prefix="fa" data-icon="trash" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 448 512" data-fa-i2svg=""><path fill="currentColor" d="M432 32H312l-9.4-18.7A24 24 0 0 0 281.1 0H166.8a23.72 23.72 0 0 0-21.4 13.3L136 32H16A16 16 0 0 0 0 48v32a16 16 0 0 0 16 16h416a16 16 0 0 0 16-16V48a16 16 0 0 0-16-16zM53.2 467a48 48 0 0 0 47.9 45h245.8a48 48 0 0 0 47.9-45L416 128H32z"></path></svg><!-- <i class="fa fa-trash "></i> --> Remove
                                </a>
                            </div>`
                        }
                    },
                ]
            });


            $(document).on("click", "#btnsavelinea", function () {

                alert("yes");
            });

        }

    });
}

//control rma
function cargarDatatableRma2() {

    dataTable = $("#tblCar").DataTable({
        "order": [[2, "desc"]],
        "fixedColumns": {
            start: 1 // Freezes the first column (left side)
        },
        pageLength: 10,
        lengthMenu: [10, 25, 50, 100],
        "scrollX": true,
        "ajax": {
            "url": "/Client/rma/GetAllrma2",
            "type": "GET",
            "dataType": "json"
        },
        "columns": [
            {
                "width": "70px",
                "data": "id",
                "render": function render(data, type, row) {
                    return `
                        <div class="d-flex gap-1">
                              <a href="#" class="btn btn-sm btn-light action-btn d-flex align-items-center gap-1" data-id="${row.id}" id="btneditlinescontrol" 
                                data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="right" title="Edit RMA Request">
                                <i class="fas fa-edit"></i>
                              </a>
                            <div class="dropdown">
                              <button class="btn btn-light action-btn d-flex" type="button" id="moreOptions" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    <i class="fa fa-ellipsis-v"></i>
                              </button>
                              <div class="dropdown-menu" aria-labelledby="moreOptions">
                                    <a href="#" class="btn btn-sm btn-light action-btn d-flex dropdown-item" onclick="lineas(${data})" data-toggle="modal" data-target="#lineas">
                                       <span><i class="fa fa-list me-2"></i>View RMA Lines</span>
                                    </a>
                                    <a class="btn btn-light btn-sm action-btn d-flex dropdown-item" data-toggle="modal" data-target="#Customer_complaint" onclick="Customer_complaint('${row.customercomplait}')">
                                        <span><i class="fa fa-comments me-2"></i>Customer Complaint</span>
                                    </a>
                                    <a href="#" class="btn btn-sm btn-light action-btn d-flex dropdown-item" data-toggle="modal" data-target="#Comments" onclick="Comments(${data})">
                                       <span><i class="fa fa-info-circle me-2"></i>Aditional Information</span>
                                    </a>
                                    <a href="#" class="btn btn-sm btn-light action-btn d-flex dropdown-item" data-toggle="modal" data-target="#Remark" onclick="Remark(${data})">
                                       <span><i class="fa fa-sticky-note me-2"></i>Remarks</span>
                                    </a>
                                    <a href="#" class="btn btn-sm btn-light action-btn d-flex dropdown-item" data-toggle="modal" data-target="#documents" onclick="Abrir(${data})">
                                       <span><i class="fa fa-paperclip me-2"></i>Attachments</span>
                                    </a>
                              </div>
                            </div>
                         </div>`;
                }
            },
            {
                "data": "id",
                "width": "180",
                "render": function render(data, type, row) {
                    return `
                    <div class="d-flex w-100 gap-1">
                        <a href="#" class="btn btn-sm btn-danger flex-fill w-50"  data-toggle="modal" data-target="#Rechazar" onclick="Disapprove(${data})"
                            data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="right" title="Reject Request">
                            <span><i class="fa fa-times me-2"></i>Reject</span>
                        </a>
                        <a href="#" class="btn btn-sm btn-success flex-fill w-50" data-toggle="modal" data-target="#Aprobar" onclick="Pass(${data})"
                            data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="right" title="Approve Request">
                            <span><i class="fa fa-check me-2"></i>Approve</span>
                        </a>
                    </div>
                    `;
                }
            },
            {
                "width": "110px",
                "data": "rmarequest"
            }, {
                "width": "110px",
                "data": "date"
            }, {
                "width": "130px",
                "data": "preparado"
            }, {
                "width": "200px",
                "data": "customerpartno"
            }, {
                "width": "200px",
                "data": "description"
            }, {
                "width": "200px",
                "data": "rmatypeofrequest"
            }, {
                "width": "120px",
                "data": "totalrmavalues",
                render: function (data) {

                    return data.toFixed(4);
                }
            }, {
                "width": "100px",
                "data": "wherebuilt"
            }, {
                "width": "130px",
                "data": "approver"
            }, {
                "width": "140px",
                "data": "customerpo"
            }
        ],
        drawCallback: function () {
            $('[data-bs-toggle="tooltip"]').each(function () {
                new bootstrap.Tooltip(this);
            });
        },
        "language": {
            "emptyTable": "No records found."
        }
    });



    $("#btnclosemedit").click(function () {
        //#meditrma

        //$("#meditrma").modal("hide");
        $('#meditrma').fadeOut();
        window.location.reload();
    });


    $(document).on("click", "#btneditlinescontrol", function () {
        let id = $(this).data("id");



        $.ajax({
            url: "/Client/Rma/GetRmaInfo/?id=" + id,
            type: "GET",
            dataType: "json",
            async: true,
            cache: true,
            success: function (data) {

                var rma = data.rma;
                $("#rmaid").val(rma.id);
                $("#rmanumber").val(rma.rmarequest);
                $("#whereb").val(rma.wherebuilt);
                $("#desc").val(rma.description);
                $("#client").val(rma.customer);
                $("#po").val(rma.customerpo);
                $("#totalrma").val(rma.totalrmavalues);
                $("#shipto").val(rma.ship_to);
                $("#contactrma").val(rma.contact);
                $("#phone").val(rma.phone);
                $("#ext").val(rma.ext);
                $("#selectrmatype").val(rma.rmatypeofrequest);
                $("#email").val(rma.email);
                $("#emailc").val(rma.company);
                $("#comments").val(rma.comment);

                $("#daterma").val(rma.date);

                console.log("LINEAS DE RMA");
                console.log(data.lines);

                reloadLineasControl();


                $("#btncodes").click(function () {
                    loadcodes();
                    $("#mcodes").modal("show");
                });
                $("#imgpn").click(function () {
                    LoadPartNumbers();
                    $("#mPN").modal("show");
                });


                //add row 
                $("#btnAddRow").click(function () {

                    let invoice = $("#invoicee").val();
                    let seq = $("#seqnew").val();
                    let pn = $("#pnnew").val();
                    let loc = $("#slocationsnew option:selected").val();
                    let qty = $("#qtynew").val();
                    let price = $("#txtprice").val();
                    let cost = $("#txtcost").val();
                    let car = $("#carnew").is(":checked") ? true : false;
                    let rcode = $("#rcodenew").val();
                    let id = $("#rmaid").val();
                    var actionselected = $("#sactions option:selected").val();

                    let linea = {
                        Invoice: invoice,
                        Coustumer: pn,
                        Loc: loc,
                        Qty: qty,
                        Cost: price,
                        Seq: seq,
                        Action: actionselected,
                        Unit: Unit,
                        Retur: rcode,
                        RmaId: id,
                        Car: car
                    };
                    var linedata = JSON.stringify(linea);


                    $.ajax({
                        url: "/Client/Rma/AddNewLine/?line=" + linedata,
                        type: "GET",
                        dataType: "json",
                        async: true,
                        cache: true,
                        success: function (data) {

                            if (data == true) {
                                $("#frmnewline").trigger("reset");
                                reloadLineasControl();
                            }
                        }
                    });

                });
                //delete row
                $(document).on("click", "#btnDeleteRow", function () {
                    let id = $(this).data("id");


                    Swal.fire({
                        title: 'Are you sure to delete line #' + id + '?',
                        text: "You won't be able to revert this!",
                        icon: 'warning',
                        showCancelButton: true,
                        confirmButtonColor: '#3085d6',
                        cancelButtonColor: '#d33',
                        confirmButtonText: 'Yes, delete it!'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            $.ajax({
                                url: "/Client/Rma/DeleteLinea/?id=" + id,
                                type: "GET",
                                dataType: "json",
                                async: true,
                                cache: true,
                                success: function (data) {

                                    if (data == true) {
                                        reloadLineasControl();
                                        Swal.fire(
                                            'Deleted!',
                                            'Your line has been deleted.',
                                            'success'
                                        );
                                    }
                                    else {
                                        Swal.fire({
                                            icon: 'error',
                                            title: 'Oops...',
                                            text: 'Something went wrong!'
                                        })
                                    }
                                }
                            });

                        }
                    })


                });

                $(document).on("click", "#selectlinea", function () {
                    let id = $(this).data("id");
                    if ($(this).is(":checked")) {
                    }
                    else {
                    }
                    //alert("eliminar linea"+id);
                });


                $("#btnshowline").click(function () {
                    $("#rowadd").fadeToggle("fast");
                });
                $(document).on("click", "#bntabrirfact", function () {
                    //abrirfacturas();
                    openinvoicesControl();
                    $("#minvoices").modal("show");
                });
                $(document).on("click", "#btnopenseq", function () {
                    LoadSeq($(".invoiceenew").val());
                    $("#mseq").modal("show");
                });
                $("#btncloseedit").click(function () {
                    $("#meditrma").modal("hide");
                });
                $(document).on("click", "#btneditlinea", function () {

                    let id = $(this).data("id");
                    arrRmaId = [];
                    arrRmaId.push(id);
                    $("#slocations-" + id).prop("disabled", false);

                    $("#qty-" + id).prop("readonly", false);
                    $("#qty-" + id).focus();

                    //$("#rcode-" + id).prop("readonly", false);


                    $("#txtprice-" + id).prop("readonly", false);
                    $("#txtcost-" + id).prop("readonly", false);
                    $(".action-" + id).prop("disabled", false);


                });


                $(document).on("click", "#btnsavelinea", function () {
                    let id = $(this).data("id");
                    alert(id);
                });
                //control open file 

                $("#tblfiles").DataTable({
                    destroy: true,
                    dom: "btpi",
                    data: data.files,
                    columns: [
                        { data: "id" },
                        {
                            data: "documento",
                            render: function (data) {
                                return "<a class='btn-link ' id='btnopenfile'>" + data + "</a>  <a> <i class='fa fa-trash text-danger'> </i> </a>";
                            }
                        }

                    ]

                });

                $(document).on("click", "#btnopenfile", function () {
                    let rma = $("#rmanumber").val();
                    let fname = $(this).text();



                    $(this).prop("href", "/Client/rma/OpenFileRMA?rmano=" + rma + "&filename=" + fname);




                });

            }
        });

        $("#meditrma").modal("show");
    });
}
//car list
function cargarDatatableCar() {
    var release = "";


    dataTable = $("#tblCar").DataTable({
        layout: {
            topStart: {
                buttons: ['copy', 'csv', 'excel', 'pdf', 'print']
            }
        },
        pageLength: 9,
        destroy: true,
        "order": [[0, "desc"]],
        "scrollX": true,

        "ajax": {
            "url": "/Client/corrective/GetAllcar",
            "type": "GET",
            contentType: "application/json",
            dataType: "json"
        },
        "columns": [{
            "data": "id",
            "width": "10%"
        },
        {
            data: "start_date",
            render: function (data) {
                return new Date(data).toLocaleString();
            }
        },
        {
            data: "approval_date",
            render: function (data) {
                return new Date(data).toLocaleString();
            }
        },

        {
            "data": "partnumber",
            "render": function render(data) {
                release = data;
                return "" + data;
            },
            "width": "10%"
        },
        {
            data: "owner_name"

        },

        {
            "data": "assignedto",
            "width": "10%"
        }, {
            "data": "rmanumber",
            "width": "10%"
        }, {
            "data": "category",
            "width": "10%"
        }, {
            "data": "department",
            "width": "10%"
        }, {
            "data": "status",
            render: function (data) {
                switch (data) {
                    case "In Process":
                        return "<span class='badge badge-warning p-1'>" + data + "</span>";
                        break;
                    case "Rejected":
                        return "<span class='badge badge-danger p-1'>" + data + "</span>";
                        break;
                    case "Closed":
                        return "<span class='badge badge-success p-1'>" + data + "</span>";
                        break;
                    case "Delayed":
                        return "<span class='badge badge-danger p-1'>" + data + "</span>";
                        break;
                    default:
                }
            },
            "width": "10%"
        },
        {
            data: "sumbit",
            render: function (data) {

                if (data == true) {
                    return "<i class='fa w-100 text-center fa-check text-success mx-auto'> </i>";
                } else {
                    return "<i class='fa w-100 text-center fa-times text-danger mx-auto'> </i>";
                }
            }
        },
        {
            "data": "defectcode",
            "width": "10%"
        },
        {
            data: null,
            render: function (data, type, row) {

                if (row.csexsw_documentos.length > 0) {
                    //mostrar archivos 
                    return "<button data-id=" + row.id + " class='btn btn-sm btn-info btn_files'><i class='fa fa-eye'> </i> </button>";


                } else {
                    //boton para agregar nuevos 
                    return "<button  data-id=" + row.id + " class='btn btn-sm btn-success btn_files'><i class='fa fa-plus'> </i> </button>";
                }
            }

        },

        //{
        //    //D3
        //    data: null,
        //    render: function (data, type, row) {

        //        var count = 0;
        //        $.each(row.csexsw_d3, function (i,b) {
        //            if (b.status == "Complete") {
        //                count++;
        //            }

        //        });
        //        var st = "";
        //        if (count == row.csexsw_d3.length && row.csexsw_d3.length>0) {
        //            st = "<span class='badge badge-success'> Complete </span>";
        //        }
        //        else {
        //            st = "<span class='badge badge-warning'> Pending </span>";
        //        }
        //        //return "D3: " +count +"/"+ row.csexsw_d3.length;
        //        return  st;
        //    }
        //    ,
        //    "width": "5%"

        //},

        //{
        //    //D5
        //    data: null,
        //    render: function (data, type, row) {

        //        var count = 0;
        //        $.each(row.csexsw_d7_1, function (i, b) {
        //            if (b.status == "Complete") {
        //                count++;
        //            }

        //        });
        //        var st = "";
        //        if (count == row.csexsw_d7_1.length && row.csexsw_d7_1.length >0) {
        //            st = "<span class='badge badge-success'> Complete </span>";
        //        }
        //        else {
        //            st = "<span class='badge badge-warning'> Pending </span>";
        //        } 
        //        return st;
        //    }
        //    ,
        //    "width": "5%"

        //},
        //{
        //    //D6
        //    data: null,
        //    render: function (data, type, row) {

        //        var count = 0; 
        //        $.each(row.csexsw_d6, function (i, b) {
        //            if (b.status == "Complete") {
        //                count++;
        //            }

        //        });
        //        var st = "";
        //        if (count == row.csexsw_d6.length && row.csexsw_d6.length >0) {
        //            st = "<span class='badge badge-success'> Complete </span>";
        //        }
        //        else {
        //            st = "<span class='badge badge-warning'> Pending </span>";
        //        }
        //        return  st; 
        //    }
        //    ,
        //    "width": "5%"

        //},

        {
            "data": "id",
            "render": function render(data, type, row) {
                if (row.status !== "Closed") {

                    if ($("#txtuser").val() == row.owner || $("#fname").val() == row.assignedto) {
                        return "<a   href='/Client/ReportsPdf/D8report/?id=" + row.id + "' style='cursor:pointer; '> <i class='fas fa-edit' style='color: green;'></i>  Edit</a> <a onclick=DeleteCAR('" + row.id + "') style='cursor:pointer; '> <i class='fas fa-trash-alt' style='color: red;'></i> Delete </a> ";
                    }
                    else {
                        return "<a href='/Client/ReportsPdf/D8report/?id=" + row.id + "'> <i class='fa fa-eye'></i> View </a>";
                    }
                } else {

                    if ($("#txtuser").val() == row.owner) {
                        return "<a   href='/Client/ReportsPdf/D8report/?id=" + row.id + "' style='cursor:pointer; '> <i class='fas fa-edit' style='color: green;'></i>  Edit</a>";

                    } else {
                        return "<a href='/Client/ReportsPdf/D8report/?id=" + row.id + "'> <i class='fa fa-eye'></i> View </a>";
                    }

                }
            },
            "width": "10%"
        }


        ],
        "language": {
            "emptyTable": "No records"
        }
    });


    $(document).on("click", ".btn_files", function () {
        let id = $(this).data("id");
        reloadCarEvidence(id);
        $("#txt_carid").val(id);
        $('#m_add_files').modal('show');
    });
}


//car list 2
function cargarDatatableCar3() {

    dataTable = $("#tblCar").DataTable({
        destroy: true,
        dom: "btpi",
        "order": [[0, "desc"]],
        "ajax": {
            "url": "/Client/corrective/GetAllcar3",
            "type": "GET",
            "dataType": "json"
        },
        "columns": [{
            "data": "id",
            "width": "5%"
        }, {
            "data": "partnumber",
            "render": function render(data) {
                return "" + data;
            },
            "width": "5%"
        },
        {
            "data": "owner_name",
            render: function (data) {
                return "<b>" + data + "</b>";
            },
            "width": "10%"
        },
        {
            "data": "assignedto",
            render: function (data) {
                var name = data;
                if (data == "" || data == null) {
                    return "<span class='badge badge-secondary p-1'> Unnassigned </span>";
                } else {
                    return "<span class='badge badge-primary p-1'> " + name + " </span>";
                }
            },
            "width": "10%"
        },
        {
            "data": "rmanumber",
            "width": "5%"
        }, {
            "data": "category",
            "width": "10%"
        }, {
            "data": "department",
            "width": "10%"
        }, {
            "data": "status",
            render: function (data) {
                switch (data) {
                    case "In Process":
                        return "<span class='badge badge-warning p-1'>" + data + "</span>";
                        break;
                    case "Rejected":
                        return "<span class='badge badge-danger p-1'>" + data + "</span>";
                        break;
                    case "Closed":
                        return "<span class='badge badge-success p-1'>" + data + "</span>";
                        break;
                    case "Delayed":
                        return "<span class='badge badge-danger p-1'>" + data + "</span>";
                        break;
                    default:
                        break;
                }
            },
            "width": "5%"
        },
        {
            data: "sumbit",
            render: function (data) {

                if (data == true) {
                    return "<i class='fa w-100 text-center fa-check text-success mx-auto'> </i>";
                } else {
                    return "<i class='fa w-100 text-center fa-times text-danger mx-auto'> </i>";
                }
            },
            "width": "5%"
        },
        {
            "data": "defectcode",
            "width": "10%"
        }, {
            "data": "id",
            "render": function render(data) {
                return '<a target="_blank"  style="cursor:pointer;" href="/Client/ReportsPdf/report8d/' + data + '">8D/</a> <a target="_blank"  style="cursor: pointer; " href="/Client/ReportsPdf/reportWhy2/' + data + '">Why/</a><a target="_blank"  style="cursor: pointer; " href="/Client/ReportsPdf/cause/' + data + '">cause</a>';
            },
            "width": "10%"
        }],
        "language": {
            "emptyTable": "No hay registros"
        }
    });
    $("#txtfilter_car_list").keyup(function () {
        dataTable.search($(this).val()).draw();
    });
}
function cargarDatatablePart() {
    var release = "";

    dataTable = $("#tblParts").DataTable({
        "ajax": {
            "url": "/Client/parts/GetAllparts",
            "type": "GET",
            "dataType": "json"
        },
        "columns": [{
            "data": "part_num",
            "render": function render(data) {
                release = data;
                return "" + data;
            },
            "width": "10%"
        }, {
            "data": "calb_type",
            "width": "10%"
        }, {
            "data": "model",
            "width": "10%"
        }, {
            "data": "serial",
            "width": "10%"
        }, {
            "data": "description",
            "width": "20%"
        }, {
            "data": "area",
            "width": "10%"
        }, {
            "data": "responsable",
            "width": "10%"
        }, {
            "data": "status",
            "width": "10%"
        }, {
            "data": "id",
            "render": function render(data) {
                return "<a  href='/Client/Parts/PartsEdit/" + release + "'  style='cursor:pointer;'> <i class='fas fa-edit' style='color: green;'></i>  Edit</a> <a onclick=DeleteComenta(" + data + ")  style='cursor:pointer;'> <i class='fas fa-trash-alt' style='color: red;'></i>  </a> ";
            },
            "width": "10%"
        }],
        "language": {
            "emptyTable": "No records found"
        }
    });
}

function name(encargado) {
    var arr = [];
    $.ajax({
        url: "/Client/Tasks/GetEmployeeName/?encargado=" + encargado,
        type: "GET",
        dataType: "json",
        success: function success(r) {
            arr[0] = r;
        }
    });
    return arr[0];
}

//Document Control
function cargarDatatableDocumentControl() {
    var now = new Date();
    var datetime = now.getFullYear() + '/' + (now.getMonth() + 1) + '/' + now.getDate();
    datetime += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds();
    var release = "";

    var filtro = document.getElementById("valor").value;

    var tbl = dataTable = $("#tblTasks").DataTable({
        destroy: true,
        responsive: true,
        dom: "btpi",
        order: [[0, 'desc']],
        "ajax": {
            "url": "/Client/Tasks/GetAllRelease/?fecha=" + datetime + "&filtro=" + filtro,
            "type": "GET",
            "dataType": "json"
        },
        "columns": [{
            "data": "version",
            "render": function render(data) {
                console.log(data);
                release = data;
                return "" + data;
            },
            "width": "10%"
        },
        {
            "data": "encargado",
            render: function (r) {
                return r;
            },
            "width": "10%"
        },
        {
            "data": "fullname",
            render: function (data, type, row) {
                return row.fullname;
            },
            "width": "10%"
        },
        {
            "data": "proyecto",
            "width": "15%"
        },
        {
            "data": "descripcion",
            "width": "15%"
        },

        {
            "data": "fecha",
            render: function (data, type, row) {
                return "<b style='font-size:12px;'>" + row.fecha + "</b>";
            },
            "width": "10%"
        },
        {
            "data": "estado",
            "render": function render(data, type, row) {
                if (data == "Approved-Released") {
                    return "<span class='badge badge-primary p-1'>Approved-Released</span>";
                }
                else if (data == "Rejected") {
                    return "<span class='badge badge-danger p-1'>Rejected</span>";
                }
                else if (data == "Pending") {
                    return "<span class='badge badge-warning p-1'>Pending</span>";
                }
                else if (data == "Unnapproved") {
                    return "<span class='badge badge-secondary p-1'>Unnapproved</span>";
                }
            }
        },
        {
            "data": "category",
            render: function (data, type, row) {
                return data;
            },
            "width": "10%"
        },
        {
            "data": "id",
            "render": function render(data, type, row) {
                var tags = "";
                if (row.category == "BOM release") {
                    tags = "<a data-tid=" + row.id + " class='btn btn-sm btn-link' id='btn-viewBOM' data-version=" + row.version + "><i class='fa fa-eye'> </i> View Details</a> <a data-toggle='modal' data-target='#Rechazar' onclick=Disapprove(" + data + ") class='btn btn-sm btn-link m-0 p-0' > <i class='fa fa-trash text-danger'> </i> Reject </a>  <button data-id=" + row.id + " class='btn btn-sm btn-link m-0 p-0' id='btnApprove'> <i class='fa fa-check-circle text-success'> </i> Approve</button>        <button data-id='" + row.id + "' id='btnReset' type='button' class='btn btn-sm btn-link text-secondary btn-sm m-0 p-0'> <i class='fa fa-backward'></i> Reset Approvals</button>   ";

                }
                else {
                    var method = '';
                    if (row.category == "Drawing Release") {
                        method = "abrir_dw_release";
                    } else {
                        method = "Abrir3";
                    }
                    if (row.statusaprobado == true) {
                        tags = " <a data-toggle='modal' data-target='#documents' onclick=" + method + "(" + data + ",'" + release + "')  style='cursor:pointer; color: blue; text-decoration: none;'><i class='far fa-edit' style='color: blue;'></i> Details </a> ";

                    } else {

                        tags = " <a data-toggle='modal' data-target='#documents' onclick=" + method + "(" + data + ",'" + release + "')  style='cursor:pointer; color: blue; text-decoration: none;'><i class='far fa-edit' style='color: blue;'></i> Details  </a><a data-toggle='modal' data-target='#Rechazar' onclick=Disapprove(" + data + ") style='cursor:pointer; color: red;text-decoration: none; '> Rejected  </a>";
                    }
                }
                return tags;
                //if (row.puedeaprobar == true) {
                //    return " <a data-toggle='modal' data-target='#documents' onclick=Abrir3(" + data + ",'" + release + "')  style='cursor:pointer; color: blue; text-decoration: none;'><i class='far fa-edit' style='color: blue;'></i> Details </a><a data-toggle='modal' data-target='#Rechazar' onclick=Disapprove(" + data + ") style='cursor:pointer; color: red;text-decoration: none; '> Rejected  </a> <a onclick=AprobarDC('/Client/Tasks/Apruebo/?id=" + data + "') style='text-decoration: none; cursor:pointer; color: green;'> Approve </a> ";
                //}
                //else {
                //}
            },
            "width": "20%"
        }
        ],
        "language": {
            "emptyTable": "No records found."
        }
    });


    function BOMLIST2(tareaid, version) {
        //BOMList
        $.ajax({
            url: "/Client/Tasks/BOMList?id=" + tareaid,
            type: "GET",
            datatype: "json",
            cache: true,
            success: function (data) {
                console.log("files");
                console.log(data);
                var fname = "";
                if (data.length > 0) {
                    $.each(data, function (i, b) {
                        fname += "<a href='/Client/Tasks/GetBomFile/?release=" + version + "&nombrepdf=&filename=" + b.filename + "' class='btn btn-link border-0 btn-showfile'>" + b.filename + "</a><br>";
                    });
                    Swal.fire({
                        title: "Documents",
                        text: "",
                        html: `${fname}`
                    });


                } else {
                    Swal.fire({
                        title: "There are no documents",
                        icon: "info"
                    });
                }


            }
        });



    }
    //show bom
    $(document).on("click", "#btn-viewBOM", function () {
        var tarea = $(this).data("tid");
        var version = $(this).data("version");
        //BOMLIST("#tbl_details",$(this).data("tid"));
        //$("#m_details").modal("show");

        BOMLIST2(tarea, version);


    });
    $("#txtFilter").focus();
    $("#txtFilter").keyup(function () {
        dataTable.search($(this).val()).draw();
    });
    $("#sReleases").change(function () {
        let text = $(this).children("option:selected").text();
        dataTable.column(6).search(text).draw();
    });

    $(document).on("click", "#btnApprove", function () {
        let id = $(this).data("id");
        Swal.fire({
            title: 'Are you sure you approve?',
            text: "You won't be able to revert this!",
            icon: 'question',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, approve it!'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: "/Client/Tasks/Approveit?id=" + id,
                    type: "GET",
                    datatype: "json",
                    cache: true,
                    success: function (data) {
                        if (data > 0) {
                            cargarDatatableDocumentControl();
                            Swal.fire(
                                'Approved it!',
                                'Your release has been approve it.',
                                'success'
                            )
                        }

                    }

                });


            }
        })
    });

    $(document).on("click", "#btnReset", function () {
        let id = $(this).data("id");
        Swal.fire({
            title: 'Are you sure to reset approvals #' + id + '?',
            text: "You won't be able to revert this!",
            icon: 'question',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, reset it!'
        }).then((result) => {
            if (result.isConfirmed) {

                $.ajax({
                    url: "/Client/Tasks/ResetAll?id=" + id,
                    type: "GET",
                    datatype: "json",
                    cache: true,
                    success: function (data) {
                        if (data > 0) {
                            cargarDatatableDocumentControl();
                            Swal.fire(
                                'Reseted it!',
                                'Your release and tasks has been reseted it.',
                                'success'
                            )
                        }

                    }

                });
            }
        })
    });
}
//My Releases
function cargarDatatableTasks2() {
    var bandera = true;
    var now = new Date();
    var datetime = now.getFullYear() + '/' + (now.getMonth() + 1) + '/' + now.getDate();
    datetime += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds();
    var release = "";


    dataTable = $("#tblTasks").DataTable({
        "ajax": {
            "url": "/Client/Tasks/GetAllnoadmin/?fecha=" + datetime,
            "type": "GET",
            "dataType": "json"
        },
        dom: 'btpi',
        order: [[0, 'desc']],
        "columns": [

            {
                "data": "version",
                "render": function render(data) {
                    release = data;
                    return "" + data;
                },
                "width": "10%"
            }
            ,
            {
                "data": "fullname",
                title: 'Prepared By',
                render: function (data, type, row) {
                    return row.fullname;
                },
                "width": "20%"
            },
            //{title: "Nombre",
            //"data": "fullname",
            //    render: function (data, type, row) {
            //        return row.fullname;

            //    },
            //    "width":"10%"
            //},
            {
                "data": "proyecto",
                "width": "10%"
            }, {
                "data": "descripcion",
                "width": "20%"
            }, {
                "data": "fecha",
                render: function (data, type, row) {
                    return "<b style='font-size:12px;'>" + row.fecha + "</b>";
                },
                "width": "20%"
            },

            //{
            //    "data": "statusaprobado",
            //    "render": function render(data, type, row) {

            //        if (row.statusaprobado == true) {
            //            return "<span class='badge badge-primary p-1'>Released</span>";
            //        } else {
            //            if (row.statusrechazado == true) {
            //                return "<span class='badge badge-danger p-1'>Rejected</span>";
            //            }
            //            else {
            //                return "<span class='badge badge-warning p-1'>Pending</span>";
            //            }
            //        }
            //        //if (data == true) {
            //        //    return "<span class='badge badge-primary p-1'>Approved-Released</span>";
            //        //} else {
            //        //    if (row.Statusrechazado == true) {
            //        //        return
            //        //        "<span class='badge badge-danger p-1'>Rejected</span>";
            //        //    } else {
            //        //        return "<span class='badge badge-warning p-1'>Pending</span>";
            //        //    }
            //        //}
            //    }
            //},

            {
                "data": "estado",
                "render": function render(data, type, row) {
                    if (row.estado == "Released") {
                        return "<span class='badge badge-primary p-1'>Released</span>";
                    }
                    else if (row.estado == "Rejected") {
                        return "<span class='badge badge-danger p-1'>Rejected</span>";
                    }
                    else if (row.estado == "Pending") {
                        return "<span class='badge badge-warning p-1'>Pending</span>";
                    }
                    else if (data == "Unnaproved") {
                        return "<span class='badge badge-secondary p-1'>Unnapproved</span>";
                    }
                }
            },

            //{
            //"data": "category",
            //"width": "10%"
            //}
            {
                "data": "id",
                "render": function render(data, type, row) {
                    if (row.statusaprobado == true) {
                        return "<a href='/Client/Tasks/Edit/" + release + "'  style='cursor:pointer; text-decoration: none;'> <i class='fas fa-edit' style='color: green;'></i>  Edit</a>  ";

                    }
                    else {
                        return "<a href='/Client/Tasks/Edit/" + release + "'  style='cursor:pointer; text-decoration: none;'> <i class='fas fa-edit' style='color: green;'></i>  Edit</a> <a onclick=Delete('/Client/Tasks/Delete/" + data + "')  style='cursor:pointer; text-decoration: none; '> <i class='fas fa-trash-alt' style='color: red;'></i> Delete </a> ";

                    }
                },
                "width": "30%"
            }

        ],
        "language": {
            "emptyTable": "No records found."
        }
    });
    $("#txtFilter").focus();
    $("#txtFilter").keyup(function () {
        dataTable.search($(this).val()).draw();
    });
}
//Releases
function cargarDatatableTasks() {
    var release = "";

    dataTable = $("#tblTasks").DataTable({
        destroy: true,
        responsive: true,
        dom: "btpi",
        order: [[0, 'desc']],
        "ajax": {
            "url": "/Client/Tasks/GetAll",
            "type": "GET",
            "dataType": "json"
        },
        "columns": [{
            "data": "version",
            "render": function render(data) {
                release = data;
                return "" + data;
            },
            "width": "10%"
        },



        {
            "data": "fullname",
            "width": "10%"
        }
            //,
            //{
            //    data: "fullname",
            //    width: "15%"
            //}
            ,
        {
            "data": "proyecto",
            "width": "10%"
        }, {
            "data": "descripcion",
            "width": "10%"
        }, {
            "data": "fecha",
            render: function (data, type, row) {
                return "<b style='font-size:12px;'>" + row.fecha + "</b>";
            },
            "width": "15%"
        },
        //{
        //    "data": "statusaprobado",
        //    "render": function render(data, type, row) {
        //        if (row.statusaprobado == true) {
        //            return "<span class='badge badge-primary p-1'>Released</span>";
        //        } else {
        //            if (row.statusrechazado == true) {
        //                return "<span class='badge badge-danger p-1'>Rejected</span>";
        //            }
        //            else {
        //                return "<span class='badge badge-warning p-1'>Pending</span>";
        //            }
        //        }
        //    }
        //},
        //{
        //"data": "category",
        //"width": "10%"
        //}
        {
            "data": "estado",
            "render": function render(data, type, row) {
                if (row.estado == "Released") {
                    return "<span class='badge badge-primary p-1'>Released</span>";
                }
                else if (row.estado == "Rejected") {
                    return "<span class='badge badge-danger p-1'>Rejected</span>";
                }
                else if (row.estado == "Pending") {
                    return "<span class='badge badge-warning p-1'>Pending</span>";
                }
                else if (data == "Unnaproved") {
                    return "<span class='badge badge-secondary p-1'>Unnapproved</span>";
                }
            }
        },

        {
            "data": "category",
            render: function (data, type, row) {
                return data;
            },
            "width": "10%"
        },
        {
            "data": "id",
            "render": function render(data, type, row) {
                if (row.statusaprobado == true) {
                    return "<a href='/Client/Tasks/Edit/" + release + "'   class='btn btn-sm btn-link m-0 p-0' title='Edit Release'> <i class='fa fa-edit text-success' ></i> Edit</a> ";

                }
                else {
                    //<button title='Reset Approvals' data-id='" + row.id + "' id='btnResetRE' type='button' class='btn btn-sm btn-link btn-sm m-0 p-0'> <i class='fa fa-backward text-secondary'></i> Reset Approvals</button>
                    return "<a href='/Client/Tasks/Edit/" + release + "'   class='btn btn-sm btn-link m-0 p-0' title='Edit Release'> <i class='fa fa-edit text-success' ></i> Edit</a> <a class='btn btn-sm btn-link m-0 p-0' onclick=Delete('/Client/Tasks/Delete/" + data + "')  > <i  class='fa fa-trash text-danger'></i> Delete  </a>   ";

                }
            },
            "width": "15%"
        }],
        "language": {
            "emptyTable": "No records found."
        }
    });
    $("#txtFilter").focus();
    $("#txtFilter").keyup(function () {
        dataTable.search($(this).val()).draw();
    });

    $(document).on("click", "#btnResetRE", function () {

        let id = $(this).data("id");
        swal({
            title: 'Are you sure to reset approvals #' + id + '?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            dangerMode: true,
            buttons: ["Cancel", "Yes"],
        }).then(function (result) {
            if (result) {

                $.ajax({
                    url: "/Client/Tasks/ResetAll?id=" + id,
                    type: "GET",
                    datatype: "json",
                    cache: true,
                    success: function (data) {
                        if (data > 0) {
                            cargarDatatableTasks();
                            swal(
                                'Reseted it!',
                                'Your release and tasks has been reseted it.',
                                'success'
                            )
                        }

                    }

                });
            }
        })
    });
}
function cargarDatatablestandar() {

    dataTable = $("#tblGages").DataTable({
        "ajax": {
            "url": "/Client/parts/GetAllstandar2",
            "type": "GET",
            "dataType": "json"
        },
        "columns": [{
            "data": "id",
            "width": "10%"
        }, {
            "data": "description",
            "width": "20%"
        }, {
            "data": "brand",
            "width": "10%"
        }, {
            "data": "serial",
            "width": "10%"
        }, {
            "data": "cert",
            "width": "10%"
        }, {
            "data": "calduedate",
            "width": "20%"
        }, {
            "data": "id",
            "render": function render(data) {
                return "<div class='text-center'> <a onclick=Delete('/Client/Parts/DeleteStandar/" + data + "') style='cursor:pointer;'> <i class='fas fa-trash-alt' style='color: red;'></i>  </a> ";
            },
            "width": "20%"
        }],
        "language": {
            "emptyTable": "No records found"
        }
    });
}
//approval control car
function cargarDatatableCar2() {

    var release = "";
    console.log(usuario);
    var info;
    if (parseInt($("#txt_approver").val()) > 0) {
        info = {
            "url": "/Client/corrective/GetAllcar2",
            "type": "GET",
            "dataType": "json"
        };
    } else {
        info = null;
    }

    dataTable = $("#tblCar").DataTable({
        destroy: true,
        dom: "btpi",
        pageLength: 10,
        "order": [[0, "desc"]],
        "ajax": info,
        "columns": [{
            "data": "id",
            "width": "10%"
        }, {
            "data": "partnumber",
            "render": function render(data) {
                release = data;
                return "" + data;
            },
            "width": "10%"
        }, {
            "data": "owner",
            "width": "10%"
        }, {
            "data": "rmanumber",
            "width": "10%"
        }, {
            "data": "category",
            "width": "10%"
        }, {
            "data": "department",
            "width": "10%"
        }, {
            "data": "status",
            render: function (data, type, row) {
                switch (data) {
                    case "In Process":
                        return "<span class='badge badge-warning p-1'>" + data + "</span>";
                        break;
                    case "Rejected":
                        return " <span class='badge badge-danger p-1'>" + data + " </span> <a class='btn btn-sm m-0 p-0 btn-link' id='btn-reject'><i class='fa fa-info-circle text-danger'> </i></a> <div style='display:none;'id='alert_reject' class='alert alert-danger position-absolute p-0' role='alert'><textarea readonly id='txtreject' style='height:150px; resize:none;' class='form-control  m-0 form-control-sm'>" + row.reject_comments + "</textarea></div>";
                        break;
                    case "Closed":
                        return "<span class='badge badge-success p-1'>" + data + "</span>";
                        break;
                    case "Delayed":
                        return "<span class='badge badge-danger p-1'>" + data + "</span>";
                        break;
                    default:
                        break;
                }
            },
            "width": "10%"
        },
        {
            data: "sumbit",
            render: function (data) {

                if (data == true) {
                    return "<i class='fa w-100 text-center fa-check text-success mx-auto'> </i>";
                } else {
                    return "<i class='fa w-100 text-center fa-times text-danger mx-auto'> </i>";
                }
            },
            "width": "5%"
        },
        {
            "data": "defectcode",
            "width": "10%"
        }, {
            "data": "id",
            "render": function render(data, type, row) {



                if (usuario_car == row.owner_name) {
                    return "<a disabled href='/Client/ReportsPdf/D8report/?id=" + data + "'   > <i class='fas fa-edit' style='color: green;' ></i>  Edit</a>  <a class='ml-2'  onclick=Approve('/Client/Corrective/Apruebo/?id=" + data + "') style='cursor:pointer; color: green;'><i class='fa fa-check'></i> Approve </a>    <a class='ml-2' onclick=Rejected('/Client/Corrective/Rechazar/?id=" + data + "&comments=') style='cursor:pointer; color: red;'><i class='fa fa-times'></i> Reject </a>";

                }
                else {
                    return "<a href='/Client/ReportsPdf/D8report/?id=" + data + "'  style='cursor:pointer; '> <i class='fas fa-edit' style='color: green;'></i>  Edit</a>  ";


                }




                return "";
            },
            "width": "20%"
        }],
        "language": {
            "emptyTable": "No records found."
        }
    });

    $(document).on("click", "#btn-reject", function () {

        $("#alert_reject").slideToggle();
        $("#txtreject").focus();
    });

    $("#txtfilter_control_car").keyup(function () {
        dataTable.search($(this).val()).draw();
    });
}
function cargarDatatableRoles() {

    dataTable = $("#tblRoles").DataTable({
        "ajax": {
            "url": "/Client/Tasks/GetAlluser",
            "type": "GET",
            "dataType": "json"
        },
        "columns": [{
            "data": "id",
            "width": "10%"
        }, {
            "data": "fullname",
            "width": "20%"
        }, {
            "data": "rol",
            "width": "50%"
        }, {
            "data": "id",
            "render": function render(data) {
                return " <a  onclick=Delete('/Client/Tasks/DeleteRol/" + data + "')  style='color:red; cursor:pointer;'>  Delete </a> ";
            },
            "width": "20%"
        }],
        "language": {
            "emptyTable": "No records found."
        }
    });
}
function cargarDatatableGages() {

    dataTable = $("#tblGages").DataTable({
        "ajax": {
            "url": "/Client/parts/GetAllgages",
            "type": "GET",
            "dataType": "json"
        },
        "columns": [{
            "data": "id",
            "width": "20%"
        }, {
            "data": "medicion",
            "width": "50%"
        }, {
            "data": "id",
            "render": function render(data) {
                return "<div class='text-center'> <a onclick=Delete('/Client/Parts/DeleteGage/" + data + "') style='cursor:pointer; '> <i class='fas fa-trash-alt' style='color: red;'></i>  </a> ";
            },
            "width": "30%"
        }],
        "language": {
            "emptyTable": "No records found"
        }
    });
}
function modal() {
    if (navigator.userAgent.match(/msie/i) || navigator.userAgent.match(/trident/i)) { } else {
        var cargarDatatable2 = function cargarDatatable2() {

            dataTable = $("#tblrma").DataTable({
                "scrollX": true,
                "ajax": {
                    "url": "/Client/corrective/GetAllM",
                    "type": "GET",
                    "dataType": "json"
                },
                "columns": [{
                    "data": "item_no",
                    "render": function render(data, type, row) {
                        return "<a data-dismiss='modal' onclick=Mover(" + "'" + data.trim() + "'" + ")>" + data + "</a>";
                    }
                }, {
                    "data": "item_desc_1"
                }],
                "language": {
                    "emptyTable": "No records"
                }
            });
        };

        $("#inputpart").hide();
        $("#inputdes").hide();
        $(document).ready(function () {


            cargarDatatable2();


        });
    }
}

function cargarDatatableCustumer() {

    dataTable = $("#tblA").DataTable({
        "autoWidth": false,


        "ajax": {
            "url": "/client/corrective/GetAllA",
            "type": "GET",
            "dataType": "json"
        },
        "columns": [{
            "data": "cus_no",
            "render": function render(data, type, row) {
                //onclick=Mover5(" + "'" + data.trim() + "'" + ")
                return "<a data-dismiss='modal' data-cus=" + row.cus_no + " >" + data.trim() + "</a>";
            }
        },
        {
            "data": "cus_name"
        }],

        "language": {
            "emptyTable": "No records found"
        }
    });





}
function loadcodes() {

    $.ajax({
        url: "/Client/corrective/GetAllS/",
        type: "GET",
        dataType: "json",
        async: true,
        cache: true,
        success: function (data) {
            console.log("codes all");
            console.log(data.data);
            $("#defectinput").html("");
            $("#defectinput").append("<option value='0'>-Select defect code-</option>");
            $.each(data.data, function (i, b) {
                $("#defectinput").append("<option value=" + b.sy_code + ">" + b.code_desc + "</option>");
            });
        }

    });

    dataTable = $("#tblcodes").DataTable({
        destroy: true,
        "autoWidth": false,
        deferRender: true,
        "ajax": {
            "url": "/client/corrective/GetAllS",
            "type": "GET",
            "dataType": "json"
        },
        "columns": [{
            "data": "sy_code",
            "render": function render(data, type, row) {
                return "<a class='btn-link' id='btnreject3' data-id=" + data.trim() + " data-desc=" + row.code_desc + ">" + data + "</a>";
            }
        }, {
            "data": "code_desc"
        }],
        "language": {
            "emptyTable": "No records found"
        }
    });

    //rcodeline
    $(document).on("click", "#btnreject3", function () {
        let id = $(this).data("id");
        let desc = $(this).data("desc");
        //$("#defectinput").val(id);
        $("#rcodenew").val("");
        $("#rcodenew").val(id);
        $("#mcodes").modal("hide");
    });

}

function loadcodes2() {



    dataTable = $("#tblcodes2").DataTable({
        destroy: true,
        "autoWidth": false,
        deferRender: true,
        "ajax": {
            "url": "/client/corrective/GetAllS",
            "type": "GET",
            "dataType": "json"
        },
        "columns": [{
            "data": "sy_code",
            "render": function render(data, type, row) {
                return "<a class='btn-link' id='btnrejectline' data-id=" + data.trim() + " data-desc=" + row.code_desc + ">" + data + "</a>";
            }
        }, {
            "data": "code_desc"
        }],
        "language": {
            "emptyTable": "No records found"
        }
    });
    //btnr 
    $(document).on("click", "#btnrejectline", function () {
        let id = $(this).data("id");
        let desc = $(this).data("desc");
        //$("#defectinput").val(id);
        $(".rcodeup").val("");
        $(".rcodeup").val(id);

        var rmaid = arrRmaId[0];
        let qty = id;
        let code = arrRCode[0];


        if (code == "") {
            $("#btnsavelinea-" + rmaid).prop("disabled", true);
        }
        else {
            if (code !== qty) {
                $("#btnsavelinea-" + rmaid).prop("disabled", false);
            }
            if (code == qty) {
                $("#btnsavelinea-" + rmaid).prop("disabled", true);
            }
        }

        $("#mcodes2").modal("hide");
    });

}
//function cargarDatatableDefectCode() {
//    alert("aqui es");
//    dataTable = $("#tblS").DataTable({
//        destroy: true,
//        deferRender: true,
//        "autoWidth": false,
//        "ajax": {
//            "url": "/client/corrective/GetAllS",
//            "type": "GET",
//            "dataType": "json"
//        },
//        "columns": [{
//            "data": "sy_code",
//            "render": function render(data, type, row) {
//                return "<a class='btn-link' id='btnreject2' data-id=" + data.trim() + " data-desc=" + row.code_desc + ">" + data + "</a>";
//            }
//        }, {
//            "data": "code_desc"
//        }],
//        "language": {
//            "emptyTable": "No records found"
//        }
//    });


//    $(document).on("click", "#btnreject2", function () {
//        let id = $(this).data("id");
//        let desc = $(this).data("desc");
//        //$("#defectinput").val(id);
//        $("#csexsw_costumber_Retur").val("");
//        $("#csexsw_costumber_Retur").val(id);
//        $("#tblSm").modal("hide");
//    });
//}
function cargarDatatabledrawingsall() {

    var release = "";
    var bandera = true;
    var id = document.getElementById("valor").value;
    dataTable = $("#tblDibujo").DataTable({
        "order": [[0, "desc"]],
        "ajax": {
            "url": "/Client/drawings/GetAll/" + id,
            "type": "GET",
            "dataType": "json"
        },
        "columns": [{
            "data": "id",
            "width": "5%"
        }, {
            "data": "csexsw_tarea.version",
            "render": function render(data) {
                release = data;
                return "" + data + "";
            },
            "width": "10%"
        }, {
            "data": "descripcion",
            "width": "20%"
        }, {
            "data": "status",
            "render": function render(data, type, row) {
                bandera = data;

                if (data == true) {
                    return "Approved Drawing";
                } else {
                    if (row.statusrechazado == true) {
                        return "Rejected";
                    } else {
                        return "Unapproved";
                    }
                }
            },
            "width": "10%"
        }, {
            "data": "dibujopdf",
            "render": function render(data, type, row) {
                if (row.csexsw_tarea.statusaprobado == true) {
                    return "<a  href=javascript:finestraSecundaria('../../../documents/approved_stamped/" + release + "/" + data + "')>" + data + "</a>";
                } else {
                    return "<a  href=javascript:finestraSecundaria('../../../documents/pending/" + release + "/" + data + "')>" + data + "</a>";
                }
            },
            "width": "30%"
        }, {
            "data": "id",
            "render": function render(data) {
                return "<a href='/Client/drawings/Edit/" + data + "' style='text-decoration: none; cursor:pointer;'>  <i class='fas fa-edit' style='color: green;'></i> Edit</a><a onclick=Delete('/Client/drawings/Delete/" + data + "')  style='text-decoration: none; cursor:pointer;'> <i class='fas fa-trash-alt' style='color: red;'></i>Delete </a><a   data-toggle='modal' data-target='#exampleModal' style='cursor:pointer; text-decoration: none;' onclick='Abrir(" + data + ")'> <i class='far fa-edit' style='color: blue;'></i>Details</a> ";
            },
            "width": "20%"
        }],
        "language": {
            "emptyTable": "No records found."
        }
    });
}
//My Rejects
function cargarDatatabledrawings2() {
    var now = new Date();
    var datetime = now.getFullYear() + '/' + (now.getMonth() + 1) + '/' + now.getDate();
    datetime += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds();
    var release = "";
    var statusrechazo = false;
    var bandera = true;
    var dib = 0;

    var status2 = document.getElementById("valor").value;
    var status = "status";
    dataTable = $("#tblDibujo").DataTable({
        "order": [[0, "desc"]],
        "ajax": {
            "url": "/Client/drawings/GetAll3/?status=" + status2 + "&fecha=" + datetime,
            "type": "GET",
            "dataType": "json"
        },
        "columns": [{
            "data": "csexsw_dibujo.id",
            "render": function render(data) {
                return "" + data;
            },
            "width": "5%"
        }, {
            "data": "csexsw_dibujo.csexsw_tarea.version",
            "render": function render(data) {
                release = data;
                return "" + data + "";
            },
            "width": "10%"
        }, {
            "data": "csexsw_dibujo.csexsw_tarea.fecha",
            render: function (data, type, row) {
                return "<b style='font-size:12px;'>" + row.csexsw_dibujo.csexsw_tarea.fecha + "</b>";
            },
            "width": "10%"
        }, {
            "data": "drawing",
            "width": "20%"
        }, {
            "data": "descripcion",
            "render": function render(data) {
                if (data == "Design Engineer") {
                    status = "status1";
                }

                if (data == "Drafting and Checking") {
                    status = "status2";
                }

                if (data == "Enginner in Charge") {
                    status = "status3";
                }

                if (data == "Finish spec ") {
                    status = "status4";
                }

                if (data == "Material spec") {
                    status = "status5";
                }

                if (data == "Assembly spec") {
                    status = "status6";
                }

                if (data == "High reliability drawing") {
                    status = "status7";
                }

                if (data == "Molding compound spec") {
                    status = "status8";
                }

                if (data == "Welding or special process") {
                    status = "status9";
                }

                return "" + data;
            },
            "width": "10%"
        }, {
            "data": "nombre",
            "width": "10%"
        }, {
            "data": "status",
            "render": function render(data, type, row) {
                bandera = data;

                if (data == true) {
                    return "<span class='badge badge-primary p-1'>Approved Drawing</span>";
                } else {
                    if (row.statusrechazo == true) {
                        return "<span class='badge badge-danger p-1'>Rejected</span>";
                    } else {
                        return "<span class='badge badge-secondary p-1'>Unapproved</span>";
                    }
                }
            },
            "width": "10%"
        }, {
            "data": "dibujopdf",
            "render": function render(data, type, row) {
                if (row.csexsw_dibujo.csexsw_tarea.statusaprobado == true) {
                    return "<a  href=javascript:finestraSecundaria('../../../documents/approved_stamped/" + row.csexsw_dibujo.csexsw_tarea.version + "/" + data + "') ><img src='../../../documento.png' alt=''></a>" + data;
                } else {
                    return "<a  href=javascript:finestraSecundaria('../../../documents/pending/" + row.csexsw_dibujo.csexsw_tarea.version + "/" + data + "') ><img src='../../../documento.png' alt=''></a>" + data;
                }
            },
            "width": "15%"
        }, {
            "data": "dibujoid",
            "render": function render(data, type, row) {
                dib = row.id;
                return "<a href='/Client/drawings/Edit/" + data + "'  style='cursor:pointer; text-decoration: none; '>  <i class='fas fa-edit' style='color: green;'></i> Edit</a><a  data-toggle='modal' data-target='#exampleModal' style='cursor:pointer; text-decoration: none; ' onclick='Abrir(" + data + ")'> <i class='far fa-edit' style='color: blue;'></i>Details</a>  ";
            },
            "width": "20%"
        }],
        "language": {
            "emptyTable": "No records found."
        }
    });
}
//My documents 
function cargarDatatabledrawings() {
    var now = new Date();
    var datetime = now.getFullYear() + '/' + (now.getMonth() + 1) + '/' + now.getDate();
    datetime += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds();

    var release = "";
    var statusrechazo = false;
    var bandera = true;
    var dib = 0;

    var status2 = document.getElementById("valor").value;
    var status = "status";


    dataTable = $("#tblDibujo").DataTable({
        destroy: true,
        responsive: true,
        dom: 'btpi',
        "order": [[0, "desc"]],
        "ajax": {
            "url": "/Client/drawings/GetAll2/?status=" + status2 + "&fecha=" + datetime,
            "type": "GET",
            "dataType": "json"
        },

        "columns": [{
            "data": "csexsw_dibujo.Id",
            "render": function render(data) {
                return "" + data;
            },
            "width": "5%"
        }, {


            //"data": "csexsw_dibujo.Id",
            "data": "csexsw_dibujo.csexsw_tarea.Version",
            "render": function render(data) {
                release = data;
                return "" + data + "";
            },
            "width": "5%"
        },

        {
            //"data": "csexsw_dibujo.Id",
            "data": "csexsw_dibujo.csexsw_tarea.Fecha",
            render: function (data, type, row) {
                return "<b style='font-size:12px;'>" + row.csexsw_dibujo.csexsw_tarea.Fecha + "</b>";
            },
            "width": "10%"
        }, {
            "data": "Drawing",
            "width": "10%"
        }, {
            "data": "Descripcion",
            "render": function render(data, type, row) {

                if (data == "Design Engineer") {
                    status = "status1";
                }

                if (data == "Drafting and Checking") {
                    status = "status2";
                }
                if (data == "Engineer in Charge") {
                    status = "status3";
                }

                if (data == "Finish spec ") {
                    status = "status4";
                }

                if (data == "Material spec") {
                    status = "status5";
                }

                if (data == "Assembly spec") {
                    status = "status6";
                }

                if (data == "High reliability drawing") {
                    status = "status7";
                }

                if (data == "Molding compound spec") {
                    status = "status8";
                }

                if (data == "Welding or special process") {
                    status = "status9";
                }
                if (data == "BOM Approver") {
                    status = "status10";
                }
                if (row.csexsw_dibujo.csexsw_tarea.category != "Drawing Release") {
                    status = "status";
                }


                return "" + data;
            },
            "width": "20%"
        }, {
            "data": "Nombre",
            "width": "10%"
        }, {
            "data": "Status",
            "render": function render(data, type, row) {
                bandera = data;
                if (data == true) {
                    return "<span class='badge badge-primary p-1'>Approved Drawing</span>";
                } else {
                    if (row.Statusrechazo == true) {
                        return "<span class='badge badge-danger p-1'>Rejected</span>";
                    } else {
                        return "<span class='badge badge-secondary p-1'>Unapproved</span>";
                    }
                }
            },
            "width": "10%"
        }, {
            "data": "Dibujopdf",
            "render": function render(data, type, row) {
                //return "<b>" + row.Dibujopdf + "</b>";
                if (row.csexsw_dibujo.sync == true) {
                    return "<a  class='btnpreview' data-folder='approved_stamped' href='#' data-r=" + row.csexsw_dibujo.csexsw_tarea.Version + " data-filename=" + data + " >" + row.csexsw_dibujo.Nombrepdf + "</a>";
                } else {
                    return "<a class='btnpreview' data-folder='pending' data-r=" + row.csexsw_dibujo.csexsw_tarea.Version + " data-filename=" + data + " >" + row.csexsw_dibujo.Nombrepdf + "</a>";
                }
            },
            "width": "10%"
        },
        {
            data: "assigned_to",
            render: function (data, type, row) {
                if (data !== "Not assigned") {
                    return "<span class='badge badge-primary'> " + data + "</span>";
                } else {
                    return "<span class='badge badge-secondary'> " + data + "</span>";
                }
            }
        },
        {
            data: null,
            render: function (data, type, row) {
                if (row.csexsw_dibujo.Part !== null) {
                    return "<span class='badge badge-primary'> " + row.csexsw_dibujo.Part + "</span>";
                } else {
                    return "";
                }
            }
        },
        {
            "data": "Dibujoid",
            "render": function render(data, type, row) {
                dib = row.Id;
                var st = "";
                switch (row.Descripcion.trim()) {
                    case "Design Engineer":
                        st = "status1";
                        break;
                    case "Drafting and Checking":
                        st = "status2";
                        break;
                    case "Engineer in Charge":
                        st = "status3";
                        break;
                    case "Finish spec":
                        st = "status4";
                        break;
                    case "Material spec":
                        st = "status5";
                        break;
                    case "Assembly spec":
                        st = "status6";
                        break;
                    case "High reliability drawing":
                        st = "status7";
                        break;
                    case "Molding compound spec":
                        st = "status8";
                        break;
                    case "Welding or special process":
                        st = "status9";
                        break;
                    case "BOM Approver":
                        st = "status10";
                        break;
                    default:
                        st = "status";
                        break;

                }
                //if (row.csexsw_dibujo.csexsw_tarea.category != "Drawing Release") {
                //    st = "status";
                //}
                if (row.Status == true) {
                    return "<a data-toggle='modal' data-target='#exampleModal' style='cursor:pointer; color: blue; text-decoration: none;' onclick='Abrir(" + data + ")'><i class='far fa-edit' style='color: blue,'></i>Details</a>   <button data-id='" + row.Id + "' id='btnReset' type='button' class='btn btn-secondary btn-sm'> <i class='fa fa-backward'></i> Reset Approvals</button>";
                }
                else {
                    if (row.Statusrechazo == true) {
                        return "<button type='button' class='btn btn-sm btn-outline-primary' data-dibujo=" + row.csexsw_dibujo.Id + " data-id=" + dib + " id='resubmit'>Re-submit</button>";

                    }
                    else {
                        var hidd_btn_reject = '';
                        var hidd_btn_assign = '';
                        var ruta = "";
                        var hidd_approve = '';
                        if (row.Nombre == "Process Engineer") {
                            if ($("#txt_fullname").val() !== row.assigned_to && row.assigned_to != "Not assigned") {
                                hidd_approve = "hidden";
                            }
                            hidd_btn_reject = 'hidden';
                            ruta = '"/Client/drawings/Apruebo_review/?id=' + data + '&status=' + st + '&dib=' + dib + '"';
                        } else {
                            hidd_btn_assign = 'hidden';
                            ruta = '"/Client/drawings/Apruebo/?id=' + data + '&status=' + st + '&dib=' + dib + '"';
                        }

                        return "<a data-toggle='modal' " + hidd_btn_reject + " data-target='#Rechazar' onclick=Disapprove2(" + dib + ",'" + status + "'," + data + ")  style='cursor:pointer; color: red; text-decoration: none;'> Rejected  </a>      <a onclick=Aprobar(" + ruta + ") " + hidd_approve + " style='cursor:pointer; color: green; text-decoration: none;'> Approve </a> <a  id='btnopenstatus' style='cursor:pointer; color: blue; text-decoration: none;' onclick='Abrir(" + data + ")' data-id=" + data + "><i class='far fa-edit' style='color: blue,'></i>Details</a> <button data-id=" + row.Id + "  " + hidd_btn_assign + " type='button' id='btn-assign' class='btn btn-xs p-1 btn-outline-primary'>Assign</button>";
                    }
                }
            },
            "width": "30%"
        }],
        "language": {
            "emptyTable": "No records found."
        }
    });
    $(document).on("click", "#btn-assign", function () {
        let id = $(this).data("id");
        $("#txt_aid").val(id);
        employees_by_rolname("Process Engineer");
        $("#massign").modal("show");
    });
    $(document).on("click", "#btn_close_assign", function () {
        $("#massign").modal("hide");
    });

    $(document).on("click", ".btnpreview", function () {
        //let folder = $(this).data("folder");
        //let fname = $(this).data("filename");
        //let rele = $(this).data("r");
        //$.ajax({
        //    url: "/Client/drawings/OpenPrewview",
        //    data: { folder: folder, release: rele, filename: fname },
        //    type: "GET",
        //    dataType: "json",
        //    cache: true,
        //    async: true,
        //    success: function (data) {
        //        if (data == '' || data == null) {
        //            Swal.fire({
        //                title: "The file does not exists",
        //                icon: "info"
        //            });
        //        }
        //        else {
        //            var pdfWindow = window.open("", "Title");
        //            pdfWindow.document.write("<iframe width='100%' height='100%' src='data:application/pdf;base64," + data + "'></iframe>");
        //        }
        //    }
        //});

        let folder = $(this).data("folder");
        let release = $(this).data("r");
        let filename = $(this).data("filename");

        let url = `/Client/drawings/OpenPrewview?folder=${folder}&release=${release}&filename=${filename}`;
        window.open(url, "_blank");

    });

    //

    $("#sCategories").change(function () {
        let text = $(this).children("option:selected").text();
        if (text == "All Task") {
            dataTable.column(4).search("").draw();
        }
        else {
            dataTable.column(4).search(text).draw();
        }
    });

    $("#txtFilter").focus();
    $("#txtFilter").keyup(function () {
        dataTable.search($(this).val()).draw();
    });
    $("#sStatus").change(function () {
        let text = $(this).children("option:selected").val();
        if (text == "Unapproved") {
            dataTable.column(6).search("Unapproved").draw();
        }
        else {
            dataTable.column(6).search(text).draw();
        }
    });



}
function assign(id, fname) {

    $.ajax({
        url: "/Client/drawings/assign/",
        data: { id: id, fullname: fname },
        dataType: "json",
        type: "GET",
        async: true,
        cache: true,
        success: function (result) {
            if (result == true) {
                //cargarDatatabledrawings();
                window.location.reload();
                $("#massign").modal("hide");
            }
        }
    });
}
function employees_by_rolname(role) {
    $.ajax({
        url: "/Client/Tasks/EmployeesByRolName/",
        data: { rolename: role },
        dataType: "json",
        type: "GET",
        async: true,
        cache: true,
        success: function (data) {
            var tblassign = $("#tbl_assign").DataTable({
                destroy: true,
                data: data,
                dom: "btpi",
                columns: [
                    { data: "empleadoID" },
                    {
                        data: "fullName",
                        render: function (data, type, row) {
                            return "<a class='btn-link' id='btnassignname' data-resid=" + row.empleadoID + ">" + data + "</a>";
                        }
                    }
                ],
                pageLength: 5
            });
            $(document).on("click", "#btnassignname", function () {
                var id = $("#txt_aid").val();
                var name = $(this).text();
                assign(id, name);
            });

            $("#txt_filter_assign").keyup(function () {
                tblassign.search($(this).val()).draw();
            });
        }
    });
}
function cargarDatatablefolios() {

    dataTable = $("#tblGages").DataTable({
        "order": [[1, "desc"]],
        "ajax": {
            "url": "/Client/parts/GetAllfolio/",
            "type": "GET",
            "dataType": "json"
        },
        "columns": [{
            "data": "id",
            "width": "20%"
        }, {
            "data": "folio",
            "width": "20%"
        }, {
            "data": "part_num",
            "width": "20%"
        }, {
            "data": "folio",
            "render": function render(data) {
                return " <a target='_blank' href='/Client/ReportsPdf/report/?folio=" + data + "' style='cursor:pointer; color: blue;'>  <i class='fas fa-edit' style='color: blue;'></i> Report</a> <a  href='/Client/Parts/editcalibrations/?folio=" + data + "' style='cursor:pointer; color: blue;'>  <i class='fas fa-edit' style='color: blue;'></i> Edit</a> ";
            },
            "width": "20%"
        }],
        "language": {
            "emptyTable": "No hay registros"
        }
    });
}
//reviewers
function cargarDatatablereviewers() {

    var now = new Date();
    var datetime = now.getFullYear() + '/' + (now.getMonth() + 1) + '/' + now.getDate();
    datetime += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds();
    var release = "";
    var statusrechazo = false;
    var bandera = true;
    var dib = 0;
    var status2 = document.getElementById("valor").value;
    var id = document.getElementById("valor2").value;
    var status = "status";
    dataTable = $("#tblDibujo").DataTable({
        "order": [[0, "desc"]],
        "ajax": {
            "url": "/Client/drawings/GetAll1/?id=" + id + "&status=" + status2 + "&fecha=" + datetime,
            "type": "GET",
            "dataType": "json"
        },
        dom: 'btpi',

        "columns": [{
            "data": "csexsw_dibujo.id",
            "render": function render(data) {
                return "" + data;
            },
            "width": "5%"
        }, {
            "data": "csexsw_dibujo.csexsw_tarea.version",
            "render": function render(data) {
                release = data;
                return "" + data + "";
            },
            "width": "5%"
        }, {
            "data": "csexsw_dibujo.csexsw_tarea.fecha",
            render: function (data, type, row) {
                return "<b style='font-size:12px;'>" + row.csexsw_dibujo.csexsw_tarea.fecha + "</b>";
            },
            "width": "10%"
        }, {
            "data": "drawing",
            "width": "10%"
        }, {
            "data": "descripcion",
            "render": function render(data) {
                if (data == "Design Engineer") {
                    status = "status1";
                }

                if (data == "Drafting and Checking") {
                    status = "status2";
                }

                if (data == "Enginner in Charge") {
                    status = "status3";
                }

                if (data == "Finish spec ") {
                    status = "status4";
                }

                if (data == "Material spec") {
                    status = "status5";
                }

                if (data == "Assembly spec") {
                    status = "status6";
                }

                if (data == "High reliability drawing") {
                    status = "status7";
                }

                if (data == "Molding compound spec") {
                    status = "status8";
                }

                if (data == "Welding or special process") {
                    status = "status9";
                }

                return "" + data;
            },
            "width": "20%"
        }, {
            "data": "nombre",
            "width": "20%"
        }, {
            "data": "status",
            "render": function render(data, type, row) {
                bandera = data;

                if (data == true) {
                    return "<span class='badge badge-primary p-1'>Approved Drawing</span>";
                } else {
                    if (row.statusrechazo == true) {
                        return "<span class='badge badge-danger p-1'>Rejected</span>";
                    } else {
                        return "<span class='badge badge-secondary p-1'>Unapproved</span>";
                    }
                }
            },
            "width": "10%"
        }, {
            "data": "dibujopdf",
            "render": function render(data, type, row) {
                if (row.csexsw_dibujo.csexsw_tarea.statusaprobado == true) {
                    return "<a  class='btnpreviewRE' data-folder='approved_stamped' data-r=" + row.csexsw_dibujo.csexsw_tarea.version + " data-filename=" + row.csexsw_dibujo.dibujopdf + " >" + row.csexsw_dibujo.nombrepdf + "</a>";
                } else {
                    return "<a  class='btnpreviewRE' data-folder='pending' data-r=" + row.csexsw_dibujo.csexsw_tarea.version + " data-filename=" + row.csexsw_dibujo.dibujopdf + " >" + row.csexsw_dibujo.nombrepdf + "</a>";

                }
            },
            "width": "30%"
        }
            //    {
            //    "data": "dibujoid",
            //    "render": function render(data, type, row) {
            //        dib = row.id;
            //        return "<a data-toggle='modal' data-target='#Rechazar' onclick=Disapprove2(" + dib + ",'" + status + "'," + data + ")  style='cursor:pointer; color: red; text-decoration: none;'> Rejected  </a> <a onclick=Aprobar('/Client/drawings/Apruebo/?id=" + data + "&status=" + status + "&dib=" + dib + "')  style='cursor:pointer; color: green; text-decoration: none;'> Approve </a> <a data-toggle='modal' data-target='#exampleModal' style='cursor:pointer; color: blue; text-decoration: none;' onclick='Abrir(" + data + ")'><i class='far fa-edit' style='color: blue,'></i>Details</a>";
            //    },
            //    "width": "30%"
            //}
        ],
        "language": {
            "emptyTable": "No records found."
        }
    });


    $(document).on("click", ".btnpreviewRE", function () {
        //let folder = $(this).data("folder");
        //let fname = $(this).data("filename");
        //let rele = $(this).data("r");
        //$.ajax({
        //    url: "/Client/drawings/OpenPrewview",
        //    data: { folder: folder, release: rele, filename: fname },
        //    type: "GET",
        //    dataType: "json",
        //    cache: true,
        //    async: true,
        //    success: function (data) {
        //        if (data == '' || data == null) {
        //            Swal.fire({
        //                title: "The file does not exists",
        //                icon: "info"
        //            });
        //        }
        //        else {
        //            var pdfWindow = window.open("", "Title");
        //            pdfWindow.document.write("<iframe width='100%' height='100%' src='data:application/pdf;base64," + data + "'></iframe>");
        //        }
        //    }
        //});

        let folder = $(this).data("folder");
        let release = $(this).data("r");
        let filename = $(this).data("filename");

        let url = `/Client/drawings/OpenPrewview?folder=${folder}&release=${release}&filename=${filename}`;
        window.open(url, "_blank");
    });

    $("#sRoles").change(function () {
        let text = $(this).children("option:selected").text();
        if (text == "All") {
            dataTable.column(4).search("").draw();
        }
        else {
            dataTable.column(4).search(text).draw();

        }
    });
    $("#sApprovers").change(function () {
        let text = $(this).children("option:selected").text();
        if (text == "All") {
            dataTable.column(5).search("").draw();
        }
        else {
            dataTable.column(5).search(text).draw();

        }
    });
    $("#txtFilter").focus();
    $("#txtFilter").keyup(function () {
        dataTable.search($(this).val()).draw();
    });
}
function zfill(number, width) {
    var numberOutput = Math.abs(number); /* Valor absoluto del n�mero */
    var length = number.toString().length; /* Largo del n�mero */
    var zero = "0"; /* String de cero */

    if (width <= length) {
        if (number < 0) {
            return ("-" + numberOutput.toString());
        } else {
            return numberOutput.toString();
        }
    } else {
        if (number < 0) {
            return ("-" + (zero.repeat(width - length)) + numberOutput.toString());
        } else {
            return ((zero.repeat(width - length)) + numberOutput.toString());
        }
    }
}
function pad(n, width, z) {
    z = z || '0';
    n = n + '';
    return n.length >= width ? n : new Array(width - n.length + 1).join(z) + n;
}





function Aprobar(url) {


    Swal.fire({
        title: 'Are you sure you want to approve?',
        icon: 'question',
        showCancelButton: true,
        confirmButtonColor: '#0275d8',
        cancelButtonColor: '#cd5c5c',
        confirmButtonText: 'Yes, approve it!'
    }).then((result) => {
        if (result.isConfirmed) {
            //Swal.fire({
            //    title: 'Approved!',
            //    text: 'Your document has been approved.',
            //    icon: 'success',
            //    showConfirmButton: false,
            //    timer: 2000
            //});
            $.ajax({
                type: 'PUT',
                url: url,

                success: function success(data) {

                    var d = JSON.parse(data);
                    if (d.success) {
                        toastr.success(d.message);
                        dataTable.ajax.reload();
                    } else {
                        toastr.error(d.message);
                    }
                }
            });
        }
    })


}
function AprobarDC(url) {

    Swal.fire({
        title: 'Are you sure you want to approve?',
        icon: 'question',
        showCancelButton: true,
        confirmButtonColor: '#0275d8',
        cancelButtonColor: '#cd5c5c',
        confirmButtonText: 'Yes, approve it!'
    }).then((result) => {
        if (result.isConfirmed) {
            //Swal.fire({
            //    title: 'Approved!',
            //    text: 'Your release has been approved.',
            //    icon: 'success',
            //    showConfirmButton: false,
            //    timer: 2000
            //});
            $.ajax({
                type: 'PUT',
                url: url,
                success: function success(data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    })

}
function Desaprobar(url) {
    $.ajax({
        type: 'PUT',
        url: url,
        success: function success(data) {
            if (data.success) {
                toastr.success(data.message);
                dataTable.ajax.reload();
            } else {
                toastr.error(data.message);
            }
        }
    });
}
function DeleteComenta(id) {
    swal({
        text: 'Are you sure to delete? (Write Reason)',
        content: "input",
        button: {
            text: "DELETE",
            closeModal: false
        }
    }).then(function (name) {
        if (name != null && name != "") {
            $.ajax({
                url: "/Client/Parts/DeletePart?id=" + id + "&reason=" + name,
                type: "GET",
                dataType: "json",
                cache: true,
                success: function success(data) {
                    console.log(data);
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    swal.stopLoading();
                    swal.close();
                },
                error: function error(xhr, status, _error21) {
                    $().toastmessage('showToast', {
                        text: 'Error con ean ' + xhr.responseText,
                        sticky: true,
                        type: 'error'
                    });
                }
            });
        }
    });
}
//delete drawing en Tasks/Edit/D0021
function Delete(url) {
    Swal.fire({
        title: "Are you sure to delete?",
        text: "This content cannot be recovered!",
        icon: "warning",
        buttons: true,
        confirmButtonText: "Yes",
        confirmButtonColor: "#d9534f",
        showConfirmButton: true,
        showCancelButton: true,
        dangerMode: true,

    }).then(function (willDelete) {


        if (willDelete.value) {
            $.ajax({
                type: 'DELETE',
                url: url,
                success: function success(data) {
                    if (data.success) {
                        toastr.success(data.message);
                        //dataTable.ajax.reload();
                        window.location.reload();
                    } else {
                        toastr.error(data.message);
                    }
                }
            });

        } else {
            console.log("ABORT")
            toastr.success("ABORT");
        }


    });
}
function Archivo(url) {
    $.ajax({
        type: 'PUT',
        url: url,
        success: function success(data) {
            finestraSecundaria(data.ruta);
            console.log(data.ruta);
        }
    });
}



function LoadPartNumbers() {
    $.ajax({
        url: "/client/corrective/GetPartNumbers/",
        type: "GET",
        dataType: "json",
        cache: true,
        async: true,
        success: function success(data) {
            var table = $("#tblPN").DataTable({
                destroy: true,
                data: data.data,
                deferRender: true,
                columns: [
                    {
                        "data": "id",
                        render: function (data, type, row) {
                            return '<a class="text-primary"  id="linkpn" data-pn=' + row.item_no + '>' + row.id + '</a>';
                        }
                    },
                    { "data": "item_no" },
                    { "data": "item_desc_1" }
                ]
            });

            $("#searchPN").keyup(function () {
                table.search($(this).val()).draw();
            });


        },
        complete: function () {
            //alert("complete");
            //$('body').loadingModal('hide');
            //$('body').loadingModal('destroy');
        },
        error: function error(xhr, status, _error17) {
            console.log(xhr.responseText);
        },
    });

    $(document).on("click", "#linkpn", function () {
        $("#pnnew").val($(this).data("pn"));
        $("#mPN").modal("hide");
    });
}
function existeloc(itemno, loc) {
    //ExisteLocation
    $.ajax({
        url: "/Client/Corrective/ExisteLocation/?itemno=" + itemno + "&loc=" + loc,
        type: "GET",
        dataType: "json",
        async: true,
        cache: true,
        success: function (data) {

            if (data !== true) {
                $("#btnAddRow").prop("disabled", true);
                $("#msgloc").fadeIn();
            }
            else {
                $("#btnAddRow").prop("disabled", false);
                $("#msgloc").fadeOut();
                $("#qtynew").focus();

            }
        }
    });
}
function validateLocation(data) {

    var loc = data.value;
    var pn = $("#pnnew").val();
    //si seleccionaste loc
    if (data.value !== '') {
        //si es factura
        if ($("#invoicee").val() !== '0') {
            existeloc(pn, loc);
        }
        //cuando no es factura
        else {
            $.ajax({
                url: "/Client/Corrective/GetPricesByPN/?pn=" + pn + "&loc=" + loc,
                type: "GET",
                dataType: "json",
                async: true,
                cache: true,
                success: function (data) {

                    if (data[0] !== undefined) {


                        //ExisteLocation
                        $.ajax({
                            url: "/Client/Corrective/ExisteLocation/?itemno=" + pn + "&loc=" + loc,
                            type: "GET",
                            dataType: "json",
                            async: true,
                            cache: true,
                            success: function (data) {

                                if (data !== true) {
                                    $("#btnAddRow").prop("disabled", true);
                                    $("#msgloc").fadeIn();
                                }
                                else {
                                    $("#btnAddRow").prop("disabled", false);
                                    $("#msgloc").fadeOut();
                                    $("#qtynew").focus();

                                }
                            }
                        });

                        $("#txtprice").val(parseFloat(data[0].price).toFixed(2));
                        $("#txtcost").val(parseFloat(data[0].std));

                    }
                    else {
                        $.ajax({
                            url: "/Client/Corrective/ExisteLocation/?itemno=" + pn + "&loc=" + loc,
                            type: "GET",
                            dataType: "json",
                            async: true,
                            cache: true,
                            success: function (data) {

                                if (data !== true) {
                                    $("#btnAddRow").prop("disabled", true);
                                    $("#msgloc").fadeIn();
                                }
                                else {
                                    $("#btnAddRow").prop("disabled", false);
                                    $("#msgloc").fadeOut();
                                    $("#qtynew").focus();

                                }
                            }
                        });
                        $("#txtprice").val(parseFloat(0));
                        $("#txtcost").val(parseFloat(0));
                    }


                }
            });

        }





    }
    else {

        if ($("#invoicee").val() !== '0') {

            $("#msgloc").fadeIn();
            $("#btnAddRow").prop("disabled", true);

        } else {
            $("#msgloc").fadeIn();
            $("#btnAddRow").prop("disabled", true);
            $("#txtprice").val(parseFloat(0));
            $("#txtcost").val(parseFloat(0));
        }

    }



}
function Deletedocument(url) {
    swal({
        title: "Are you sure to delete?",
        text: "This content cannot be recovered!",
        icon: "warning",
        buttons: true,
        dangerMode: true,

    }).then(function (willDelete) {


        if (willDelete) {
            $.ajax({
                type: 'DELETE',
                url: url,
                success: function success(data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    } else {
                        toastr.error(data.message);
                    }
                }
            });

        } else {
            console.log("ABORT")
            toastr.success("ABORT");
        }

    });

}
function Deletedocumentregargar(url) {
    swal({
        title: "Are you sure to delete?",
        text: "This content cannot be recovered!",
        icon: "warning",
        buttons: true,
        dangerMode: true,

    }).then(function (willDelete) {


        if (willDelete) {
            $.ajax({
                type: 'DELETE',
                url: url,
                success: function success(data) {
                    if (data.success) {
                        toastr.success(data.message);
                        setTimeout(function () {
                            recargar();
                        }, 2000);
                    } else {
                        toastr.error(data.message);
                    }
                }
            });

        } else {
            console.log("ABORT")
            toastr.success("ABORT");
        }


    });




}
function recargar() {
    location.reload();
}
$(document).on('focusin', function (e) {
    if ($(e.target).closest(".tox-tinymce, .tox-tinymce-aux, .moxman-window, .tam-assetmanager-root").length) {
        e.stopImmediatePropagation();
    }
});

//up rma
$("#btnupdaterma").click(function () {
    let rmaid = $("#rmaid").val();
    let where = $("#whereb option:selected").val();
    let desc = $("#desc").val();
    let phone = $("#phone").val();
    let ext = $("#ext").val();
    let fax = $("#fax").val();
    let totalrma = $("#totalrma").val();
    let po = $("#po").val();
    let contact = $("#contactrma").val();
    let rmatype = $("#selectrmatype option:selected").text();
    let email = $("#email").val();
    let comments = $("#comments").val();
    let reason = $("#sreason").children("option:selected").text();

    UpdateRMA(rmaid, where, desc, phone, ext, fax, totalrma, po, contact, rmatype, email, comments, reason);

});

function UpdateRMA(id, where, desc, phone, ext, fax, totalrma, po, contact, type, email, comments, reason) {

    console.log("type rma : " + type);
    $.ajax({
        url: "/Client/rma/UpdateRMA?id=" + id + "&where=" + where + "&desc=" + desc + "&phone=" + phone + "&ext=" + ext + "&fax=" + fax + "&totalrma=" + totalrma + "&po=" + po + "&contact=" + contact + "&type=" + type + "&email=" + email + "&comments=" + comments + "&reason=" + reason,
        type: "GET",
        dataType: "json",
        async: true,
        cache: true,
        success: function (updated) {
            if (updated == true) {
                window.location.reload();
            }
        }

    });
}




function Disapprove(data) {


    $("#ids").val(data);

}
function Pass(data) {


    $("#idsa").val(data);

}