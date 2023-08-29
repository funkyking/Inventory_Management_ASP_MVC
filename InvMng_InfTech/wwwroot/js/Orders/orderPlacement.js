// Global Variables

// Main
let tempOrderId = 0;
let customerId = 0;
let tempOrderItems = [];




/* Customer */
let newCustomerData = {};



/* Table */

let ActiveRow = 0; // Last User Accessed Row
let rowCount = 0; // TotalRows
const itemsPerPage = 5; // Max Displayed items in the table
let currentPage = 1; // Table Page Index

//Before submitting the form, convert the orderItemsMap to an array and clear the map.
//const orderItemsArray = Object.values(orderItemsMap);
//orderItemsMap = {}; // Clear the map for the next set of changes
const orderItemsMap = {}; // Use this to store order items
//let orderItemsArray = []; //Temp row data storage as OrderItem.cs




// Customer adding and confirming
$(document).ready(function () {

    // Check Customer Button Click
    $('#checkCustomerBtn').on('click', function () {
        const customerName = $('#customerName').val().trim();
        if (customerName === '') {
            alert('Please enter a valid customer name.');
            return;
        }

        const customer = { name: customerName };

        // Perform an Ajax request to check if the customer exists in the database
        // You can use jQuery.ajax() or fetch() to perform the Ajax request
        // Example using jQuery.ajax():
        $.ajax({
            url: '/Orders/CheckCustomerExistence',
            method: 'GET',
            data: customer, // Send the entire customer object
            success: function (data) {
                if (data.exists) {
                    customerId = data.customerId;

                    $('#customerIdInput').val(customerId); //Pass Value to Html for submission
                    $('#addCustomerBtn').addClass('d-none');
                    $('#checkCustomerBtn').removeAttr('disabled');//allow user to check again//.attr('disabled', true);
                    $('#submitOrderBtn').removeAttr('disabled');
                    $('#customerStatus').html('<i class="fa-regular fa-circle-check fa-bounce" style="color: #0bae23;"></i>');
                } else {
                    newCustomerData = customer; // Save the entire customer object
                    $('#addCustomerBtn').removeClass('d-none');
                    $('#checkCustomerBtn').removeAttr('disabled');//.attr('disabled', true);
                    $('#customerStatus').html('<i class="fa-regular fa-circle-xmark fa-beat-fade" style="color: #ff5353;"></i>');
                }
            },
            error: function () {
                alert('Error checking customer existence.');
            }
        });
    });

    // Add New Customer Button Click (Show Bootstrap Modal)
    $('#addCustomerBtn').on('click', function () {

        const Name = $('#customerName').val();
        const Email = Name + '@mail.com'; 

        $('#newCustomerName').val(Name);
        $('#newCustomerEmail').val(Email);


        $('#newCustomerModal').modal('show');
    });

    // Save Customer Button in Bootstrap Modal Click
    $('#saveCustomerBtn').on('click', function () {
        const customerName = $('#newCustomerName').val().trim();
        const customerEmail = $('#newCustomerEmail').val().trim();

        // Validate customer name and email fields
        if (customerName === '' || customerEmail === '') {
            alert('Please enter both customer name and email.');
            return;
        }

        // Perform an Ajax request to create a new customer in the database
        // You can use jQuery.ajax() or fetch() to perform the Ajax request
        // Example using jQuery.ajax():

        // 
        $.ajax({
            url: '/Orders/CreateNewCustomer',
            method: 'POST',
            data: { name: customerName, email: customerEmail },
            success: function (data) {
                if (data.success) {
                    customerId = data.customerId;
                    $('#addCustomerBtn').addClass('d-none');
                    $('#checkCustomerBtn').removeAttr('disabled');//allow user to check again//.attr('disabled', true);
                    $('#submitOrderBtn').removeAttr('disabled');
                    $('#customerStatus').html('<i class="fa-regular fa-circle-check fa-bounce" style="color: #0bae23;"></i><p>Customer created successfully.</p>');
                    $('#newCustomerModal').modal('hide');



                    const cInfo = `Successfully Created Customer
                                    \nName: ${customerName}\nEmail: ${customerEmail}`;

                    // Display toast notification
                    Toastify({
                        text: cInfo,
                        duration: 10000,
                        gravity: "bottom",
                        backgroundColor: "#0bae23",
                    }).showToast();


                } else {
                    alert('Error creating the new customer.');
                }
            },
            error: function () {
                alert('Error creating the new customer.');
            }
        });
    });
});

