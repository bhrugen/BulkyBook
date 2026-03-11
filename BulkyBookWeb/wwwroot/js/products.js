var productDataTable;

$(document).ready(function () {
    productDataTable();
})

productDataTable= $('#tblData').DataTable({
    ajax: '/admin/product/getall',
    columns: [
        { data: 'title', "width": "25%" },
        { data: 'isbn', "width": "15%" },
        { data: 'price', "width": "10%", "render": function (data) { return '$' + data.toFixed(2); } },
        { data: 'author', "width": "15%" },
        {
            data: 'category.name', "width": "10%", "render": function (data)
            { return '<span class="badge bg-secondary">' + data + '</span>'; }
        },
        {
            data: 'id', "width": "25%", "render": function (data)
            {
                return `<div class="d-flex gap-2 justify-content-end">
                            <a href="/admin/product/upsert?id=${data}" class="btn btn-sm btn-outline-success">
                                 <i class="bi bi-pencil-square"></i> Edit
                            </a>
                              <a onclick="Delete('/admin/product/delete/${data}')" class="btn btn-sm btn-outline-danger">
                                 <i class="bi bi-trash"></i> Delete
                            </a>
                        </div > `;
            }
        }
    ]
});


function Delete(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {

            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    productDataTable.ajax.reload();
                    Swal.fire({
                        title: "Deleted!",
                        text: "Your file has been deleted.",
                        icon: "success"
                    });
                }
            })
        }
    });
}