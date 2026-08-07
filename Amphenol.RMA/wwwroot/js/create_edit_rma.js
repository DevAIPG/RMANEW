var dataTable;
var contadorcode = 1;
var clase = "code1";
var clase2 = "inv1";
var clase3 = "seq1";
var clase4 = "cos1";
var clase5 = "acttion1";
var clase6 = "price1";
var clase7 = "loc1";
var arrfactura = [];

var marcador = $("#validar1").val();
var release = "";
var enviando = false;
var candado = true;
$(".spam4").hide();
$(".spam5").hide();
$(".spam1").hide();
$(".spam2").hide();
$(".spam3").hide();
$("#creer").hide();

$(".gif").hide();

$(".panel").hide();
//cargarDatatableDefectCode();
function FormDataenvio(invoice, seq, code, checkcar, acttion, coustomer, unit, qty, loc, inicio, finalizado, actions) {
    var id = document.getElementById("id").value;
    var Rmarequest = document.getElementById("foliorma").value;
    var rmastatus = document.getElementById("rmastatus").value;
    var rmaapprover = document.getElementById("rmaapprover").value;
    var rmasumbit = document.getElementById("rmasumbit").value;
    var rmadata = document.getElementById("rmadata").value;
    var rmawherebuilt = document.getElementById("validar1").value;
    var total = document.getElementById("total").value;
    var client = document.getElementById("client").value;
    var rmatypeofrequest = document.getElementById("select").value;
    var description = document.getElementById("description").value;
    var customercomplait = document.getElementById("customercomplait").value;
    var rma500 = document.getElementById("rma500").value;
    var customerpo = document.getElementById("customerpo").value;
    var shipto = document.getElementById("ship").value;
    var contact = document.getElementById("contact").value;
    var phone = document.getElementById("phone").value;
    var ext = document.getElementById("ext").value;
    var fax = document.getElementById("fax").value;
    var contactemail = document.getElementById("contactemail").value;
    var companyemail = document.getElementById("companyemail").value;
    var comment = document.getElementById("comment").value;
    var reason = $("#sreason option:selected").text();
    //var actionselected = $("#sactions").val();
    //tinymce.get("comment").getContent();
    var infoProducto = new FormData();

    var totalFicheros = $("#subidaArchivo").get(0).files.length;
    if (totalFicheros != 0) {
        for (var i = 0; i < totalFicheros; i++) {
            infoProducto.append("archivos[" + i + "]", $("#subidaArchivo")[0].files[i]);
        }
    }

    infoProducto.append("id", id);
    infoProducto.append("Rmarequest", Rmarequest);
    infoProducto.append("rmastatus", rmastatus);
    infoProducto.append("rmaapprover", rmaapprover);
    infoProducto.append("rmasumbit", rmasumbit);
    infoProducto.append("rmadata", rmadata);
    infoProducto.append("rmawherebuilt", rmawherebuilt);
    infoProducto.append("total", total);
    infoProducto.append("client", client);
    infoProducto.append("rmatypeofrequest", rmatypeofrequest);
    infoProducto.append("description", description);
    infoProducto.append("description", description);
    infoProducto.append("customercomplait", customercomplait);
    infoProducto.append("rma500", rma500);
    infoProducto.append("customerpo", customerpo);
    infoProducto.append("shipto", shipto);
    infoProducto.append("contact", contact);
    infoProducto.append("phone", phone);
    infoProducto.append("ext", ext);
    infoProducto.append("fax", fax);
    infoProducto.append("contactemail", contactemail);
    infoProducto.append("companyemail", companyemail);
    infoProducto.append("comment", comment);
    infoProducto.append("rmareason", reason);

    var count = actions.length;
    if (count > 0) {
        for (var i = 0; i < count; i++) {
            infoProducto.append('actions[' + i + ']', actions[i]);

        }
    }
    //infoProducto.append("actions", actionselected);


    coustomer.forEach(function (coustomer, index) {
        infoProducto.append('coustumer[' + index + ']', coustomer);
    });

    acttion.forEach(function (acttion, index) {
        infoProducto.append('acttion[' + index + ']', acttion);
    });
    seq.forEach(function (seq, index) {
        infoProducto.append('seq[' + index + ']', seq);
    });

    code.forEach(function (code, index) {
        infoProducto.append('code[' + index + ']', code);
    });
    qty.forEach(function (qty, index) {
        infoProducto.append('qty[' + index + ']', qty);
    });
    loc.forEach(function (loc, index) {
        infoProducto.append('loc[' + index + ']', loc);
    });
    invoice.forEach(function (invoice, index) {
        infoProducto.append('invoice[' + index + ']', invoice);
    });
    unit.forEach(function (unit, index) {
        infoProducto.append('unit[' + index + ']', unit);
    });
    checkcar.forEach(function (checkcar, index) {
        infoProducto.append('checkcar[' + index + ']', checkcar);
    })
    //actions.forEach(function (actions, index) {
    //    infoProducto.append('actions[' + index + ']', actions);
    //})
    infoProducto.append("inicio", inicio);
    infoProducto.append("finalizado", finalizado);


    InsertBD(infoProducto);

}

function test2() {



    var invoice = new Array();
    var action = new Array();
    var seq = new Array();
    var code = new Array();
    var qty = new Array();
    var unit = new Array();
    var acttion = new Array();
    var coustomer = new Array();
    var loc = new Array();
    var checkcar = new Array();
    var finalizado = false;
    var inicio = true;

    var x = 0;
    $("#tblRoles tbody >  tr").each(function (index) {
        if (x < 100) {
            seq[x] = $(this).find(".seq").val();

            invoice[x] = $(this).find(".invoice").val();
            code[x] = $(this).find(".code").val();
            checkcar[x] = $(this).find(".checkcar").val();
            acttion[x] = $(this).find(".acttion").val();
            unit[x] = $(this).find(".unit").val();
            loc[x] = $(this).find(".loc").val();
            coustomer[x] = $(this).find(".coustumer").val();
            unit[x] = $(this).find(".unit").val();
            qty[x] = $(this).find(".qty").val();
            action[x] = $(this).find(".saction").val();

            x++;
        } else {
            seq[x] = $(this).find(".seq").val();

            invoice[x] = $(this).find(".invoice").val();
            code[x] = $(this).find(".code").val();
            checkcar[x] = $(this).find(".checkcar").val();
            acttion[x] = $(this).find(".acttion").val();
            unit[x] = $(this).find(".unit").val();
            loc[x] = $(this).find(".loc").val();
            coustomer[x] = $(this).find(".coustumer").val();
            unit[x] = $(this).find(".unit").val();
            qty[x] = $(this).find(".qty").val();
            action[x] = $(this).find(".saction").val();



            FormDataenvio(invoice, seq, code, checkcar, acttion, coustomer, unit, qty, loc, inicio, finalizado, action);
            inicio = false;
            invoice = [];

            seq = [];

            code = [];

            checkcar = [];

            acttion = [];

            unit = [];

            loc = [];

            coustomer = [];

            unit = [];

            qty = [];

            action = [];
            x = 0;

        }
    });

    finalizado = false;
    FormDataenvio(invoice, seq, code, checkcar, acttion, coustomer, unit, qty, loc, inicio, finalizado, action);

    inicio = false;
    finalizado = true;
    FormDataenvio(invoice, seq, code, checkcar, acttion, coustomer, unit, qty, loc, inicio, finalizado, action);






}
function redireccionar() {
    location.href = "/Client/Rma/index";
}

