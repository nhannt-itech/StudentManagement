var dataTable;
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Manager/Rule/GetAll"
        },
        "columns": [
            { "data": "name" },
            { "data": "min" },
            { "data": "max" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a href="/Manager/Rule/Update/${data}" class="btn btn-success bg-gradient-success text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i>
                                </a>
                            </div>
                            `;
                }, "width": "15%"
            }
        ]
    });
}
