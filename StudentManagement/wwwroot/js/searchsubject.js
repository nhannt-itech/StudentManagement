
var dataTable;
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Teacher/SearchInfo/GetAllSubject"
        },
        "columns": [
            { "data": "id"},
            { "data": "name"},
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
                }, 
            }
        ]
    });
}

$('#Detail').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget) // Button that triggered the modal
    var idSubject = button.data('id') // Extract info from data-* attributes
    var modal = $(this)
    $.ajax({
        method: 'GET',
        url: '/Teacher/SearchInfo/DetailsSubject/' + idSubject,
        success: function (data) {
            console.log(data.idSubject);
            modal.find('#Id').val(data.id);
            modal.find('#Name').val(data.name);
            modal.find('#PassQuantity1').val(data.pass1);
            modal.find('#PercentTage1').val(data.per1);  
            modal.find('#PassQuantity2').val(data.pass2);
            modal.find('#PercentTage2').val(data.per2);  
        }
    })
})