// Display Customer list on page load
$(document).ready(function () {
    // Function to fetch all customers from the database and display them in the list
    function getAllCustomers() {
        $.ajax({
            url: '/Orders/GetAllCustomers',
            method: 'GET',
            success: function (data) {
                // Display all customers in the list
                const customerListHtml = data.map(customer =>
                    `<li class="list-group-item">${customer.name}</li>`
                ).join('');

                $('#customerDetailsSection').html(customerListHtml);

                // Show the customer list
                $('.customer-list').show();
            },
            error: function () {
                alert('Error fetching customer data.');
            }
        });
    }

    // Load customer list on page load
    getAllCustomers();

    // Function to filter the customer list based on the input text
    function filterCustomers(inputText) {
        $.ajax({
            url: '/Orders/GetCustomerList',
            method: 'GET',
            data: { searchText: inputText },
            success: function (data) {

                // If there are Customers matched
                if (data.length > 0) {

                    // Display the filtered customer list in the dropdown
                    const customerListHtml = data.map(customer =>
                        `<li class="list-group-item">${customer.name}</li>`
                    ).join('');

                    $('#customerDetailsSection').html(customerListHtml);

                    // Show the customer list
                    $('.customer-list').show();

                } else {
                    $('.customer-list').hide();
                }
                
            },
            error: function () {
                alert('Error fetching customer data.');
            }
        });
    }

    // Listen for the input event on the customerName input box
    $('#customerName').on('input', function () {
        const inputText = $(this).val().trim();
        if (inputText.length > 0) {
            filterCustomers(inputText); // If input is not empty, filter customers
        } else {
            getAllCustomers(); // If input is empty, display all customers
        }
    });

    // Handle click on customer details in the dropdown
    $('#customerDetailsSection').on('click', 'li', function () {
        const selectedCustomerName = $(this).text();
        // Set the selected customer name in the input box
        $('#customerName').val(selectedCustomerName);
        // Hide the dropdown
        $('.customer-list').hide();
    });

    // Hide the dropdown when clicking outside the input box
    $(document).on('click', function (e) {
        if (!$('.customer-list').is(e.target) && $('.customer-list').has(e.target).length === 0) {
            $('.customer-list').hide();
        }
    });
});


// Handles Brand,PartId and Quantity
$(document).ready(function () {


    // Function to fetch the list of brands and populate the Brand dropdown
    function fetchBrandList() {
        $.ajax({
            url: '/Orders/GetBrandList',
            method: 'GET',
            success: function (data) {
                // Populate the Brand dropdown template with the retrieved data
                const brandDropdown = `
                    <select class="form-control brand-dropdown" name="OrderItems[${rowCount}].Brand" required>
                      <option value="">Select Brand</option>
                      ${data.map(brand => `<option value="${brand}">${brand}</option>`).join('')}
                    </select>
                  `;

                // Handle Add Order Item button click
                $('#addOrderItemBtn').on('click', function () {
                    // Increment the row counter
                    rowCount++;

                    // Create a new row for the table
                    const newRow = `
                        <tr class="row-{ActiveRow}">
                            <td class="text-center">
                                ${brandDropdown}
                            </td>
                            <td class="text-center">
                                <select class="form-control part-id-dropdown" name="OrderItems[${rowCount}].PartId" required>
                                    <option value="">Select PartId</option>
                                </select>
                            </td>
                            <td class="text-center">
                                <div class="d-flex align-items-center justify-content-center">
                                    <span class="remaining-quantity mr-2"></span>
                                    <input type="number" class="form-control quantity-input ml-2" name="OrderItems[${rowCount}].Quantity" style="width: 100px;" required>
                                </div>
                            </td>
                            <td class="text-center">
                                <button type="button" class="btn btn-danger btn-sm remove-item" onclick="removeRow(this)">
                                    <i class="fa-solid fa-xmark" style="color: #ffffff;"></i>
                                </button>
                            </td>
                        </tr>

                    `;

                    // Append the new row to the table body
                    $('#orderItemsSection table tbody').append(newRow);

                    // Fetch the list of PartIds for the selected Brand when the row is added
                    fetchPartIdList(rowCount);


                    // Pagination And Items Displayed in the Table
                    pagination();

                });
            },
            error: function () {
                alert('Error fetching brand data.');
            }
        });
    }

    // Fetch the list of brands when the page loads
    fetchBrandList();

    // Function to fetch the list of PartIds and populate the PartId dropdown for a specific row
    function fetchPartIdList(rowNumber) {


        selectedRow = rowNumber;

        // Handle change event on the Brand dropdown (inside the table rows)
        $(`#orderItemsSection table tbody tr:nth-child(${rowNumber})`).on('change', '.brand-dropdown', function () {
            const selectedBrand = $(this).val();
            const partIdDropdown = $(this).closest('tr').find('.part-id-dropdown');
            const remainingQuantitySpan = $(this).closest('tr').find('.remaining-quantity');

            // Perform an AJAX request to fetch the list of PartIds based on the selected Brand
            $.ajax({
                url: '/Orders/GetPartIdList',
                method: 'GET',
                data: { brand: selectedBrand },
                success: function (data) {
                    // Populate the PartId dropdown with the retrieved data
                    partIdDropdown.empty();
                    partIdDropdown.append('<option value="">Select PartId</option>');
                    data.forEach(partId => {
                        partIdDropdown.append(`<option value="${partId}">${partId}</option>`);
                    });
                },
                error: function () {
                    alert('Error fetching PartId data.');
                }
            });
        });

        // Handle change event on the PartId dropdown (inside the table rows)
        $(`#orderItemsSection table tbody tr:nth-child(${rowNumber})`).on('change', '.part-id-dropdown', function () {
            const selectedBrand = $(this).closest('tr').find('.brand-dropdown').val();
            const selectedPartId = $(this).val();
            const selectedQuantity = parseInt($(this).closest('tr').find('input[name^="OrderItems["]').val(), 10);
            const remainingQuantitySpan = $(this).closest('tr').find('.remaining-quantity');
            

            // Perform an AJAX request to get the remaining quantity
            $.ajax({
                url: '/Orders/GetRemainingQuantity',
                method: 'GET',
                data: { brand: selectedBrand, partId: selectedPartId },
                success: function (data) {
                    // Check if the remaining quantity is greater than 0
                    if (data > 0) {
                        remainingQuantitySpan.text(`Qty: ${data}`);
                    } else {
                        remainingQuantitySpan.text('Qty: 0.');
                    }
                },
                error: function () {
                    alert('Error fetching remaining quantity.');
                }
            });
        });


        // Handle change evnet on the quantity Input (inside the table rows)
        $(`#orderItemsSection table tbody tr:nth-child(${rowNumber})`).on('change', '.quantity-input', function () {
            const selectedBrand = $(this).closest('tr').find('.brand-dropdown').val();
            const selectedPartId = $(this).closest('tr').find('.part-id-dropdown').val();
            const selectedQuantity = $(this).val();

        });
    }

    // Determine which row is being Actively Selected
    $('#orderItemsSection').on('change', '.brand-dropdown, .part-id-dropdown, .quantity-input, .remove-item', function () {
        
    });


    $('#orderItemsSection').on('change', '.brand-dropdown, .part-id-dropdown, .quantity-input, .remove-item', function () {
        //Determines Active Row
        ActiveRow = $(this).closest('tr').index();
        pagination();


        const selectedBrand = $(this).closest('tr').find('.brand-dropdown').val();
        const selectedPartId = $(this).closest('tr').find('.part-id-dropdown').val();
        const currentRow = $(this).closest('tr').index();
        const selectedQuantity = $(this).closest('tr').find('.quantity-input').val();
        



        tableManagement(currentRow, selectedBrand, selectedPartId, selectedQuantity);

    });



});









