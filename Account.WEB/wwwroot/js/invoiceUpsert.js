var dataTable;

$(document).ready(function () {
    var id = document.getElementById("InvoiceId").value;
    loadDataTable(id);
});


function loadDataTable(id) {

    dataTable = $('#kt_table_widget_1').DataTable({
        "ajax": {
            "url": "/Invoice/GetAllItems",
            "data": { "id": id },
            "type": "POST",
            "datatype": "json"
        },
        "columns": [
            { "data": "itemName", "width": "20%" },
            { "data": "itemCount", "width": "15%" },
            { "data": "itemPrice", "width": "10%" },   
            {
                "data": "id"
                ,
                "render": function (data) {
                    debugger
                    if (data) {
                        debugger
                        return `
                            <div class="text-center"> 
                                <a onclick=Update("/InvoiceDetail/Upsert/${data}") class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i> 
                                </a>
                                <a onclick=Delete("/InvoiceDetail/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                    <i class="fas fa-trash-alt"></i>
                                </a>
                            </div>
                        `;
                    }

                }, "width": "20%"
            }
        ]
    });
}
function Update(url) {
    $("#ModalLabel").html("Update Invoice Item");

    $(document).ready(function () {
        $.ajax({
            url: url,    //parameters go here in object literal form
            type: 'GET',
            datatype: 'json',
            success: function (data) {
                debugger
                //alert('got here with data');
                $("#id").val(data.data.id);
                $("#invoiceHeaderId").val(data.data.invoiceHeaderId);
                $("#itemName").val(data.data.itemName);
                $("#itemCount").val(data.data.itemCount);
                $("#itemPrice").val(data.data.itemPrice);
                $("#CashierList").empty();

            },
            error: function () {
                alert('something bad happened');
            }
        });
        $('#modal-Item').modal("show");
    });




}
function UpdatePost() {
    
    $("#itemUpdate").submit(function (e) {

        e.preventDefault(); // avoid to execute the actual submit of the form.

        var form = $("#itemUpdate");
        debugger
        $.ajax({
            type: "POST",
            url: '/InvoiceDetail/Upsert',
            data: form.serialize(), // serializes the form's elements.
            success: function (data) {
                if (data.success) {
                    $("#id").val("");
                    $('#modal-Item').modal("hide");
                    $(':input', '#itemUpdate')
                        .not(':button, :submit, :reset, :hidden')
                        .val('')
                        .prop('checked', false)
                        .prop('selected', false);
                    toastr.success(data.message);
                    dataTable.ajax.reload();
                }
                else {
                    toastr.error(data.message);
                } // show response from the  script.
            }
        });

    });
}
function AddItem(id) {
    debugger
    $("#invoiceHeaderId").val(id);
    $("#ModalLabel").html("Add Invoice Item");
    
    $('#modal-Item').modal("show");
}
function AddItemPost() {

    $("#AddItem").submit(function (e) {

        e.preventDefault(); // avoid to execute the actual submit of the form.

        var form = $("#AddItem");
        debugger
        $.ajax({
            type: "POST",
            url: '/InvoiceDetail/Upsert',
            data: form.serialize(), // serializes the form's elements.
            success: function (data) {
                if (data.success) {
                    $(':input', '#AddItem')
                        .not(':button, :submit, :reset, :hidden')
                        .val('')
                        .prop('checked', false)
                        .prop('selected', false);
                    toastr.success(data.message);
                    dataTable.ajax.reload();
                }
                else {
                    toastr.error(data.message);
                } // show response from the  script.
            }
        });

    });
}
// this is the id of the form
function Delete(url) {
    swal({
        title: "Are you sure you want to Delete?",
        text: "You will not be able to restore the data!",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}

function onBranchChange() {

    var branchid = document.getElementById('Branch').value;

    if (branchid != null && branchid != 0) {
        $.ajax({
            url: "/Invoice/GetCashier",
            data: { id: branchid }, //parameters go here in object literal form
            type: 'GET',
            datatype: 'json',
            success: function (data) {
                //alert('got here with data');

                $("#Cashier").empty();
                $('#Cashier').append($('<option value="">-اختر الكاشير-</option>')).prop("selectedIndex", 0);
                var selOpts = "";
                /*$.each($(invoiceDetails), function (key, value) {*/
                $.each(data, function (k, v) {

                    debugger
                    selOpts += "<option value='" + v.id + "'>" + v.cashierName + "</option>";
                });
                $('#Cashier').append(selOpts);
            },
            error: function () {
                alert('something bad happened');
            }
        });

    } else {
        $('#Cashier').empty();

        $('#Cashier').prepend($('<option value="">-اختر الكاشير -</option>')).prop("selectedIndex", 0);
    }
}