var persona = "";
var user = "";

function Fil() {
    if (navigator.userAgent.match(/msie/i) || navigator.userAgent.match(/trident/i)) {

        var filtro = document.getElementById("inputpart").value;
        var numeroCaracteres = filtro.length;
        if (numeroCaracteres > 1) {

            var now = new Date();
            var datetime = now.getFullYear() + '/' + (now.getMonth() + 1) + '/' + now.getDate();
            datetime += ' ' + now.getHours() +

                ':' + now.getMinutes() + ':' + now.getSeconds();
            $.ajax({
                url: "/Client/corrective/GetAllM2/?filtro=" + filtro + "&fecha=" + datetime,
                type: "GET",
                datatype: "json",
                cache: true,

                success: function (data) {
                    $("#tblrma").children("tbody").empty();
                    for (i = 0; i < data.data.length; i++) {





                        $("#tblrma").children("tbody").append('<tr>' +
                            '<td ><a data-dismiss="modal" onclick=Mover("' + data.data[i].item_no.trim() + '")>' + data.data[i].item_no.trim() + '</a></td>' +
                            '<td > ' + data.data[i].item_desc_1.trim() + ' </td></tr>');


                    }





                },
                error: function (xhr, status, error) { $().toastmessage('showToast', { text: 'Error con ean ' + xhr.responseText, sticky: true, type: 'error' }); }
            });
        }

    }

}

function Fil2() {

    if (navigator.userAgent.match(/msie/i) || navigator.userAgent.match(/trident/i)) {

        var filtro2 = document.getElementById("inputpart").value;
        var filtro = document.getElementById("inputdes").value;
        var numeroCaracteres = filtro.length;
        if (numeroCaracteres > 0) {

            var now = new Date();
            var datetime = now.getFullYear() + '/' + (now.getMonth() + 1) + '/' + now.getDate();
            datetime += ' ' + now.getHours() +

                ':' + now.getMinutes() + ':' + now.getSeconds();
            $.ajax({
                url: "/Client/corrective/GetAllMD/?filtro=" + filtro + "&filtro2=" + filtro2 + "&fecha=" + datetime,
                type: "GET",
                datatype: "json",
                cache: true,

                success: function (data) {
                    $("#tblrma").children("tbody").empty();
                    for (i = 0; i < data.data.length; i++) {





                        $("#tblrma").children("tbody").append('<tr>' +
                            '<td ><a data-dismiss="modal" onclick=Mover("' + data.data[i].item_no.trim() + '")>' + data.data[i].item_no.trim() + '</a></td>' +
                            '<td > ' + data.data[i].item_desc_1.trim() + ' </td></tr>');


                    }





                },
                error: function (xhr, status, error) { $().toastmessage('showToast', { text: 'Error con ean ' + xhr.responseText, sticky: true, type: 'error' }); }
            });
        }

    }




}


