var userDataTable;

$(document).ready(function () {
    loadDataTable();
})

function loadDataTable() {
    userDataTable = $('#tblData').DataTable({
        ajax: '/admin/user/getall',
        columns: [
            { data: 'name', "width": "25%" },
            { data: 'email', "width": "15%" },
            { data: 'phoneNumber', "width": "10%" },
            {
                data: 'role', "width": "10%", "render": function (data) { return '<span class="badge bg-secondary">' + data + '</span>'; }
            },
            {
                data: { id: "id", lockoutEnd:"lockoutEnd" }, "width": "25%", "render": function (data) {

                    var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();
                    var isLocked = lockout > today;

                    return `<div class="d-flex gap-2 justify-content-end">
                    <a onclick="LockUnlock('${data.id}')" class="btn btn-sm ${isLocked ? 'btn-danger' : 'btn-success'}">
                                 <i class="bi bi-${isLocked ? 'lock' : 'unlock'}-fill"></i> ${isLocked ?'Lock':'Unlock'}
                            </a>
                            <a href="/admin/user/RoleManagment?userId=${data.id}" class="btn btn-sm btn-outline-secondary">
                                 <i class="bi bi-person-badge"></i> Role
                            </a>
                              <a onclick="Delete('/admin/user/ChangePassword?userId=${data.id}')" class="btn btn-sm btn-outline-danger">
                                 <i class="bi bi-key-fill"></i> Password
                            </a>
                        </div > `;
                }
            }
        ]
    });
}

function LockUnlock(id) {
    $.ajax({
        type: "POST",
        url: '/admin/user/LockUnlock',
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                userDataTable.ajax.reload();
            }
        }
    });
}

