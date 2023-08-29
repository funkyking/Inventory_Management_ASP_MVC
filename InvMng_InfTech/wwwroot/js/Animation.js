
// Beat Animation for Icon
$(document).ready(function () {
    $('.beating-icon').hover(
        function () {
            $(this).addClass('fa-beat');
        },
        function () {
            $(this).removeClass('fa-beat');
        }
    );
});


// Spin Animation for Icon
$(document).ready(function () {
    $('.spin-icon').hover(
        function () {
            $(this).addClass('fa-spin');
        },
        function () {
            $(this).removeClass('fa-spin');
        }
    );
});

