

showInPopup = (url, title) => {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            $('#form-modal .modal-body').html(res);
            $('#form-modal .modal-title').html(title);
            $('#form-modal').modal('show');
        }
    })
}

jQueryAjaxPost = form => {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    $('#view-all').html(res.html)
                    $('#form-modal .modal-body').html('');
                    $('#form-modal .modal-title').html('');
                    $('#form-modal').modal('hide');
                }
                else
                    $('#form-modal .modal-body').html(res.html);
            },
            error: function (err) {
                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}

//Adds date automatically
$(document).ready(function () {
    document.getElementById('dateAddedInput').valueAsDate = new Date();
});



$(document).ready(function () {
    $('#myTable').DataTable({
        outerWidth: false,
        outerHeight: false,
        autoWidth: false,
        paging: true,        // Enable pagination
        searching: true,    // Enable search bar
        ordering: true,     // Enable column sorting
        responsive: true,   // Make table responsive
        lengthMenu: [10, 15, 25, 50, 75, 100], // Entries dropdown options
        pageLength: 10, // Default number of rows per page
        
        language: {
            search: "", // Change search label
            searchPlaceholder: "Search...",
            lengthMenu: "_MENU_", // Change entries label
            info: "Showing _START_ of _TOTAL_ entries", // Change info label
            paginate: {
                first: "<<",
                last: ">>",
                next: ">",
                previous: "<"
            }, // Change pagination labels

        },
        dom: "l<'row'<'col-md-6'B><'col-md-6 text-end'f>>" +
            "<rt>" +
            "<'row'<'col-md-6'i><'col-md-6'p>>",
        buttons: [
            {
                extend: 'collection',
                text: '<i class="fa-solid fa-file-export"></i> Export',
                className: 'btn-success btn-block export-btn',
                style: 'color: white;',
                buttons: ['copy', 'csv', 'excel', 'pdf', 'print']
            }
        ],
        initComplete: function (settings, json) {
            // Add a container for the export buttons
            $('.dt-buttons').wrap('<div class="button-group-container"></div>');


            // Add the export buttons after the initialization
            var colvisbtn = [
                {
                    extend: 'colvis', // Adding 'colvis' button
                    text: '<i class="fa-solid fa-table-columns"></i> Columns',
                    className: 'btn-light btn-block colvis-btn',
                    columns: ':not(.noVis)', // Exclude columns with class 'noVis'
                    postfixButtons: ['colvisRestore'] // Add the 'colvisRestore' button
                }
            ];
            new $.fn.dataTable.Buttons(settings, {
                buttons: colvisbtn
            }).container().appendTo($('.button-group-container'));

            // Add spacing between buttons
            $('.dt-buttons, .button-group-container .btn-success').addClass('button-spacing');
        }

    });
});




// Sidenav Toggle
window.addEventListener('DOMContentLoaded', event => {

    // Toggle the side navigation
    const sidebarToggle = document.body.querySelector('#sidebarToggle');
    if (sidebarToggle) {
         //Uncomment Below to persist sidebar toggle between refreshes
         if (localStorage.getItem('sb|sidebar-toggle') === 'true') {
             document.body.classList.toggle('sb-sidenav-toggled');
         }
        sidebarToggle.addEventListener('click', event => {
            event.preventDefault();
            document.body.classList.toggle('sb-sidenav-toggled');
            localStorage.setItem('sb|sidebar-toggle', document.body.classList.contains('sb-sidenav-toggled'));
        });
    }

});
