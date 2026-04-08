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