function finestraSecundaria(url) {
    var popUpWidth = screen.width * 0.70; // Display popup window covering 70% of the screen.
    var popUpHeight = screen.height * 0.70;
    var iframe = '<html><head><style>body, html {width: 100%; height: 100%; margin: 0; padding: 0}</style></head><body><iframe src="' + url.replace(/¶/g, " ") + '" style="height:calc(100% - 4px);width:calc(100% - 4px)"></iframe></html></body>';
    var win = window.open("", "Amphenol Industrial", "height=" + popUpHeight + ",width=" + popUpWidth + ",menubar=0,menubar=0,resizable=1,scrollbars=1,titlebar=0,status=1");
    win === null || win === void 0 ? void 0 : win.frames.document.write(iframe);
}
function back() {
    history.back();
}
function comprobarxmlHttpRequest() {
    let request;
    try {
        request = new XMLHttpRequest();
    }
    catch (trymicrosoft) {
        try {
            request = new ActiveXObject("Msxml2.XMLHTTP");
        }
        catch (othermicrosoft) {
            try {
                request = new ActiveXObject("Microsoft.XMLHTTP");
            }
            catch (failed) {
                request = false;
            }
        }
    }
    if (!request)
        alert("Error inicializando XMLHttpRequest!");
}
function focusin() {
    $(document).on('focusin', function (e) {
        if ($(e.target).closest(".tox-tinymce, .tox-tinymce-aux, .moxman-window, .tam-assetmanager-root").length) {
            e.stopImmediatePropagation();
        }
    });
    //tinymce.init({
    //    selector: 'textarea',
    //       plugins: 'print preview paste importcss searchreplace autolink autosave save directionality code visualblocks visualchars fullscreen image link media template codesample table charmap hr pagebreak nonbreaking anchor toc insertdatetime advlist lists wordcount imagetools textpattern noneditable help charmap quickbars emoticons',
    //    imagetools_cors_hosts: ['picsum.photos'],
    //    menubar: 'file edit view insert format tools table help',
    //    toolbar: 'undo redo | bold italic underline strikethrough | fontselect fontsizeselect formatselect | alignleft aligncenter alignright alignjustify | outdent indent |  numlist bullist | forecolor backcolor removeformat | pagebreak | charmap emoticons | fullscreen  preview save print | insertfile image media template link anchor codesample | ltr rtl',
    //    toolbar_sticky: true,
    //    autosave_ask_before_unload: false,
    //    autosave_interval: '30s',
    //    autosave_prefix: '{path}{query}-{id}-',
    //    autosave_restore_when_empty: false,
    //    autosave_retention: '2m',
    //    image_advtab: true

    //});
   
}
  //var now = new Date();
  //  var datetime = now.getFullYear() + '/' + (now.getMonth() + 1) + '/' + now.getDate();
  //  datetime += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds();

 
  //      //my documents
  //      $.ajax({
  //          url: "/Client/drawings/GetAll2/?status=status1&fecha=" + datetime,
  //          type: "GET",
  //          dataType: "json",
  //          cache: true,
  //          success: function success(data) {
  //              console.log("my documents");
  //              console.log(JSON.parse(data.data)); 
               
  //          },
  //          error: function error(xhr, status, _error) {
  //              console.log("Error my documents");
  //              console.log(xhr.responseText);
  //          }
  //      });


$.ajax({
    url: "/Client/Tasks/GetAlluserloginadmin",
    type: "GET",
    dataType: "json",
    cache: true,
    success: function success(data) {
        if (data.data == "admin") {
            $('.panel').show();
        } else {
            $('.panel').hide();
        }
    },
    error: function error(xhr, status, _error) {
        $().toastmessage('showToast', {
            text: 'Error con ean ' + xhr.responseText,
            sticky: true,
            type: 'error'
        });
    }
});


$.ajax({
    "url": "/Client/corrective/GetAllpersona",
    type: "GET",
    dataType: "json",
    cache: true,
    async: true,
    success: function success(data) {
        persona = data.data;
    },
    error: function error(xhr, status, _error3) {
        $().toastmessage('showToast', {
            text: 'Error con ean ' + xhr.responseText,
            sticky: true,
            type: 'error'
        });
    }
});




$.ajax({
    url: "/client/Tasks/GetAlluserlogin",
    type: "GET",
    dataType: "json",
    async: true,
    cache: true,
    success: function success(data) {
        user = data.nombre;
    },
    error: function error(xhr, status, _error2) {
        $().toastmessage('showToast', {
            text: 'Error con ean ' + xhr.responseText,
            sticky: true,
            type: 'error'
        });
    }
});
$('.validar').live('keydown', function (event) {
    if (event.keyCode == 127) {
        return true;
    }

    if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && event.keyCode !== 190 && event.keyCode !== 110 && event.keyCode !== 8 && event.keyCode !== 9) {
        return false;
    } 
});

function mensaje() {
    $("label.error").hide();
}

function espacios() {
    var inputs = $("input[type=text]");

    for (var i = 0; i < inputs.length; i++) {
        var aux = $(inputs[i]).val().trim();
        //$(inputs[i]).val(aux);
    }
}

comprobarxmlHttpRequest();

//# sourceMappingURL=metodos.js.map