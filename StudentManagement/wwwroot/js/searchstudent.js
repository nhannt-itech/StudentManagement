$(document).ready(function () {
    $('#dataTable').DataTable({
        "ajax": {
            "url": "/Manager/Student/getall"
        },
        "columns": [
            { "data": "name" },
            { "data": "gender" },
            { "data": "birth" },
            { "data": "address" },
            { "data": "email" },
            { "data": "yearToSchool" },
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
                }, "width": "8%"
            }
        ]
    });
});

$('#Detail').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget) // Button that triggered the modal
    var idStudent = button.data('id') // Extract info from data-* attributes
    var modal = $(this)
    $.ajax({
        method: 'GET',
        url: '/Manager/Student/Details/' + idStudent,
        success: function (data) {
            console.log(data.idDiscount);
            modal.find('#Id').val(data.id);
            modal.find('#Name').val(data.name);
            modal.find('#Gender').val(data.gender);
            modal.find('#Birth').val(data.birth);
            modal.find('#Address').val(data.address);
            modal.find('#Email').val(data.email);
            modal.find('#YearToSchool').val(data.yearToSchool);
            modal.find('#ClassStudent').val(data.inClass);
            modal.find('#RecordSubject').val(data.scores);
            modal.find('#Achievements').val(data.achievements)
        }
    })
})