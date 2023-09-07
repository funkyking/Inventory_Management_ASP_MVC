
/* Create Page Functions */

// Function to preview uploaded image
$(document).ready(function previewImage(input) {
    var preview = document.getElementById('partImagePreview');
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            var img = new Image();
            img.src = e.target.result;
            img.onload = function () {
                var canvas = document.createElement('canvas');
                var ctx = canvas.getContext('2d');
                canvas.width = 150;
                canvas.height = 150;
                ctx.drawImage(img, 0, 0, canvas.width, canvas.height);
                preview.src = canvas.toDataURL('image/jpeg'); // You can also use 'image/png'
            };
        }
        reader.readAsDataURL(input.files[0]);
    } else {
        preview.src = "";
    }
});


// Suggest values (Location, SubLocation and Brand)
$(document).ready(function () {
    // Define the URL endpoints for fetching suggestions
    const locationUrl = '/InventoryMasters/GetLocations';
    const subLocationUrl = '/InventoryMasters/GetSubLocations';
    const brandUrl = '/InventoryMasters/GetBrandName';
    const partsUrl = '/InventoryMasters/GetPartName';
    const GetPartNumberUrl = '/InventoryMasters/GetPartNumber'
    const SupplierUrl = '/InventoryMasters/GetSupplierName'

    // Configure autocomplete for Location field
    $('#location').autocomplete({
        source: locationUrl
    });

    // Configure autocomplete for SubLocation field
    $('#subLocation').autocomplete({
        source: subLocationUrl
    });

    $('#partName').autocomplete({
        source: function (request, response) {
            const selectedBrandName = $('#brand').val(); // Get the selected brand name
            $.ajax({
                url: partsUrl,
                data: {
                    term: request.term,
                    brand: selectedBrandName // Pass the selected brand name as a parameter
                },
                dataType: 'json',
                success: function (data) {
                    response(data); // Provide the response data for autocomplete
                }
            });
        },
        select: function (event, ui) {
            const selectedPartName = ui.item.value;
            $.ajax({
                url: GetPartNumberUrl,
                data: { term: selectedPartName },
                dataType: 'json',
                success: function (data) {
                    $('#partNumber').val(data);

                    const partNumber = data;
                    $.ajax({
                        url: '/InventoryMasters/_ExistingStockCheck',
                        type: 'GET',
                        data: {
                            PartNumber: partNumber,
                            timestamp: new Date().getTime() // Add a timestamp to prevent caching
                        },
                        success: function (data) {
                            if (!data) {
                                $('#notImportant').show(); // Fade in the container
                            } else {
                                $('#notImportant').hide(); // Fade out the container
                            }
                        },
                        error: function (error) {
                            console.error(error);
                        }
                    });
                }
            });
        }
    });

    $('#brand').autocomplete({
        source: brandUrl
    });


    $('#supplier').autocomplete({
        source: SupplierUrl
    })


});


// Auto Generate Part Number
$(document).ready(function () {
    // Get the input element
    const partNumberInput = $('#partNumber');

    // Get the generate button
    const generateButton = $('#generatePartNumber');

    // Attach a click event to the generate button
    generateButton.click(function () {
        // Call the GenerateNewPartNumber action method
        $.get('/InventoryMasters/GeneratePartNumber', function (data) {
            // Set the value of the input box
            partNumberInput.val(data);
        });
    });
});

