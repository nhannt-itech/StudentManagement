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
                            <div class="text-center"   >
                                <a href="#" data-target="#Detail" data-toggle="modal" data-id="${data}" 
                                class="btn btn-success" style="font-size:small">Chi tiết</a>

                                </a>              
                        </div>
                            `;
                }, "width": "15%"
            } 
        ]
    });
}



$('#Detail').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget) // Button that triggered the modal
    var idClass = button.data('id') // Extract info from data-* attributes
    var modal = $(this)
    $.ajax({
        method: 'GET',
        url: '/Teacher/Class/Details/' + idClass,
        success: function (data) {
            console.log(data.idClass);
            modal.find('#Id').val(data.id);
            modal.find('#Name').val(data.name);
            modal.find('#Year').val(data.year);
            modal.find('#NumStudents').val(data.numStudents);
            modal.find('#Grade').val(data.grade);
            modal.find('#hocSinhGioi').val(data.gioi);
            modal.find('#hocSinhKha').val(data.kha);
            modal.find('#hocSinhTB').val(data.tb);
            modal.find('#hocSinhYeu').val(data.yeu);


        }
    })
})