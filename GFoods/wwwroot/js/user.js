﻿var dataTable;
$(document).ready(function () {
    loadDataTable();
});
function loadDataTable() {

    dataTable = $('#tblData').DataTable({
        "ajax": {
            url: '/Admin/User/GetAll',
        },
        "columns": [
            { "data": 'name', "width": "20%" },
            { "data": 'email', "width": "15%" },
            { "data": 'phoneNumber', "width": "10%" },
            { "data": 'company.name', "width": "10%" },
            { "data": 'role', "width": "10%" },

            {
                "data": { id: "id", lockoutEnd: "lockoutEnd" },
                "render": function (data) {
                    var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();
                    if (lockout > today) {
                        return `
                        <div class="text-center">
                            <a onclick=LockUnLock('${data.id}') class="btn btn-danger text-white" style="cursor:pointer;width:150px">
                                <i class="bi bi-lock-fill"></i> Khóa
                            </a>
                            <a href="/admin/user/RoleManagment?userId=${data.id}" class="btn btn-danger text-white" style="cursor:pointer;width:150px">
                                <i class="bi bi-pencil-square"></i> Permission
                            </a>
                        </div>
                    `
                    } else {
                        return `
                        <div class="text-center">
                            <a onclick=LockUnLock('${data.id}') class="btn btn-success text-white" style="cursor:pointer;width:150px">
                                <i class="bi bi-unlock-fill"></i> Mở khóa
                            </a>
                            <a href="/admin/user/RoleManagment?userId=${data.id}" class="btn btn-danger text-white" style="cursor:pointer;width:150px">
                                <i class="bi bi-pencil-square"></i> Permission
                            </a>
                        </div>
                    `
                    }
                },
                "width": "35%"
            }
        ],
    });
}
function LockUnLock(id) {
    $.ajax({
        type: "POST",
        url: "/admin/User/LockUnLock",
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                dataTable.ajax.reload();
            }
        }
    });
}
