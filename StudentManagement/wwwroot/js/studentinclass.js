var dataTable;

$(document).ready(function () {

    loadDataTable();

    $('.js-example-basic-single').select2();
    $.ajax({
        type: "GET",
        url: "/Teacher/Class/GetStudentNotInClass/",
        success: function (data) {

            $("#StudentList").addItems(data);
        }
    });
});

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
            "url": "/Teacher/Class/GetStudentInClass/" + $('#ClassId').val(),
        },
        "columns": [
            { "data": "id" },
            { "data": "name" },
            { "data": "gender" },
            { "data": "birth" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a href="/Teacher/Class/Upsert/${data}" class="btn btn-success bg-gradient-success text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i>
                                </a>
                                <a onclick=Delete("/Teacher/Class/Delete/${data}") class="btn btn-danger bg-gradient-danger text-white" style="cursor:pointer">
                                    <i class="fas fa-trash-alt"></i> 
                                </a>
                            </div>
                            `;
                }, "width": "15%"
            }
        ]
    });
}

function AddStudentInClass() {
    swalWithBootstrapButtons.fire({
        title: 'Thêm học sinh vào lớp',
        text: "Đồng thời sẽ tạo bảng điểm cho học sinh!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Có',
        cancelButtonText: 'Không',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
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
                    }
                    else {
                        swalWithBootstrapButtons.fire(
                            'Thất bại!',
                            'Mã học sinh không tồn tại.',
                            'error'
                        )
                    }
                }
            })
        }
        else if (result.dismiss === Swal.DismissReason.cancel) {
            swalWithBootstrapButtons.fire(
                'Hủy bỏ',
                'Học sinh chưa thêm vào lớp',
                'error'
            )
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

const swalWithBootstrapButtons = Swal.mixin({
    customClass: {
        confirmButton: 'btn btn-success',
        cancelButton: 'btn btn-danger'
    },
    buttonsStyling: false
})
