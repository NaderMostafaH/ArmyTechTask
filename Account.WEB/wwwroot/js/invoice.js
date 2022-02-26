var dataTable;

$(document).ready(function () {
        loadDataTable();   
});

function loadDataTable() {
    var listData = '';
    dataTable = $('#kt_table_widget_1').DataTable({
        "ajax": {
            "url": "/Invoice/GetAll"
        },
        "columns": [
            { "data": "id", "width": "10%" },
            { "data": "customerName", "width": "20%" },
            { "data": "branch", "width": "15%" },
            { "data": "cashierName", "width": "10%" },
            {
                "data": "invoiceDetails",
                "render": function (invoiceDetails) {
                    if (invoiceDetails.length > 0) {
                        listData = '';
                        $.each($(invoiceDetails), function (key, value) {
                            listData = listData + '<li>' + value.itemName + '</li>'
                        })
                        return `
                            <ul>'${listData}'</ul>
                        `;
                    }
                    else {
                        return ``;
                    }
                },

                "width": "10%"
            },
            { "data": "price", "width": "10%" },
            {
                "data":  "id"
                ,
                "render": function (data) {
                    debugger
                    if (data) {
                        debugger
                        return `
                            <div class="text-center">
                                 <a onclick=AddItem("${data}") class="btn btn-primary text-white" style="cursor:pointer">
                                    <i class="fas fa-plus"></i>
                                </a>
                                <a href="/Invoice/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i> 
                                </a>
                                <a onclick=Delete("/Invoice/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
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
function AddItem(id) {
    $("#invoiceHeaderId").val(id);
    debugger
  $('#modal-Item').modal("show");
}
function AddInvoice() {
    debugger
    getBranchList()
    $('#modal-Invoice').modal("show");
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
function AddInvoicePost() {

    $("#AddInvoice").submit(function (e) {

        e.preventDefault(); // avoid to execute the actual submit of the form.

        var form = $("#AddInvoice");
        debugger
        $.ajax({
            type: "POST",
            url: '/Invoice/Upsert',
            data: form.serialize(), // serializes the form's elements.
            success: function (data) {
                if (data.success) {
                    $('#modal-Invoice').modal("hide");

                    $(':input', '#AddInvoice')
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
function getBranchList() {

    if (document.readyState) {
        $.ajax({
            url: "/Invoice/GetBranch",
            data: {}, //parameters go here in object literal form
            type: 'GET',
            datatype: 'json',
            success: function (data) {
                //alert('got here with data');

                $("#BranchList").empty();
                $('#BranchList').append($('<option value="">-اختر الفرع</option>')).prop("selectedIndex", 0);
                var selOpts = "";
                /*$.each($(invoiceDetails), function (key, value) {*/
                $.each(data, function (k, v) {

                    debugger
                    selOpts += "<option value='" + v.id + "'>" + v.branchName + "</option>";
                });
                $('#BranchList').append(selOpts);
            },
            error: function () {
                alert('something bad happened');
            }
        });

    } else {
        $('#BranchList').empty();

        $('#BranchList').prepend($('<option>-اختر الفرع</option>')).prop("selectedIndex", 0);
    }
};
function onBranchChange() {

    var branchid = document.getElementById('BranchList').value;

    if (branchid != null && branchid != 0) {
        $.ajax({
            url: "/Invoice/GetCashier",
            data: { id: branchid }, //parameters go here in object literal form
            type: 'GET',
            datatype: 'json',
            success: function (data) {
                //alert('got here with data');

                $("#CashierList").empty();
                $('#CashierList').append($('<option value="">-اختر الكاشير</option>')).prop("selectedIndex", 0);
                var selOpts = "";
                /*$.each($(invoiceDetails), function (key, value) {*/
                    $.each(data, function (k, v) {

                        debugger
                        selOpts += "<option value='" + v.id + "'>" + v.cashierName + "</option>";
                });
                $('#CashierList').append(selOpts);
            },
            error: function () {
                alert('something bad happened');
            }
        });

    } else {
        $('#CashierList').empty();

        $('#CashierList').prepend($('<option>-اختر الكاشير</option>')).prop("selectedIndex", 0);
    }
}