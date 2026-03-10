$('#tblData').DataTable({
    ajax: '/product/getall',
    columns: [
        { data: 'title' },
        {defaultContent:''}
    ]
});