function tableManagement(currentRow, selectedBrand, selectedPartId, selectedQuantity) {

    // Check for Duplicate First
    if (findDuplicate(currentRow, selectedBrand, selectedPartId)) {
        alert('Combination already exists!');


        const partIdDropdown = $(`#orderItemsSection table tbody tr:nth-child(${currentRow + 1}) .part-id-dropdown`);

        // Set the first option as selected
        partIdDropdown.val(partIdDropdown.find('option:first').prop('value'));

        // Update selectedPartId
        selectedPartId = partIdDropdown.val();

        createNewItemArray(currentRow, selectedBrand, selectedPartId, selectedQuantity);

    }
    else {
        createNewItemArray(currentRow, selectedBrand, selectedPartId, selectedQuantity);
    }
}

function findDuplicate(currentRow, selectedBrand, selectedPartId) {

    let duplicateFound = false;

    // Iterate through each row except the current one
    $('#orderItemsSection table tbody tr').each(function (index, row) {
        if (index !== currentRow) {
            const rowBrand = $(row).find('.brand-dropdown').val();
            const rowPartId = $(row).find('.part-id-dropdown').val();

            if (rowBrand === selectedBrand && rowPartId === selectedPartId) {
                duplicateFound = true;
                return false; // Exit the loop early
            }
        }
    });

    return duplicateFound;
}

function createNewItemArray(currentRow, selectedBrand, selectedPartId, selectedQuantity) {

    //Before submitting the form, convert the orderItemsMap to an array and clear the map.
    //const orderItemsArray = Object.values(orderItemsMap);
    //orderItemsMap = {}; // Clear the map for the next set of changes

    // Check if an entry exists for the current row
    if (orderItemsMap[currentRow]) {
        // Update existing values
        orderItemsMap[currentRow] = {
            Brand: selectedBrand,
            PartId: selectedPartId,
            Quantity: selectedQuantity,
        };
    } else {
        // Add to orderItemsMap
        orderItemsMap[currentRow] = {
            Brand: selectedBrand,
            PartId: selectedPartId,
            Quantity: selectedQuantity,
        };
    }

}











