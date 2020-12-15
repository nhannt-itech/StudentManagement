var dataTable;
$(document).ready(function () {
    loadDataTable();
    $('.js-example-basic-single').select2();

    $.ajax({
        type: "GET",
        url: "/Teacher/Class/GetStudent/",
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
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Teacher/Class/GetAll"
        },
        "columns": [
            { "data": "name" },
            { "data": "year" },
            { "data": "numStudents" },
            { "data": "grade" },
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