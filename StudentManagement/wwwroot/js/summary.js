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
                "url": "/Teacher/Summary/Index/?grade=" + $("#Grade").val()
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
                        return Math.round(data * 100) + '%';
                    }
                }
            ]
        });
    }
    catch{
        console.log('Hiện tại chưa có thống kê!');
    }

}