// Main Function For Pagination and Items Displayed on the table
function pagination() {
    displayItems();
    updatePagination();
}


// Function to display items for the current page
function displayItems() {

    const rows = $('#orderItemsSection table tbody tr');
    const totalRows = rows.length;
    const totalPages = Math.ceil(totalRows / itemsPerPage);

     // Ensure that currentPage is within valid bounds
    if (currentPage < 1) {
        currentPage = 1;
    } else if (currentPage > totalPages) {
        currentPage = totalPages;
    }

    const startIndex = (currentPage - 1) * itemsPerPage;
    const endIndex = Math.min(startIndex + itemsPerPage, totalRows);


    // Hide all rows in the table
    rows.hide();


    // Show rows for the current page
    rows.slice(startIndex, endIndex).show();

}


// Function to update the pagination links
function updatePagination() {
    const totalRows = $('#orderItemsSection table tbody tr').length;
    const totalPages = Math.ceil(totalRows / itemsPerPage);
    $('#pagination').empty();

    for (let i = 1; i <= totalPages; i++) {
        const paginationItem = `
            <li class="page-item ${i === currentPage ? 'active' : ''}">
                <a class="page-link" href="#" data-page="${i}">${i}</a>
            </li>
        `;
        $('#pagination').append(paginationItem);
    }

    // Add click event listener to pagination links
    $('#pagination a.page-link').click(function (e) {
        e.preventDefault();
        currentPage = $(this).data('page');
        displayItems();
        updatePagination();
    });
}




// Function to remove a row from the table
function removeRow(btn) {

    //This works
    rowCount--;
    $(btn).closest('tr').remove();
    pagination();
}



$(document).ready(function () {
    // Call the function when appropriate, e.g., on a button click
    $('#createOrderButton').on('click', function () {
        createOrderAndItems();
    });
});

function createOrderAndItems() {
    // Collect necessary data
    const totalAmount = parseFloat($('#InputTotalAmount').val());

    // Create the OrderItems
    const orderItemsArray = Object.values(orderItemsMap);

    // Send a POST request to create the OrderItems
    $.ajax({
        url: '/Orders/CreateOrderItems',
        type: 'POST',
        data: JSON.stringify(orderItemsArray),
        contentType: 'application/json',
        success: function (response) {
            if (response.success) {
                // OrderItems created successfully, now create the Order
                const orderData = {
                    totalAmount: totalAmount,
                    customerId: customerId,
                    orderItems: response.orderItems // Sent back from the server

                };

                tempOrderItems = response.orderItems;

                


                // Send a POST request to create the Order
                $.ajax({
                    url: '/Orders/CreateOrder',
                    type: 'POST',
                    data: JSON.stringify(orderData),
                    contentType: 'application/json',
                    success: function (response) {
                        if (response.success) {
                            //console.log("Order and OrderItems created successfully.");
                            //console.log("Generated OrderId:", response.orderId);
                            tempOrderId = response.orderId;


                            // Update OrderItems with the generated orderId
                            updateOrderItemsWithOrderId();
                        } else {
                            console.error("Failed to create Order:", response.message);
                        }
                    },
                    error: function (error) {
                        console.error("An error occurred while creating Order:", error);
                    }
                });
            } else {
                console.error("Failed to create OrderItems:", response.message);
            }
        },
        error: function (error) {
            console.error("An error occurred while creating OrderItems:", error);
        }
    });
}

function updateOrderItemsWithOrderId() {


    for (const eachitem in tempOrderItems) {
        tempOrderItems[eachitem].orderId = tempOrderId;
    }
    const orderItemsArray = tempOrderItems;
    console.log(orderItemsArray);


    // Send a POST request to update OrderItems with OrderId
    $.ajax({
        url: '/Orders/UpdateOrderItemsWithOrderId',
        type: 'POST',
        data: JSON.stringify(orderItemsArray),//JSON.stringify(orderItemsArray),
        contentType: 'application/json',
        success: function (response) {
            if (response.success) {
                console.log("OrderItems updated with OrderId successfully.");
            } else {
                console.error("Failed to update OrderItems with OrderId:", response.message);
            }
        },
        error: function (error) {
            console.error("An error occurred while updating OrderItems with OrderId:", error);
        }
    });
}




$(document).ready(function () {

    $('#toast').on('click', function () {
        Toastify({
            text: "This is a toast",
            duration: 3000,
            destination: "https://github.com/apvarun/toastify-js",
            newWindow: true,
            close: true,
            gravity: "top", // `top` or `bottom`
            position: "right", // `left`, `center` or `right`
            stopOnFocus: true, // Prevents dismissing of toast on hover
            style: {
                background: "linear-gradient(to right, #00b09b, #96c93d)",
            },
            onClick: function () { } // Callback after click
        }).showToast();
    });


});





