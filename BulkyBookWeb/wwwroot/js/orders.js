var productDataTable;

$(document).ready(function () {
    productDataTable();
})

productDataTable= $('#tblData').DataTable({
    ajax: '/admin/order/getall',
    columns: [
        { data: 'id', "width": "5%" },
        { data: 'name', "width": "20%" },
        { data: 'phoneNumber', "width": "15%" }, 
        { data: 'applicationUser.email', "width": "20%" },
        {
            data: 'orderStatus', "width": "15%", "render": function (data)
            { return '<span class="badge bg-secondary">' + data + '</span>'; }
        },

        { data: 'orderTotal', "width": "15%", "render": function (data) { return '$' + data.toFixed(2); } },
        {
            data: 'id', "width": "10%", "render": function (data)
            {
                return `<div class="d-flex gap-2 justify-content-end">
                            <a href="/admin/order/details?orderId=${data}" class="btn btn-sm btn-outline-success">
                                 <i class="bi bi-pencil-square"></i> Details
                            </a>
                             
                        </div > `;
            }
        }
    ]
});

