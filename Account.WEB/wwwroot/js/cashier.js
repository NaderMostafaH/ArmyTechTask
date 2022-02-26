var dataTable;

$(document).ready(function () {
        loadDataTable();   
});




function loadDataTable() {
    dataTable = $('#kt_table_widget_1').DataTable({
        "ajax": {
            "url": "/Cashier/GetAll"
        },
        "columns": [
            { "data": "cashierName", "width": "30%" },
            { "data": "branch", "width": "30%" },
            { "data": "ordersCount", "width": "20%" },
            {
                "data":  "id"
                ,
                "render": function (data) {
                    debugger
                    if (data) {
                        debugger
                        return `
                            <div class="text-center">
                                <a href="/Cashier/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i> 
                                </a>
                                <a onclick=Delete("/Cashier/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
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

function Delete(url) {
    swal({
        title: "هل أنت متاكد انك تريد حذف بيانات الكاشير ؟",
        text: "عند قيامك بحذف بيانات الكاشير فانك ستقوم بحذف جميع بيانات الفواتير التي قام بتسجيلها مسبقا !",
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