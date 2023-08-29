

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
        lengthMenu: [5, 10, 25, 50, 75, 100], // Entries dropdown options
        pageLength: 5, // Default number of rows per page
        language: {
            search: "", // Change search label
            lengthMenu: "Show _MENU_ entries", // Change entries label
            info: "Showing _START_ of _TOTAL_ entries", // Change info label
            paginate: {
                first: "<<",
                last: ">>",
                next: ">",
                previous: "<"
            }, // Change pagination labels
        },

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
