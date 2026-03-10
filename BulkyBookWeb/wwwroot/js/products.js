$('#tblData').DataTable({
    ajax: '/product/getall',
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
                            <a href="/product/edit?id=${data}" class="btn btn-sm btn-outline-success">
                                 <i class="bi bi-pencil-square"></i> Edit
                            </a>
                              <a href="/product/edit?id=${data}" class="btn btn-sm btn-outline-danger">
                                 <i class="bi bi-trash"></i> Delete
                            </a>
                        </div > `;
            }
        }
    ]
});