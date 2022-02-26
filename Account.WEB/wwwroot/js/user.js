var dataTable;

$(document).ready(function () {

    var url = window.location.search;
    if (url.includes("all")) {
        loadDataTable("GetAll?status=all");
    }
    else {
        if (url.includes("Deactive")) {
            loadDataTable("GetAll?status=Deactive");
        }
        else {
            if (url.includes("Active")) {
                loadDataTable("GetAll?status=Active");
            }
            else {
                loadDataTable("GetAll?status=all");
            }
        }
    }
});


function loadDataTable(url) {
    var listData = '';
    dataTable = $('#tblData').DataTable({
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": "/Admin/User/" + url,
            "type": "POST",
            "datatype": "json"
        },
        "columns": [
            { "data": "email", "width": "25%" },
            { "data": "userName", "width": "25%" },
            { "data": "phoneNumber", "width": "10%" },
            {
                "data": "userType",
                "render": function (userType) {
                    switch (userType) {
                        case 'S':
                            return `طالب`;
                            break;
                        case 'E':
                            return `موظف`;
                            break;
                        case 'A':
                            return `مدير النظام`;
                            break;
                        case 'D':
                            return `ع هـ تدريس`;
                            break;
                        case 'T':
                            return `ع هـ معاونة`;
                            break;
                        default:
                            return '';
                            break;
                    }
                },
                "width": "10%"
            },
            {
                "data": "rolesList",
                "render": function (rolesList) {
                    if (rolesList.length > 0) {
                        listData = '';
                        $.each($(rolesList), function (key, value) {
                            listData = listData + '<li>' + value + '</li>'
                        })
                        return `
                            <ul>'${listData}'</ul>
                        `;
                    }
                    else {
                        return ``;
                    }
                    
                },
                "width": "15%"
            },
            {
                "data": {
                    id: "id", lockoutEnd: "lockoutEnd"
                },
                "render": function (data) {
                    var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();
                    if (lockout > today) {
                        //user is currently locked
                        return `
                            <div class="text-center">
                                <a onclick=LockUnlock('${data.id}')  style="cursor:pointer;">
                                    <i class="fas fa-lock-open"></i>
                                </a>
                                <a href=/Admin/User/ManageRoles/${data.id} style="cursor:pointer;">
                                    <i class="fas fa-user-tag" style="cursor:pointer;"></i>
                                </a>
                                <a href=/Admin/User/Edit/${data.id} style="cursor:pointer;">
                                    <i class="fas fa-edit" style="cursor:pointer;"></i>
                                </a>
                                <a href=/Admin/User/ResetPassword/${data.id} style="cursor:pointer;">
                                    <i class="fas fa-key" style="cursor:pointer;"></i>
                                </a>
                            </div>
                           `;
                    }
                    else {
                        return `
                            <div class="text-center">
                                <a onclick=LockUnlock('${data.id}')  style="cursor:pointer;">
                                    <i class="fas fa-lock"></i>
                                </a>
                                <a href=/Admin/User/ManageRoles/${data.id} style="cursor:pointer; width:100px;">
                                    <i class="fas fa-user-tag" style="cursor:pointer;"></i>
                                </a>
                                <a href=/Admin/User/Edit/${data.id} style="cursor:pointer;">
                                   <i class="fas fa-edit" style="cursor:pointer;"></i>
                                </a>
                                <a href=/Admin/User/ResetPassword/${data.id} style="cursor:pointer;">
                                    <i class="fas fa-key" style="cursor:pointer;"></i>
                                </a>
                            </div>
                           `;
                    }

                }, "width": "25%"
            }
        ]
    });
}

function LockUnlock(id) {

    $.ajax({
        type: "POST",
        url: '/Admin/User/LockUnlock',
        data: JSON.stringify(id),
        contentType: "application/json",
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