function InsertBD(datos) {
    var xhr = new XMLHttpRequest();
    var url = document.querySelector('#crearma').getAttribute('action');
    xhr.open('POST', url, true);

    xhr.onload = function () {
        if (this.status === 200) {

            var respuesta = JSON.parse(xhr.responseText);

            if (respuesta.data == "Terminado") {
                setTimeout(function () {
                    redireccionar();
                }, 8000);


            }
        }
    };

    xhr.upload.onprogress = function (e) {
        if (e.lengthComputable) {
            var progreso = e.loaded / e.total * 100;
        }
    };

    xhr.send(datos);
}
function updatePaginationPN(totalPages, currentPage) {
    var paginationElement = document.getElementById('paginationPN');
    paginationElement.innerHTML = '';
    for (let i = 1; i <= totalPages; i++) {
        const button = document.createElement('button');
        button.innerText = i;
        button.addEventListener('click', () => {
            loadPartNumbers(i);
            currentPage = i;
        });
        if (i === currentPage) {
            button.disabled = true;
        }
        paginationElement.appendChild(button);
    }

}
function abrirrc(btn) {
    var linea = $(btn).data("linea");

    var dataTable = $("#tblS").DataTable({
        destroy: true,
        deferRender: true,
        "autoWidth": false,
        "ajax": {
            "url": "/client/corrective/GetAllS",
            "type": "GET",
            "dataType": "json"
        },
        "columns": [{
            "data": "sy_code",
            "render": function render(data, type, row) {
                return "<a class='btn-link' id='btnreject" + linea + "' data-id=" + data.trim() + " data-desc=" + row.code_desc + ">" + data + "</a>";
            }
        }, {
            "data": "code_desc"
        }],
        "language": {
            "emptyTable": "No records found"
        }
    });
    $("#tblSm").modal("show");


    $(document).on("click", "#btnreject" + linea, function () {
        let id = $(this).data("id");
        let desc = $(this).data("desc");
        //$("#defectinput").val(id);
        $(".code" + linea).val("");
        $(".code" + linea).val(id);
        $("#tblSm").modal("hide");
    });

    //$("#mtblSt").modal("show");
}
function LoadSecuencias(fact, linea) {

    $.ajax({
        url: "/client/corrective/getSecuencias/",
        type: "GET",
        data: { factura: fact },
        dataType: "json",
        cache: true,
        async: true,
        success: function success(data) {
            var table = $("#tblQ").DataTable({
                destroy: true,
                data: data,
                columns: [
                    {
                        "data": "line_seq_no",
                        render: function (data, type, row) {
                            return '<a class="text-primary"  data-item=' + row.item_no + ' id="linkseqq' + linea + '" data-seq=' + row.line_seq_no + '>' + row.line_seq_no + '</a>';
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
        },
    });


    $(document).on("click", "#linkseqq" + linea, function () {

        $(".seq" + linea).val($(this).text());
        let item = $(this).data("item");
        $("#cos" + linea).val(item);
        $("#cos" + linea).prop("readonly", true);
        $("#cos" + linea).prop("disabled", true);
        arrfactura = [];
        $.ajax({
            url: "/Client/Corrective/GetPricesByInvoice/?invoicenumber=" + $(".inv" + linea).val() + "&loc=D",
            type: "GET",
            dataType: "json",
            async: true,
            cache: true,
            success: function (data) {

                if (data[0] !== undefined) {
                    $("#msgloc" + linea).fadeOut();
                    $("#creer").prop("disabled", false);
                    $(".price" + linea).val(parseFloat(data[0].price).toFixed(2));
                    $("#cost" + linea).val(parseFloat(data[0].std));
                    arrfactura.push(data[0].loc);

                    if (parseFloat($(".price" + linea).val()) > parseFloat($("#cost" + linea).val())) {
                        //no
                        $(this).css("border", "1px solid green");
                    }
                    else {
                        //si
                        $(this).css("border", "1px solid red");

                        Swal.fire({
                            title: 'Price must be higher than unit cost. Do you want to continue?',
                            icon: 'warning',
                            showCancelButton: true,
                            confirmButtonColor: '#d33',
                            cancelButtonColor: '#95a5a6',
                            confirmButtonText: 'Yes',
                            cancelButtonText: 'No'
                        }).then((result) => {
                            if (result.isConfirmed) {

                            }
                        });

                    }


                }


            }
        });
        $("#tblQm").modal("hide");

    });
}


function LoadPN(pn, linea) {

    $.ajax({
        url: "/client/corrective/GetPartNumbers/",
        type: "GET",
        dataType: "json",
        cache: true,
        async: true,
        success: function success(data) {
            var table = $("#tblPN").DataTable({
                destroy: true,
                deferRender: true,
                data: data.data,
                columns: [
                    {
                        "data": "id",
                        render: function (data, type, row) {
                            return '<a class="text-primary"  id="linkpn' + linea + '" data-pn=' + row.item_no + '>' + row.id + '</a>';
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
            // $('body').loadingModal('hide');
            // $('body').loadingModal('destroy');
            //$("#loading").fadeOut();


        },
        error: function error(xhr, status, _error17) {
        },
    });
    $(document).on("click", "#linkpn" + linea, function () {
        $("#" + pn).val($(this).data("pn"));

        var bandera = true;
        $.ajax({
            url: "/client/corrective/GetPartes/?term=" + $("#" + pn).val().replace(/#/g, "%23"),
            type: "GET",
            dataType: "json",
            cache: true,
            async: true,
            success: function success(data) {
                var availabconstags = data.result;
                $('input[name=coustumer]').autocomplete({
                    source: availabconstags,
                    minlength: 1,
                    select: function select(event, ui) {
                        $("#" + pn).css("color", "green");
                        var color = "";
                        $.ajax({
                            "url": "/client/corrective/GetAllM3/?filtro=" + ui.item.value.replace(/#/g, "%23") + "&filtro2=" + loc,
                            type: "GET",
                            dataType: "json",
                            cache: true,
                            success: function success(data) {
                                if (loc == "") {
                                    $("." + clase6 + "").val(0);
                                    $("." + clase5 + "").val(0);
                                }

                                $("." + clase6 + "").val(data.data.price.toFixed(6));
                                $("." + clase5 + "").val(data.data.std_cost.toFixed(6));
                                enviaDatos();

                            },
                            error: function error(xhr, status, _error8) {
                                $().toastmessage('showToast', {
                                    text: 'Error con ean ' + xhr.responseText,
                                    sticky: true,
                                    type: 'error'
                                });
                            }
                        });
                        $.ajax({
                            "url": "/client/corrective/GetAllpass/?filtro=" + ui.item.value.replace(/#/g, "%23"),
                            type: "GET",
                            dataType: "json",
                            cache: true,
                            async: true,
                            success: function success(data) {
                                color = data.data;
                            },
                            error: function error(xhr, status, _error9) {
                                $().toastmessage('showToast', {
                                    text: 'Error con ean ' + xhr.responseText,
                                    sticky: true,
                                    type: 'error'
                                });
                            }
                        });

                        if (color == "pass") {
                            $(this).css("color", "green");
                        } else {
                            $(this).css("color", "red");
                        }

                        setTimeout(espacios, 1000);
                        setTimeout(mensaje, 500);
                    },
                    position: {
                        my: "left top",
                        at: "top"
                    },
                    delay: 500,
                    open: function open() {
                        $("ul.ui.menu").width($("#" + pn).innerWidth());
                    },
                    classes: {
                        "ui.autocomplete": "highlight"
                    }
                });

                if (data.existe != "existe") {
                    bandera = false;
                } else {
                    bandera = true;
                }

                if (bandera == false) {
                    $("#" + pn).css("color", "red");
                } else {
                    $("#" + pn).css("color", "green");
                }
            },
            error: function error(xhr, status, _error10) {
                $().toastmessage('showToast', {
                    text: 'Error con ean ' + xhr.responseText,
                    sticky: true,
                    type: 'error'
                });
            }
        });
        $("#mPN").modal("hide");
    });
}
function loadpn(btn) {
    var linea = $(btn).data("linea");
    var idpn = "cos" + linea;
    $("#mPN").modal("show");
    LoadPN(idpn, linea);

}
function loadseq(btn) {
    var linea = $(btn).data("linea");
    var idseq = "imgBuscarSeq" + linea;
    $("#tblQm").modal("show");

}


function abrirfacturas(btn) {
    var linea = $(btn).data("linea");
    const client = document.getElementById("client").value;
    $.ajax({
        url: "/client/corrective/GetAllI2/?filtro=" + client.trim(),
        type: "GET",
        datatype: "json",
        async: true,
        cache: true,

        success: function (data) {
            $("#tblI").DataTable({
                destroy: true,
                data: data,
                columns: [
                    {
                        data: 'invNo',
                        render: function (data, type, row) {
                            return '<a class="text-primary"  id="linkinvno' + linea + '" data-no=' + row.invNo + '>' + row.invNo + '</a>';
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
    $("#tblIm").modal("show");
    //linkinvno
    //$(document).on("click", "#tblIm", function () {
    //    $("#abrir1").val($(this).data("no"));
    //    $("#tblIm").modal("hide");
    //});
    $(document).on("click", "#linkinvno" + linea, function () {
        //alert($(this).text());
        $(".inv" + linea).val($(this).text());
        $("#tblIm").modal("hide");
        LoadSecuencias($(this).text(), linea);

    });

};

function loadPartNumbers(currentPage) {


    //var resultsTable = document.getElementById("tblPN").getElementsByTagName('tbody')[0];
    //fetch("/client/corrective/GetPartNumbers/")
    //    .then(response => response.json())
    //    .then(data => {
    //        resultsTable.innerHTML = '';
    //        console.log(data);
    //        data.items.forEach(item => {
    //            resultsTable.innerHTML += `<tr>
    //                <td>${item.id}</td>
    //                <td>${item.item_no}</td>
    //                <td>${item.item_desc_1}</td>
    //            </tr>`; 
    //        });
    //        updatePaginationPN(data.totalPages, currentPage);
    //    })
    //    .catch(error => console.log(error));

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
            alert("complete");
            //$('body').loadingModal('hide');
            //$('body').loadingModal('destroy');
        },
        error: function error(xhr, status, _error17) {
        },
    });

    $(document).on("click", "#linkpn", function () {
        $("#csexsw_costumber_Coustumer").val($(this).data("pn"));
        $("#mPN").modal("hide");
    });

}
$(document).ready(function () {


    $("#crearma").keypress(function (e) {
        if (e.which == 13) {

            return false;
        }
    });



});

$("#crearma").validate({
    rules: {
        coustumer: {
            required: true,
        },
        qty: {
            required: true,
        },
        acttion: {
            required: true,
        },
        unit: {
            required: true,
        },
    },
    messages: {},
    tooltip_options: {
        qty: {
            trigger: "focus",
        },
        acttion: {
            trigger: "focus",
        },
        unit: {
            trigger: "focus",
        },
        coustumer: {
            trigger: "focus",
        },
    },
    submitHandler: function submitHandler(formulario, e) {
        e.preventDefault();
        var pass = true;
        var validar1 = document.getElementById("validar1").value;
        var validar2 = document.getElementById("select").value;
        var validar3 = document.getElementById("client").value;
        var validar4 = document.getElementById("ship").value;

        if (
            validar1 == "0" ||
            validar2 == "0" ||
            validar3 == "" ||
            validar4 == ""
        ) {
            if (validar1 == "0") {
                $(".spam1").show();
            } else {
                $(".spam1").hide();
            }

            if (validar2 == "0") {
                $(".spam2").show();
            } else {
                $(".spam2").hide();
            }

            if (validar3 == "") {
                $(".spam3").show();
            } else {
                $(".spam3").hide();
            }

            if (validar4 == "") {
                $(".spam5").show();
            } else {
                $(".spam5").hide();
            }
        } else {
            if (pass == true) {
                var pass2 = true;
                var mensaje = "";
                $("#tblRoles > tbody input[name='invoice']").each(function () {
                    var color = $(this).css("background-color");

                    if (color == "rgb(255, 0, 0)") {
                        mensaje =
                            "The Invoice ship-to address no. must match the RMA ship-to address no.";
                        pass2 = false;
                    }
                });

                $("#tblRoles > tbody input[name='coustumer']").each(function () {
                    var color = $(this).css("color");

                    if (color == "rgb(255, 0, 0)" || color == "rgb(33, 33, 33)") {
                        pass2 = false;
                        mensaje = "Invalid Part Number or not validated.";
                    }
                });




                let ArrayUnit = []
                $("#tblRoles > tbody input[name='acttion']").each(function () {
                    let valor = parseFloat($(this).val())
                    ArrayUnit.push(valor)

                });


                let ArrayPrice = []
                $("#tblRoles > tbody input[name='unit']").each(function () {
                    let valor = parseFloat($(this).val())
                    ArrayPrice.push(valor)

                });


                //ArrayPrice.forEach(function (elemento, indice, array) {



                //    if (elemento > ArrayUnit[indice]) {
                //        console.log("Es correcto")
                //    } else {
                //        pass2 = false;
                //        mensaje = "Price must be higher than unit cost";
                //        console.log(mensaje)
                //    }
                //});
                $("#tblRoles > tbody input[name='unit']").each(function () {
                    if (parseFloat($(this).val()) > 0) {
                    } else {
                        pass2 = false;
                        mensaje = "Price cannot be zero";
                    }

                });

                var verificar = true;
                $("#tblRoles > tbody select[name='loc']").each(function () {
                    var valor = $(this).attr("id");

                    let select = document.querySelector("[id='" + valor + "']").options[
                        document.querySelector("[id='" + valor + "']").selectedIndex
                    ].value;

                    if (select != "" && select != null) {
                    } else {
                        $(this).css("background", "red");
                        verificar = false;
                    }
                });

                if (verificar == true) {
                    $(".cargar").hide();
                    $(".gif").show();
                    $(":submit").disabled = "none";
                    pass = false;
                    espacios();
                    if (candado == true) {
                        test2();
                        candado = false;
                    }
                }

            }
        }
    },
});

$("#tblRoles > tbody input[name='coustumer']").each(function () {
    $(this).rules("add", {
        required: true,
    });
});
$("#validar1").on("change", function () {
    $(".spam1").hide();
});
$(".abrir3").live("dblclick", function () {
    $("#rma").modal("show");
});
$(".abrir1").live("dblclick", function () {
    $("#tblIm").modal("show");
});
$(".abrir2").live("dblclick", function () {
    $("#tblQm").modal("show");
});
$(".abrir4").live("dblclick", function () {
    $("#tblSm").modal("show");
});
$("input[type=checkbox]").live("click", function (e) {
    if ($(this).is(":checked")) {
        var atributo = $(this).attr("class");
        var arr = atributo.split(" ");
        $("#" + arr[0] + "").attr("value", "true");
        $(this).attr("value", "true");
        $("#" + arr[0] + "").val("true");
    } else {
        var _atributo = $(this).attr("class");

        var _arr = _atributo.split(" ");

        $("#" + _arr[0] + "").attr("value", "false");
        $("#" + _arr[0] + "").val("false");
        $(this).attr("value", "false");
    }
});

$("input[name=coustumer]").live("keydown", function (e) {
    var color = $(this).css("color");
    $("." + clase2 + "").css("background", "");
    $("." + clase2 + "").css("color", "black");
    $("." + clase2 + "").val("");
    $("." + clase3 + "").val(0);
    if (color != "rgb(255, 0, 0)") {
        var keyCode = e.keyCode || e.which;
        if ((keyCode == 9 || keyCode == 13) && $("." + clase2 + "").val() == "") {
            var valor = $(this).attr("value");
            var _color = "";
            let id = document.getElementById("client").value;
            if (
                valor.trim() != null &&
                valor.trim() != "" &&
                id != null &&
                id != ""
            ) {
                let select = document.querySelector("[id='" + clase7 + "']").options[
                    document.querySelector("[id='" + clase7 + "']").selectedIndex
                ].value;
                $("." + clase6 + "").val(0);
                $("." + clase5 + "").val(0);

                if (select != "" && select != null) {
                    $.ajax({
                        url:
                            "/client/corrective/GetAllM3/?filtro=" +
                            valor.replace(/#/g, "%23") +
                            "&filtro2=" +
                            select +
                            "&cliente=" +
                            id,
                        type: "GET",
                        dataType: "json",
                        cache: true,
                        success: function success(data) {
                            $("." + clase6 + "").val(data.data.price.toFixed(6));
                            $("." + clase5 + "").val(data.data.std_cost.toFixed(6));
                            enviaDatos();
                        },
                        error: function error(xhr, status, _error4) {
                            $().toastmessage("showToast", {
                                text: "Error con ean " + xhr.responseText,
                                sticky: true,
                                type: "error",
                            });
                        },
                    });
                    $.ajax({
                        url:
                            "/client/corrective/GetAllpass/?filtro=" +
                            valor.replace(/#/g, "%23"),
                        type: "GET",
                        dataType: "json",
                        cache: true,
                        async: true,
                        success: function success(data) {
                            _color = data.data;
                        },
                        error: function error(xhr, status, _error5) {
                            $().toastmessage("showToast", {
                                text: "Error con ean " + xhr.responseText,
                                sticky: true,
                                type: "error",
                            });
                        },
                    });

                    if (_color == "pass") {
                        $(this).css("color", "green");
                    } else {
                        $(this).css("color", "red");
                    }
                }
            }
            var aux = $(this).val().trim();
            $(this).val(aux);
        }
    }
});
$("input[name=coustumer]").live("paste", function (e) {
    var color = $(this).css("color");
    $("." + clase2 + "").css("background", "");
    $("." + clase2 + "").css("color", "black");

    $("." + clase2 + "").val("");
    $("." + clase3 + "").val(0);
    let id = document.getElementById("client").value;

    if (color != "rgb(255, 0, 0)") {
        var _color2 = "";
        setTimeout(function () {
            //currentTarget added in jQuery 1.3
            let valor = $(e.currentTarget).val();
            if ($("." + clase2 + "").val() == "" && id != null && id != "") {
                let select = document.querySelector("[id='" + clase7 + "']").options[
                    document.querySelector("[id='" + clase7 + "']").selectedIndex
                ].value;
                $("." + clase6 + "").val(0);
                $("." + clase5 + "").val(0);

                if (select != "" && select != null) {
                    $.ajax({
                        url:
                            "/client/corrective/GetAllM3/?filtro=" +
                            valor.replace(/#/g, "%23") +
                            "&filtro2=" +
                            select +
                            "&cliente=" +
                            id,
                        type: "GET",
                        dataType: "json",
                        cache: true,
                        success: function success(data) {
                            $("." + clase6 + "").val(data.data.price.toFixed(6));
                            $("." + clase5 + "").val(data.data.std_cost.toFixed(6));
                            enviaDatos();
                        },
                        error: function error(xhr, status, _error6) {
                            $().toastmessage("showToast", {
                                text: "Error con ean " + xhr.responseText,
                                sticky: true,
                                type: "error",
                            });
                        },
                    });
                    $.ajax({
                        url:
                            "/client/corrective/GetAllpass/?filtro=" +
                            valor.replace(/#/g, "%23"),
                        type: "GET",
                        dataType: "json",
                        cache: true,
                        async: true,
                        success: function success(data) {
                            _color2 = data.data;
                        },
                        error: function error(xhr, status, _error7) {
                            $().toastmessage("showToast", {
                                text: "Error con ean " + xhr.responseText,
                                sticky: true,
                                type: "error",
                            });
                        },
                    });
                }
            }

            //do stuff
        }, 100);

        if (_color2 == "pass") {
            $(this).css("color", "green");
        } else {
            $(this).css("color", "red");
        }

        var aux = $(this).val().trim();
        $(this).val(aux);
    }
});
$("input[name=coustumer]").live("keyup", function (e) {
    var bandera = true;
    $.ajax({
        url:
            "/client/corrective/GetPartes/?term=" +
            $(this).val().replace(/#/g, "%23"),
        type: "GET",
        dataType: "json",
        cache: true,
        async: true,
        success: function success(data) {
            var availabconstags = data.result;
            $("input[name=coustumer]").autocomplete({
                source: availabconstags,
                minlength: 1,
                select: function select(event, ui) {
                    let select = document.querySelector("[id='" + clase7 + "']").options[
                        document.querySelector("[id='" + clase7 + "']").selectedIndex
                    ].value;
                    $(this).css("color", "green");
                    $("." + clase3 + "").val(0);
                    $("." + clase2 + "").val("");
                    $("." + clase2 + "").css("color", "black");
                    $("." + clase2 + "").css("background", "");

                    var color = "";
                    let id = document.getElementById("client").value;
                    if (id != null && id != "") {
                        $("." + clase6 + "").val(0);
                        $("." + clase5 + "").val(0);

                        if (select != "" && select != null) {
                            $.ajax({
                                url:
                                    "/client/corrective/GetAllM3/?filtro=" +
                                    ui.item.value.replace(/#/g, "%23") +
                                    "&filtro2=" +
                                    select +
                                    "&cliente=" +
                                    id,
                                type: "GET",
                                dataType: "json",
                                cache: true,
                                success: function success(data) {
                                    $("." + clase6 + "").val(data.data.price.toFixed(6));
                                    $("." + clase5 + "").val(data.data.std_cost.toFixed(6));
                                    enviaDatos();
                                },
                                error: function error(xhr, status, _error8) {
                                    $().toastmessage("showToast", {
                                        text: "Error con ean " + xhr.responseText,
                                        sticky: true,
                                        type: "error",
                                    });
                                },
                            });
                            $.ajax({
                                url:
                                    "/client/corrective/GetAllpass/?filtro=" +
                                    ui.item.value.replace(/#/g, "%23"),
                                type: "GET",
                                dataType: "json",
                                cache: true,
                                async: true,
                                success: function success(data) {
                                    console.log(data);
                                    color = data.data;
                                },
                                error: function error(xhr, status, _error9) {
                                    $().toastmessage("showToast", {
                                        text: "Error con ean " + xhr.responseText,
                                        sticky: true,
                                        type: "error",
                                    });
                                },
                            });

                            if (color == "pass") {
                                $(this).css("color", "green");
                            } else {
                                $(this).css("color", "red");
                            }
                        }
                    }
                    setTimeout(espacios, 1000);
                    setTimeout(mensaje, 500);
                },
                position: {
                    my: "left top",
                    at: "top",
                },
                delay: 500,
                open: function open() {
                    $("ul.ui.menu").width($(this).innerWidth());
                },
                classes: {
                    "ui.autocomplete": "highlight",
                },
            });

            if (data.existe != "existe") {
                bandera = false;
            } else {
                bandera = true;
            }
        },
        error: function error(xhr, status, _error10) {
            $().toastmessage("showToast", {
                text: "Error con ean " + xhr.responseText,
                sticky: true,
                type: "error",
            });
        },
    });
    if (bandera == false) {
        console.log($(this));

        $(this).css("color", "red");
    } else {
        $(this).css("color", "green");
    }
});

function enviaDatos(btn) {

    var linea = $(btn).data("linea");
    var qty = document.getElementsByClassName("qty");
    //var qty = $("#qty" + linea);
    var unit = document.getElementsByClassName("unit");
    //var unit = $(".price" + linea);
    var valor = 0;
    var valor2 = 0;
    var i = 0;
    $("#tblRoles tbody >  tr").each(function (index) {
        valor = parseFloat(qty[i].value) * parseFloat(unit[i].value);
        valor2 = valor2 + valor;

        console.log(valor);
        $("#total").val(valor2);
        i++;
    });
    $("#price" + linea).change(function () {
        if (parseFloat($(this).val()) > parseFloat($("#cost" + linea).val())) {
            //no
            $(this).css("border", "1px solid green");
        }
        else {
            //si
            $(this).css("border", "1px solid red");
        }
    });
    $("#price" + linea).blur(function () {
        if (parseFloat($(this).val()) > parseFloat($("#cost" + linea).val())) {
            //no
            //$(this).focus();
        }
        else {
            //si  


            Swal.fire({
                title: 'Price must be higher than unit cost. Do you want to continue?',
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#d33',
                cancelButtonColor: '#95a5a6',
                confirmButtonText: 'Yes',
                cancelButtonText: 'No'
            }).then((result) => {
                if (result.isConfirmed) {

                }
            });


        }
    });

}

function validador() {
    $("#tblRoles > tbody input[name='coustumer']").each(function () {
        var valor = $(this).attr("value");
        var color = "";

        if (valor.trim() != null && valor.trim() != "") {
            $.ajax({
                url:
                    "/client/corrective/GetAllpass/?filtro=" + valor.replace(/#/g, "%23"),
                type: "GET",
                dataType: "json",
                cache: true,
                async: true,
                success: function success(data) {
                    console.log(data);
                    color = data.data;
                },
                error: function error(xhr, status, _error11) {
                    $().toastmessage("showToast", {
                        text: "Error con ean " + xhr.responseText,
                        sticky: true,
                        type: "error",
                    });
                },
            });

            console.log(color);
            if (color == "pass") {
                $(this).css("color", "green");
            } else {
                $(this).css("color", "red");
            }
        }
    });
}

espacios();
focusin();

function existeloc(itemno, loc, linea) {
    //ExisteLocation
    $.ajax({
        url: "/Client/Corrective/ExisteLocation/?itemno=" + itemno + "&loc=" + loc,
        type: "GET",
        dataType: "json",
        async: true,
        cache: true,
        success: function (data) {

            if (data !== true) {
                $("#creer").prop("disabled", true);
                $("#msgloc" + linea).fadeIn();
            }
            else {
                $("#creer").prop("disabled", false);
                $("#msgloc" + linea).fadeOut();
            }
        }
    });
}

$("label .error").tooltip();
//select location
function Dato(data) {
    console.log("locationnnn");
    var linea = $(data).data("linea");
    var loc = data.value;
    var pn = $("#cos" + linea).val();
    //si seleccionaste loc
    if (data.value !== '') {
        //si es factura
        if ($(".inv" + linea).val() !== '0') {

            existeloc(pn, loc, linea);

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

                        $(".price" + linea).val(parseFloat(data[0].price).toFixed(2));
                        $("#cost" + linea).val(parseFloat(data[0].std));
                        existeloc(pn, loc, linea);
                    }
                    else {
                        existeloc(pn, loc, linea);
                        $(".price" + linea).val(parseFloat(0));
                        $("#cost" + linea).val(parseFloat(0));
                    }


                }
            });

        }





    }
    else {

        if ($(".inv" + linea).val() !== '0') {

            $("#msgloc" + linea).fadeIn();
            $("#creer").prop("disabled", true);

        } else {
            $("#msgloc" + linea).fadeIn();
            $("#creer").prop("disabled", true);
            $(".price" + linea).val(parseFloat(0));
            $("#cost" + linea).val(parseFloat(0));
        }

    }


    console.log(data.getAttribute("data-filterListId"));
    setTimeout(espacios, 1000);
    clase = "code" + data.getAttribute("data-filterListId");
    clase2 = "inv" + data.getAttribute("data-filterListId");
    clase4 = "cos" + data.getAttribute("data-filterListId");
    clase3 = "seq" + data.getAttribute("data-filterListId");
    clase5 = "acttion" + data.getAttribute("data-filterListId");
    clase6 = "price" + data.getAttribute("data-filterListId");
    clase7 = "loc" + data.getAttribute("data-filterListId");
}


function Dato2(data) {
    console.log(data.getAttribute("data-filterListId"));
    setTimeout(espacios, 1000);
    clase = "code" + data.getAttribute("data-filterListId");
    clase2 = "inv" + data.getAttribute("data-filterListId");
    clase4 = "cos" + data.getAttribute("data-filterListId");
    clase3 = "seq" + data.getAttribute("data-filterListId");
    clase5 = "acttion" + data.getAttribute("data-filterListId");
    clase6 = "price" + data.getAttribute("data-filterListId");
    clase7 = "loc" + data.getAttribute("data-filterListId");
}

function Mover1(data) {
    $("." + clase + "").val(data);
    espacios();
}

function Mover4(data, data2, data3, data4) {
    $("." + clase4 + "").val(data2);

    $("." + clase3 + "").val(parseInt(data));
    $("." + clase4 + "").val(data2.replace(/¶/g, " ").trim());

    $("." + clase6 + "").val(data3.toFixed(6));
    $("." + clase5 + "").val(data4.toFixed(6));
    enviaDatos();
    espacios();
    $("." + clase4 + "").css("color", "green");


}

$("#mtblSt").on("hidden.bs.modal", function (e) {
    document.getElementById("ship").focus();
    document.getElementById("total").focus();
});

function Comments(data) {
    console.log(data);

    $.ajax({
        url: "/Client/rma/comments/?id=" + data,
        type: "GET",
        dataType: "json",
        cache: true,
        success: function success(data_) {
            console.log(data_);
            $("#commentcontrol").text( data_.data[0]);
        },
        error: function error(xhr, status, _error19) {
            $().toastmessage("showToast", {
                text: "Error con ean " + xhr.responseText,
                sticky: true,
                type: "error",
            });
        },
    });
}

function Remark(data) {
    $("#idrema").val(data);
}

function lineas(data) {

    $(document).ready(function () {
        var filtroValue = data; 

        $.ajax({
            url: "/Client/corrective/GetAllCos/?filtro=" + filtroValue,
            type: "GET",
            dataType: "json",
            success: function (response) {
                var tbody = $("#tbllineas tbody");
                tbody.empty(); 

              
                response.data.forEach(function (item) {
                    var row = "<tr>" +
                        "<td align='center'>" + item.invoice + "</td>" +
                        "<td align='center'>" + item.seq + "</td>" +
                        "<td align='center'>" + item.coustumer + "</td>" +
                        "<td align='center'>" + item.qty + "</td>" +
                        "<td align='center'>" + item.unit + "</td>" +
                        "<td align='center'>" + item.cost + "</td>" +
                        "<td align='center'>" + item.action + "</td>" +
                        "<td align='center'>" + item.retur + "</td>" +
                        "</tr>";
                    tbody.append(row);
                });
            },
            error: function (err) {
                console.error("AJAX error:", err);
            }
        });
    });

}

function Abrir(data) {
    var now = new Date();
    var datetime =
        now.getFullYear() + "/" + (now.getMonth() + 1) + "/" + now.getDate();
    datetime +=
        " " + now.getHours() + ":" + now.getMinutes() + ":" + now.getSeconds();
    $.ajax({
        url: "/Client/rma/GetDocument/?id=" + data + "&fecha=" + datetime,
        type: "GET",
        dataType: "json",
        cache: true,
        success: function success(data) {
            console.log(data);
            $("#tblStandar").children("tbody").empty();

            for (var i = 0; i < data.data.length; i++) {
                var pdf = data.data[i].documento;
                var id = data.data[i].id;
                $("#tblStandar")
                    .children("tbody")
                    .append(
                        "<tr>" +
                        '<td align="center" style="dislay: none;">' +
                        data.data[i].documento +
                        "</td>" +
                        '<td align="center" style="dislay: none;"> <a id="btnrma_file-' + id + '" data-rma=' + data.rmano + ' data-fname=' + data.data[i].documento + ' ><img src="../../../documento.png" ></a></td></tr> '
                    );
                $(document).on("click", "#btnrma_file-" + id, function () {


                    let rma = $(this).data("rma");
                    let fname = $(this).data("fname");

                    $(this).prop("href", "/Client/rma/OpenFileRMA?rmano=" + rma + "&filename=" + fname);

                });


            }
        },
        error: function error(xhr, status, _error20) {
            $().toastmessage("showToast", {
                text: "Error con ean " + xhr.responseText,
                sticky: true,
                type: "error",
            });
        },
    });
}

function Document(data) {
    var now = new Date();
    var datetime =
        now.getFullYear() + "/" + (now.getMonth() + 1) + "/" + now.getDate();
    datetime +=
        " " + now.getHours() + ":" + now.getMinutes() + ":" + now.getSeconds();
    $.ajax({
        url: "/Client/rma/GetDocument/?id=" + data + "&fecha=" + datetime,
        type: "GET",
        dataType: "json",
        cache: true,
        success: function success(data) {
            console.log(data);
            $("#tblStandar").children("tbody").empty();

            for (var i = 0; i < data.data.length; i++) {
                var pdf = data.data[i].documento;
                var id = data.data[i].id;
                $("#tblStandar")
                    .children("tbody")
                    .append(
                        "<tr>" +
                        '<td align="center" style="dislay: none;">' +
                        data.data[i].documento +
                        "</td>" +
                        '<td align="center" style="dislay: none;"> <a href=javascript:finestraSecundaria("../../../documents/documents/rma/' +
                        pdf.replace(/\s/g, "¶") +
                        '")><img src="../../../documento.png" ></a></td>' +
                        '<td align="center" style="dislay: none;"><a onclick=Deconstedocument("/Client/rma/Deconstedocument/' +
                        id +
                        '") class="btn btn-danger text-white" style="cursor: pointer; width: 100px;"> <i class="fas fa-trash-alt"></i> Deconste </a></td> </tr> '
                    );
            }
        },
        error: function error(xhr, status, _error21) {
            $().toastmessage("showToast", {
                text: "Error con ean " + xhr.responseText,
                sticky: true,
                type: "error",
            });
        },
    });
}

var proceso = document.getElementById("proceso").value;

if (proceso == "edit") {
    //enviaDatos();
    $(".ship").show();
    var commentid = document.getElementById("client").value;

    function Cargar3() {
        $("#crearma").attr("action", "/client/rma/done/ ");
        setTimeout(mensaje, 5000);
    }

    function Cargar2() {
        $("#crearma").attr("action", "/client/rma/edit/ ");
        setTimeout(mensaje, 5000);
    }
    function Cargar() {
        $("#crearma").attr("action", "/client/rma/saveedit/ ");
        setTimeout(mensaje, 5000);
    }
}

if (proceso == "create") {
    $(".ship").hide();

    var zeroFill = function zeroFill(number, width) {
        width -= number.toString().length;

        if (width > 0) {
            return new Array(width + (/\./.test(number) ? 2 : 1)).join("0") + number;
        }

        return number + "";
    };


    var contador = function contador() {
        connection.invoke("SendUsuarios", user)["catch"](function (err) {
            return console.error(err.toString());
        });
    };

    function Cargar2() {

        $("#crearma").attr("action", "/client/rma/create/ ");
        setTimeout(mensaje, 5000);
    }

    function Cargar() {
        $("#crearma").attr("action", "/client/rma/save/ ");
        setTimeout(mensaje, 5000);
    }

    var folio = parseInt(document.getElementById("folio").value);
    var connection = new signalR.HubConnectionBuilder()
        .withUrl("/rmaHub")
        .build();
    var connectionpermiso = true;
    var folioid = folio - 1;
    connection.on("ReceiveMessageUsuarios", function (user, contador) {
        if (connectionpermiso == true) {
            folioid = folioid + contador;
            $("#foliorma").val("8" + zeroFill(folioid.toString(), 4));
            connectionpermiso = false;
        }

        console.log(user);
    });
    connection.on("disconnected", function (contador) {
        if (folioid != folio) {
            folioid = folioid - 1;

            if (folioid == folio) {
                folioid = folio + contador - 1;
            }
        }

        $("#foliorma").val("8" + zeroFill(folioid.toString(), 4));
        console.log(zeroFill(folioid.toString(), 4));
    });
    connection
        .start()
        .then(function () {
            contador();
        })
    ["catch"](function (err) {
        return console.error(err.toString());
    });

}
$('select[name="loc"]').live("change", function () {
    $(this).css("background", "");
});

function botonrechazo(event) {
    var botonrechazo = document.getElementById("botonrechazo");
    botonrechazo.disabled = false;
    $(".cargar").hide();
    var formulario = document.getElementById("frmrechazo");

    if (enviando == false) {
        enviando = true;
        formulario.submit();
        $(".gif").show();
    } else {
        console.log("The form is already being sent");
    }
}

function botonaprobar(event) {
    var botonaprobar = document.getElementById("botonaprobar");
    botonaprobar.disabled = false;
    $(".cargar").hide();
    var formulario = document.getElementById("frmaprobar");

    if (enviando == false) {
        enviando = true;
        formulario.submit();
        $(".gif").show();
    } else {
        console.log("The form is already being sent");
    }
}

if ($("#select").val() == "DISTY SCRAP ALLOWANCE") {
    $(".ocultar500").hide();
}

$("#select").on("change", function () {
    let select = $(this).val();

    if (select == "DISTY SCRAP ALLOWANCE") {
        $(".spam2").hide();
        $(".ocultar500").show();
    } else {
        $(".spam2").hide();
        $(".ocultar500").hide();
    }
});

(function () {
    var app = angular.module("app", [
        "angularUtils.directives.dirPagination",
        "ui.bootstrap",
    ]);

    app.factory("Ordershistory", [
        "$http",
        "$q",
        function ($http, $q) {
            var self = {
                cargando: false,
                err: false,
                conteo: 0,
                orders: [],
                pag_actual: 1,
                pag_siguiente: 1,
                pag_anterior: 1,
                total_paginas: 1,
                paginas: [],

                cargarPagina: function (pag, customer, orders) {
                    var d = $q.defer();

                    $http
                        .get(
                            "/Client/Corrective/GetAllInv6/?cus_no=" +
                            customer +
                            "&orders=" +
                            orders +
                            "&pagina=" +
                            pag +
                            "&por_pagina=10"
                        )
                        .success(function (data) {
                            self.err = data.err;
                            self.conteo = data.conteo;
                            self.orders = data.data;
                            self.pag_actual = data.pag_actual;
                            self.pag_siguiente = data.pag_siguiente;
                            self.pag_anterior = data.pag_anterior;
                            self.total_paginas = data.total_paginas;
                            self.paginas = data.paginas;

                            return d.resolve();
                        });

                    return d.promise;
                },
            };

            return self;
        },
    ]);


    app.factory("Ordershistorynew", [
        "$http",
        "$q",
        function ($http, $q) {
            var self = {
                cargando: false,
                err: false,
                conteo: 0,
                orders: [],
                pag_actual: 1,
                pag_siguiente: 1,
                pag_anterior: 1,
                total_paginas: 1,
                paginas: [],

                cargarPagina: function (pag, customer, orders) {
                    var d = $q.defer();

                    $http
                        .get(
                            "/Client/Corrective/GetAllInv6/?cus_no=" +
                            customer +
                            "&orders=" +
                            orders +
                            "&pagina=" +
                            pag +
                            "&por_pagina=10"
                        )
                        .success(function (data) {
                            self.err = data.err;
                            self.conteo = data.conteo;
                            self.orders = data.data;
                            self.pag_actual = data.pag_actual;
                            self.pag_siguiente = data.pag_siguiente;
                            self.pag_anterior = data.pag_anterior;
                            self.total_paginas = data.total_paginas;
                            self.paginas = data.paginas;

                            return d.resolve();
                        });

                    return d.promise;
                },
            };

            return self;
        },
    ]);





    app.filter("toFixed", function () {
        return function (palabra) {
            if (palabra) {
                return palabra.toFixed(4);
            }
        };
    });
    app.filter("fecha", function () {
        return function (mensaje) {
            if (mensaje) {
                return mensaje;
            }
        };
    });

    function OtherController($scope) {
        $scope.pageChangeHandler = function (num) {
            console.log("going to page " + num);
        };
    }
    function OtherController3($scope) {
        $scope.pageChangeHandler3 = function (num) {
            console.log("going to page " + num);
        };
    }
    function OtherController4($scope) {
        $scope.pageChangeHandler4 = function (num) {
            console.log("going to page " + num);
        };
    }

    app.controller("OtherController3", OtherController3);

    app.controller("OtherController5", OtherController5);

    app.controller("OtherController4", OtherController4);

    app.controller("OtherController2", OtherController2);

    app.controller("OtherController", OtherController);
    function OtherController5($scope) {
        $scope.pageChangeHandler5 = function (num) {
            console.log("going to page " + num);
        };
    }

    function OtherController2($scope) {
        $scope.pageChangeHandler2 = function (num) {
            console.log("going to page " + num);
        };
    }

    app.controller("Rma", [
        "$scope",
        "Ordershistory", "Ordershistorynew",
        function ($scope, Ordershistory, Ordershistorynew) {
            var pag = 1;
            $scope.orderhistory = {};
            $scope.searchorders = "0";

            var pagnew = 1;
            $scope.orderhistorynew = {};
            $scope.searchordersnew = "0";

            $scope.moverAnew = function (pag) {
                console.log(document.getElementById("client").value);

                Ordershistorynew.cargarPagina(
                    pagnew,
                    document.getElementById("client").value,
                    $scope.searchordersnew
                ).then(function () {
                    $scope.orderhistorynew = Ordershistorynew;
                    console.log($scope.orderhistorynew);
                });
            };

            $scope.moverAnew(pagnew);


            $scope.moverA = function (pag) {
                console.log(document.getElementById("client").value);

                Ordershistory.cargarPagina(
                    pag,
                    document.getElementById("client").value,
                    $scope.searchorders
                ).then(function () {
                    $scope.orderhistory = Ordershistory;
                    console.log($scope.orderhistory);
                });
            };

            $scope.moverA(pag);

            $scope.Mover = function (data) {
                let id = document.getElementById("client").value;
                console.log(document.getElementById("client").value);

                $("." + clase2 + "").css("color", "black");
                $("." + clase2 + "").css("background", "");

                $("." + clase4 + "").val(data);
                $("." + clase2 + "").val("");
                $("." + clase3 + "").val(0);
                $("." + clase4 + "").css("color", "green");
                if (id != "" && id != null) {
                    let select = document.querySelector("[id='" + clase7 + "']").options[
                        document.querySelector("[id='" + clase7 + "']").selectedIndex
                    ].value;
                    $("." + clase6 + "").val(0);
                    $("." + clase5 + "").val(0);

                    if (select != "" && select != null) {
                        $.ajax({
                            url:
                                "/client/corrective/GetAllM3/?filtro=" +
                                data +
                                "&filtro2=" +
                                select +
                                "&cliente=" +
                                id,
                            type: "GET",
                            dataType: "json",
                            cache: true,
                            success: function success(data) {
                                console.log(data);
                                $("." + clase6 + "").val(data.data.price.toFixed(6));
                                $("." + clase5 + "").val(data.data.std_cost.toFixed(6));
                                enviaDatos();
                                espacios();
                            },
                            error: function error(xhr, status, _error15) {
                                $().toastmessage("showToast", {
                                    text: "Error con ean " + xhr.responseText,
                                    sticky: true,
                                    type: "error",
                                });
                            },
                        });
                    }
                }
            };
            $scope.pageSize2 = 10;
            $scope.items = [];
            $scope.pageChangeHandler2 = function (num) {
                console.log("Orders page changed to " + num);
            };
            $.ajax({
                url: "/client/corrective/GetAllM/",
                type: "GET",
                datatype: "json",
                async: true,
                cache: true,

                success: function (data) {
                    console.log(data);
                    for (var i = 0; i < data.data.length; i++) {
                        $scope.items.push({
                            item_no: data.data[i].item_no,
                            item_desc_1: data.data[i].item_desc_1,
                        });
                    }
                    $scope.count2 = Object.keys($scope.items).length;

                    $("#ship").focus().attr("style", "background-color: #efef7b !important");

                },
                error: function (xhr, status, error) {
                    $().toastmessage("showToast", {
                        text: "Error con ean " + xhr.responseText,
                        sticky: true,
                        type: "error",
                    });
                },
            });

            $scope.pageSize5 = 10;
            $scope.pageChangeHandler5 = function (num) {
                console.log("Orders page changed to " + num);
            };

            $scope.pageSize4 = 10;
            $scope.todolistinv = [];
            $scope.pageChangeHandler4 = function (num) {
                console.log("Orders page changed to " + num);
            };

            $scope.pageSize3 = 10;
            $scope.todolistclient = [];
            $scope.pageChangeHandler3 = function (num) {
                console.log("Orders page changed to " + num);
            };
            $scope.$watch('p', function (newVal) {
                $scope.count3 = $scope.todolistclient.filter($scope.customFilter).length;
            });
            $.ajax({
                url: "/client/corrective/GetAllA/",
                type: "GET",
                datatype: "json",
                async: true,
                cache: true,

                success: function (data) {
                    console.log(data);



                    $scope.todolistclient = data.data;

                    $scope.count3 = Object.keys($scope.todolistclient).length;

                    $(document).ready(function () {
                        $("#client").focus().attr("style", "background-color: #efef7b !important");

                    });

                },
                error: function (xhr, status, error) {
                    $().toastmessage("showToast", {
                        text: "Error con ean " + xhr.responseText,
                        sticky: true,
                        type: "error",
                    });
                },
            });

            $scope.abririnv = function () {
                if ($scope.model2 == true) {
                    $scope.model2 = false;
                    const client = document.getElementById("client").value;

                    $scope.todolistinv = [];

                    $.ajax({
                        url: "/client/corrective/GetAllI2/?filtro=" + client,
                        type: "GET",
                        datatype: "json",
                        async: true,
                        cache: true,

                        success: function (data) {
                            console.log(data);
                            $scope.todolistinv = data;
                            $scope.count4 = Object.keys($scope.todolistinv).length;
                        },
                        error: function (xhr, status, error) {
                            $().toastmessage("showToast", {
                                text: "Error con ean " + xhr.responseText,
                                sticky: true,
                                type: "error",
                            });
                        },
                    });
                }
            };

            $scope.Mover7 = function (data) {
                $(".ship").show();
                console.log("click ships");
                console.log(data);
                $("#ship").val(data.cus_alt_adr_cd);
                $("#contact").val(data.contact_1);
                $("#phone").val(data.phone_no);
                $("#fax").val(data.fax_no);
                $("#ext").val(data.phone_ext);
                $("#contactemail").val(data.email_address);
                $("#companyemail").val(data.cmp_e_mail);
            };

            $scope.Mover5 = function (data) {
                $("#ship").val("");
                $scope.todoList = [];

                $(".t").css("color", "black");
                $(".t").css("background", "");

                $scope.todoAdd();

                $scope.count = 0;

                $.ajax({
                    url: "/client/corrective/GetAllInv5/?cus_no=" + data,
                    type: "GET",
                    datatype: "json",
                    async: true,
                    cache: true,

                    success: function (data) {
                        var data = JSON.parse(data);

                        console.log(data);
                        for (var i = 0; i < data.length; i++) {
                            $scope.count = 1;
                        }
                    },
                    error: function error(xhr, status, _error23) {
                        $().toastmessage("showToast", {
                            text: "Error con ean " + xhr.responseText,
                            sticky: true,
                            type: "error",
                        });
                    },
                });

                $scope.cliente = true;
                $scope.model = true;
                $scope.model2 = true;
                $("#client").val(data.trim());

                $("#creer").show();

                $("#client").css("color", "green");

                $(".spam4").hide();
                $(".spam3").hide();

                $("#contact").val("");
                $("#phone").val("");
                $("#fax").val("");
                $("#ext").val("");
                $("#contactemail").val("");
                $("#ship").val("");
                $("#companyemail").val("");

                $(".ship").hide();


                $scope.todoListships = [];

                $.ajax({
                    url: "/client/corrective/GetAllSt/?client=" + data,
                    type: "GET",
                    dataType: "json",
                    cache: true,
                    async: true,
                    success: function success(data) {
                        console.log(data);
                        $scope.todoListships = data;

                        $scope.count5 = Object.keys($scope.todoListships).length;
                    },
                    error: function error(xhr, status, _error17) {
                        $().toastmessage("showToast", {
                            text: "Error con ean " + xhr.responseText,
                            sticky: true,
                            type: "error",
                        });
                    },
                });
            };
            //PNS
            $scope.order = [];

            $scope.todoList = [];
            $scope.todoListships = [];
            $scope.cliente = false;
            $scope.model = false;
            $scope.model2 = false;
            $scope.count = 0;
            var proceso = document.getElementById("proceso").value;

            $scope.pageSize = 10;
            $scope.clientelineas = false;

            if (proceso == "edit") {
                let id = document.getElementById("client").value;

                $scope.todoListships = [];

                $.ajax({
                    url: "/client/corrective/GetAllSt/?client=" + id,
                    type: "GET",
                    dataType: "json",
                    cache: true,
                    async: true,
                    success: function success(data) {
                        console.log(data);
                        $scope.todoListships = data;

                        $scope.count5 = Object.keys($scope.todoListships).length;
                    },
                    error: function error(xhr, status, _error17) {
                        $().toastmessage("showToast", {
                            text: "Error con ean " + xhr.responseText,
                            sticky: true,
                            type: "error",
                        });
                    },
                });
                $scope.abririnv();
                $scope.cliente = true;
                $scope.clientelineas = true;

                $scope.model = true;
                $scope.model2 = true;
                $scope.count = 0;

                $.ajax({
                    url: "/client/corrective/GetAllInv5/?cus_no=" + id,
                    type: "GET",
                    datatype: "json",
                    async: true,
                    cache: true,

                    success: function (data) {
                        var data = JSON.parse(data);

                        console.log(data);
                        for (var i = 0; i < data.length; i++) {
                            $scope.count = 1;
                        }
                    },
                    error: function error(xhr, status, _error23) {
                        $().toastmessage("showToast", {
                            text: "Error con ean " + xhr.responseText,
                            sticky: true,
                            type: "error",
                        });
                    },
                });
            }



            $scope.ship = function () {
                mensaje();
                espacios();
                let ship = document.getElementById("ship").value;
                console.log(ship);
                if (ship != "" && ship != null) {
                    $scope.clientelineas = true;
                } else {
                    $scope.clientelineas = false;
                }
            };
            $scope.observado = false;
            $scope.$watch("clientobservado", function (newValue, oldValue) {
                if (newValue === oldValue) {
                    $scope.observado = false;
                    return;
                }
                $scope.observado = true;

                console.log(newValue + "-" + oldValue);
            });

            $scope.buscarclient = function () {
                let id = document.getElementById("client").value;

                $.ajax({
                    url: "/client/corrective/Getclientscodigo/?term=" + id,
                    type: "GET",
                    dataType: "json",
                    cache: true,
                    async: true,
                    success: function success(data) {
                        var availabconstags = data.result;
                        console.log(data);
                        $("#client").autocomplete({
                            source: availabconstags,
                            minlength: 2,
                            select: function select(event, ui) {
                                var customer = (ui.item.value).trim();
                                console.log("Customer_1: " + customer);
                                $scope.Mover5(customer);
                                /*document.getElementById("total").focus();*/
                            },
                            classes: {
                                "ui.autocomplete": "highlight",
                            },
                        });

                        if (data.existe != "existe") {
                            var x = document.getElementById("client");
                            x.style.color = "red";
                            console.log("Customer_2: " + x);
                            $scope.count = 0;
                            $(".spam4").show();

                            $(".ship").val("");
                            $scope.todoList = [];

                            $scope.todoAdd();

                            $("#ship").val("");
                            $scope.model = false;
                            $scope.model2 = false;
                            $scope.clientelineas = false;
                        } else {
                            let id = document.getElementById("client").value;
                            //showcusno

                            var customer = id.trim();
                            console.log("Customer_3: " + customer);

                            $scope.Mover5(customer);

                            $(".ship").hide();
                        }
                    },
                    error: function error(xhr, status, _error14) {
                        $().toastmessage("showToast", {
                            text: "Error con ean " + xhr.responseText,
                            sticky: true,
                            type: "error",
                        });
                    },
                });
            };

            $scope.customFilter = function (item) {
                if (!$scope.p) return true;
                var searchText = $scope.p.toLowerCase();
                return (item.data.cus_no && item.data.cus_no.toLowerCase().includes(searchText)) ||
                    (item.data.cus_name && item.data.cus_name.toLowerCase().includes(searchText)) ||
                    (item.data.curr_cd && item.data.curr_cd.toLowerCase().includes(searchText));
            };


            $scope.select = function () {
                if (
                    document.querySelector("[id='" + clase2 + "']").value == "" ||
                    document.querySelector("[id='" + clase2 + "']").value == null
                ) {
                    let select = document.querySelector("[id='" + clase7 + "']").options[
                        document.querySelector("[id='" + clase7 + "']").selectedIndex
                    ].value;
                    let id = document.getElementById("client").value;
                    let valor = $("." + clase4 + "").val();
                    $("." + clase6 + "").val(0);
                    $("." + clase5 + "").val(0);

                    if (select != "" && select != null) {
                        $.ajax({
                            url:
                                "/client/corrective/GetAllM3/?filtro=" +
                                valor.replace(/#/g, "%23") +
                                "&filtro2=" +
                                select +
                                "&cliente=" +
                                id,
                            type: "GET",
                            dataType: "json",
                            cache: true,
                            success: function success(data) {
                                console.log(data);
                                $("." + clase6 + "").val(data.data.price.toFixed(6));
                                $("." + clase5 + "").val(data.data.std_cost.toFixed(6));
                                enviaDatos();
                            },
                            error: function error(xhr, status, _error4) {
                                $().toastmessage("showToast", {
                                    text: "Error con ean " + xhr.responseText,
                                    sticky: true,
                                    type: "error",
                                });
                            },
                        });
                    }
                }
            };

            $scope.pageChangeHandler = function (num) {
                console.log("Orders page changed to " + num);
            };
            $scope.Mover9 = function (order) {
                var oldList = $scope.todoList;
                $scope.todoList = [];
                angular.forEach(oldList, function (x) {
                    if (x.coustumer != "") $scope.todoList.push(x);
                });
                setTimeout(enviaDatos, 5000);

                var linea = $scope.linea++;
                $scope.todoList.push({
                    linea: linea,
                    invoice: "",
                    seq: 0,
                    coustumer: order.item_no,
                    qty: "",
                    unit: parseFloat(order.price.toFixed(4)),
                    acttion: parseFloat(order.cost.toFixed(4)),
                    loc: "",
                    checkcar: false,
                    code: "",
                    done: false,
                });
            };

            $scope.Mover3 = function (data) {
                console.log(data.cus_alt_adr_cd);
                console.log($("#ship").val());

                if (
                    $("#ship").val().trim() === data.cus_alt_adr_cd.trim() ||
                    data.cus_alt_adr_cd == "               "
                ) {
                    $("." + clase2 + "").css("background", "green");
                    $("." + clase2 + "").css("color", "white");
                } else {
                    $("." + clase2 + "").css("color", "white");
                    $("." + clase2 + "").css("background", "green");
                }

                $("." + clase2 + "").val(data.inv_no);
                $("." + clase3 + "").val(0);
                $("." + clase4 + "").val("");

                $("#tblQ").dataTable().fnDestroy();
                dataTable = $("#tblQ").DataTable({
                    order: [[0, "desc"]],
                    autoWidth: false,
                    ajax: {
                        url: "/client/corrective/GetAllQ/?filtro=" + data.inv_no,
                        type: "GET",
                        dataType: "json",
                    },
                    columns: [
                        {
                            data: "lineSeqNo",
                            render: function render(data, type, row) {
                                return (
                                    "<a data-dismiss='modal' data-item=" + row.itemNo + " onclick=Mover4(" +
                                    data +
                                    ",'" +
                                    row.itemNo.replace(/\s/g, "¶").trim() +
                                    "'," +
                                    row.unitPrice +
                                    "," +
                                    row.unitCost +
                                    ")>" +
                                    data +
                                    "</a></td> "
                                );
                            },
                            width: "10%",
                        },
                        {
                            data: "ordType",
                            width: "10%",
                        },
                        {
                            data: "ordNo",
                            width: "10%",
                        },
                        {
                            data: "itemNo",
                            width: "10%",
                        },

                        {
                            data: "unitPrice",
                            width: "10%",
                        },

                        {
                            data: "unitCost",
                            width: "10%",
                        },
                    ],
                    language: {
                        emptyTable: "No records",
                    },
                    width: "100%",
                });
                espacios();
            };
            $scope.todoList = [];
            $scope.linea = 2;
            var id = parseInt(document.getElementById("id").value);
            $.ajax({
                url: "/client/corrective/GetAllCos/?filtro=" + id,
                type: "GET",
                dataType: "json",
                cache: true,
                async: true,
                success: function success(data) {
                    console.log(data);
                    console.log(id);

                    for (var i = 0; i < data.data.length; i++) {
                        var linea = $scope.linea++;
                        $scope.todoList.push({
                            linea: linea,
                            invoice: data.data[i].invoice,
                            seq: data.data[i].seq,
                            coustumer: data.data[i].coustumer,
                            qty: data.data[i].qty,
                            unit: data.data[i].unit,
                            acttion: data.data[i].cost,
                            loc: data.data[i].loc,
                            checkcar: data.data[i].car,
                            code: data.data[i].retur,
                            done: false,
                        });
                    }
                    setTimeout(enviaDatos, 5000);

                    setTimeout(validador, 5000);
                    if (data.data.length == 0) {
                        $scope.todoList.push({
                            linea: 1,
                            invoice: 0,
                            seq: 0,
                            coustumer: "",
                            qty: "",
                            unit: "",
                            acttion: "",
                            loc: "",
                            checkcar: false,
                            code: "",
                            done: false,
                        });
                    }
                },
                error: function error(xhr, status, _error23) {
                    $().toastmessage("showToast", {
                        text: "Error con ean " + xhr.responseText,
                        sticky: true,
                        type: "error",
                    });
                },
            });

            $scope.Dato = function (linea) {
                Dato(linea);
            };

            $scope.Dato2 = function (linea) {
                Dato2(linea);
            };

            $scope.todoAdd = function () {
                var linea = $scope.linea++;
                $scope.todoList.push({
                    linea: linea,
                    invoice: 0,
                    seq: 0,
                    coustumer: "",
                    qty: "",
                    unit: "",
                    acttion: "",
                    loc: "",
                    checkcar: false,
                    code: "",
                    done: false,
                });
            };

            $scope.importexcel = function () {
                var infoProducto = new FormData();
                var totalFicheros = 0;
                totalFicheros = $("#subidaArchivo2").get(0).files.length;

                if (totalFicheros != 0) {
                    for (var i = 0; i < totalFicheros; i++) {
                        infoProducto.append(
                            "archivos[" + i + "]",
                            $("#subidaArchivo2")[0].files[i]
                        );
                    }
                }

                var xhr = new XMLHttpRequest();
                xhr.open("POST", "/client/rma/Importexcel", true);
                xhr.open("POST", "/client/rma/Importexcel", false);
                $scope.financial = function (x) {
                    return x.toFixed(6);
                };

                xhr.onload = function () {
                    if (this.status === 200) {
                        var respuesta = JSON.parse(xhr.responseText);
                        swal(respuesta.message);
                        console.log(respuesta.data);

                        for (var _i = 0; _i < respuesta.data.length; _i++) {
                            var car = false;

                            if (respuesta.data[_i].car == "Yes") {
                                car = true;
                            }

                            var linea = $scope.linea++;

                            var unit = $scope.financial(parseFloat(respuesta.data[_i].unit));

                            var cost = $scope.financial(parseFloat(respuesta.data[_i].cost));

                            $scope.todoList.push({
                                linea: linea,
                                invoice: respuesta.data[_i].invoice,
                                seq: respuesta.data[_i].seq,
                                coustumer: respuesta.data[_i].coustumer,
                                qty: respuesta.data[_i].qty,
                                unit: parseFloat(unit),
                                acttion: parseFloat(cost),
                                loc: respuesta.data[_i].loc,
                                checkcar: car,
                                code: respuesta.data[_i].retur,
                                done: false,
                            });
                        }

                        setTimeout(validador, 5000);
                        setTimeout(enviaDatos, 5000);
                    } else {
                        swal("Not import excel");
                        console.log(this.status);
                    }
                };

                xhr.upload.onprogress = function (e) {
                    if (e.lengthComputable) {
                        var progreso = (e.loaded / e.total) * 100;
                        console.log(progreso);
                    }
                };

                xhr.send(infoProducto);
                $("#subidaArchivo2").val(null);
            };

            $scope.remove = function () {
                var oldList = $scope.todoList;
                $scope.todoList = [];
                angular.forEach(oldList, function (x) {
                    if (!x.done) $scope.todoList.push(x);
                });
                setTimeout(validador, 5000);
                setTimeout(enviaDatos, 5000);
            };
        },
    ]);
})();


