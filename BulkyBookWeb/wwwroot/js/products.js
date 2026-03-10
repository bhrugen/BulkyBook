$('#tblData').DataTable({
    ajax: '/product/getall',
    columns: [
        { data: 'title' },
        { data: 'isbn' },
        { data: 'price' },
        { data: 'author' },
        { defaultContent: '' },

        {defaultContent:''}
    ]
});