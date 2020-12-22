var dataTable;
//$(document).ready(function () {
//    loadDataTable();
//});

function loadDataTable() {
    $("#tblData").dataTable().fnDestroy();

    try {
        dataTable = $('#tblData').DataTable({
            "ajax": {
                "type": "POST",
                "url": "/Teacher/SummarySubject/Index/?subjectId=" + $("#SubjectId").val()
                    + "&grade=" + $("#Grade").val()
                    + "&year=" + $("#Year").val()
                    + "&semester=" + $("#Semester").val()
            },
            "columns": [
                { "data": "class.name" },
                { "data": "class.numStudents" },
                { "data": "passQuantity" },
                {
                    "data": "percentage",
                    "render": function (data) {
                        return data + '%';
                    }
                }
            ]
        });
    }
    catch{
        console.log('Hiện tại chưa có thống kê!');
    }
    
}
