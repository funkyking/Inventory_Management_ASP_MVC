

// Reset Button to remove all the forms textbox
document.getElementById('resetButton').addEventListener('click', function () {
    // Reset all input fields
    var inputFields = document.querySelectorAll('input[type="text"], input[type="email"], input[type="tel"]');
    inputFields.forEach(function (field) {
        field.value = '';
    });
});