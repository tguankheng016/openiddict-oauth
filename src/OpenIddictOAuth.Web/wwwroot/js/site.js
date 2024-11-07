// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function displayError(err) {
    console.log(err);
    let errorMessage = err?.responseJSON?.detail;
    displayErrorMessage(errorMessage);
}

function displayErrorMessage(errMessage) {
    if (!notNullOrEmpty(errMessage)) {
        return;
    }

    Swal.fire({
        html: errMessage,
        icon: "error",
        buttonsStyling: false,
        confirmButtonText: "Ok, got it!",
        customClass: {
            confirmButton: "btn font-weight-bold btn-primary"
        }
    });
}

function displaySuccessMessage(successMessage) {
    if (!notNullOrEmpty(successMessage)) {
        return;
    }

    Swal.fire({
        html: successMessage,
        icon: "success",
        buttonsStyling: false,
        confirmButtonText: "Ok, got it!",
        customClass: {
            confirmButton: "btn font-weight-bold btn-primary"
        }
    });
}

function notNullOrEmpty(value) {
    return value !== '' && value !== null && value !== undefined;
}

function serializeForm(formElement) {
    if (!notNullOrEmpty(formElement)) {
        return undefined;
    }

    let formData = new FormData(formElement);

    // Convert FormData object to a query string
    return new URLSearchParams(formData).toString();
}

KTUtil.onDOMContentLoaded(function () {
    toastr.options = {
        "closeButton": false,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toastr-bottom-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };

    //initDarkMode();
});