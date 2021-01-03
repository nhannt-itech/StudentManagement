
var dataTable;
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Manager/Subject/GetAll"
        },
        "columns": [
            { "data": "id"},
            { "data": "name"},
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                            <a href="/Manager/Subject/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                <i class="fas fa-edit"></i> 
                            </a>

                            <a  onclick=Delete("/Manager/Subject/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                <i class="fas fa-trash-alt"></i> 
                            </a>

                        </div>
                    `;
                }, 
            }
        ]
    });
}

function Delete(url) {
    swal({
        title: "Bạn có chắc chắn muốn xóa Môn học?",
        text: "Xóa môn học sẽ xóa tất cả dữ liệu của học sinh, bảng điểm, lớp học, thống kê có môn học",
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