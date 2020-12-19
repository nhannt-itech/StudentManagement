var dataTable;

$(document).ready(function () {

    loadDataTable();

    $('.js-example-basic-single').select2();
    ReloadStudent();
});

function ReloadStudent() {
    var select = document.getElementById("StudentList");
    var length = select.options.length;
    for (i = length - 1; i >= 0; i--) {
        select.options[i] = null;
    }

    $.ajax({
        type: "GET",
        url: "/Teacher/Class/GetStudentNotInClass/",
        success: function (data) {
            $("#StudentList").addItems(data);
        }
    });
}

$.fn.addItems = function (data) {
    return this.each(function () {
        $.each(data, function (index, itemData) {
            var opt = document.createElement('option');
            opt.appendChild(document.createTextNode(itemData.text));
            opt.value = itemData.value;
            document.getElementById('StudentList').appendChild(opt);
        });
    });
};

function loadDataTable() {
    dataTable = $('#tblStudentInClass').DataTable({
        "ajax": {
            "url": "/Teacher/Class/GetStudentInClass/" + $('#ClassId').val()
        },
        "columns": [
            { "data": "id" },
            { "data": "name" },
            { "data": "gender" },
            {
                "data": "birth",
                "render": function (data) {
                    return new Date(data).toLocaleDateString();
                }
            },
            { "data": "address" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a onclick=Delete("${data}") class="btn btn-danger bg-gradient-danger text-white" style="cursor:pointer">
                                    <i class="fas fa-trash-alt"></i> 
                                </a>
                            </div>
                            `;
                }
            }
        ]
    });
}

function AddStudentInClass() {
    $.ajax({
        type: "POST",
        url: "/Teacher/Class/AddStudentInClass/?classId=" + $('#ClassId').val() + "&studentId=" + $('#StudentList').val(),
        success: function (data) {
            if (data.success) {
                swalWithBootstrapButtons.fire(
                    'Thêm thành công!',
                    'Học sinh đã được thêm vào lớp học.',
                    'success'
                );
                dataTable.ajax.reload();
                ReloadStudent();
            }
            else {
                swalWithBootstrapButtons.fire(
                    'Thất bại!',
                    data.message,
                    'error'
                )
            }
        }
    })
}

function SelectGradeSetClassName() {
    var grade = $('#Grade').val();
    var year = $('#Year').val();
    if (grade != "") {
        $.ajax({
            type: "GET",
            url: "/Teacher/Class/SelectGradeSetClassName/?grade=" + grade + "&year=" + year,
            success: function (data) {
                $('#Name').attr('value', data.name);
            }
        });
    }
    else {
        $('#Name').attr('value', '');
    }
}

function Delete(id) {
    swalWithBootstrapButtons.fire({
        title: 'Bạn có muốn xóa học sinh?',
        text: "Khi xóa lớp bạn sẽ xóa hết học sinh trong lớp và bảng điểm có sẵn!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Có',
        cancelButtonText: 'Không',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "DELETE",
                url: "/Teacher/Class/DeleteStudentFromClass/?classId=" + $('#ClassId').val() + "&studentId=" + id,
                success: function (data) {
                    if (data.success) {
                        swalWithBootstrapButtons.fire(
                            'Deleted!',
                            'Your file has been deleted.',
                            'success'
                        );
                        dataTable.ajax.reload();
                        ReloadStudent();
                    }
                    else {
                        swalWithBootstrapButtons.fire(
                            'Error',
                            'Can not delete this, maybe it not exit or error from sever',
                            'error'
                        )
                    }
                }
            })
        }
        else if (result.dismiss === Swal.DismissReason.cancel) {
            swalWithBootstrapButtons.fire(
                'Cancelled',
                'Your record is safe :)',
                'error'
            )
        }
    })
}

const swalWithBootstrapButtons = Swal.mixin({
    customClass: {
        confirmButton: 'btn btn-success',
        cancelButton: 'btn btn-danger'
    },
    buttonsStyling: false
})
