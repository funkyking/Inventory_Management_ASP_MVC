﻿
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
        source: partsUrl,
        select: function (event, ui) {
            // When a partName is selected, set the value of Get_PartNumber
            const selectedPartName = ui.item.value;
            $.ajax({

                url: GetPartNumberUrl,
                data: { term: selectedPartName },
                dataType: 'json',
                success: function (data) {
                    $('#Get_PartNumber').val(data);
                    $('#Get_PartNumber').val(data);
                }
            });
        }
    })

    $('#brand').autocomplete({
        source: brandUrl
    });


    $('#supplier').autocomplete({
        source: SupplierUrl
    })


});


// Function to fetch part names based on brand input
function fetchPartNames(brand) {
    const partNameUrl = '/InventoryMasters/GetPartName?brand=' + brand;

    $.ajax({
        url: partNameUrl,
        type: 'GET',
        success: function (data) {
            // Configure autocomplete for Part Name field with filtered data
            $('#partName').autocomplete({
                source: data
            });
        },
        error: function (error) {
            console.log(error);
        }
    });
}


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

