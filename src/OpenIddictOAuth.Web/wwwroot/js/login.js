"use strict";

// Class definition
let KTLoginForm = function () {
    // Shared variables
    const form = document.querySelector('#kt_sign_in_form');

    // Init code generator form
    let initSignInForm = () => {
        // Init form validation rules. For more info check the FormValidation plugin's official documentation:https://formvalidation.io/
        let validator = FormValidation.formValidation(
            form,
            {
                fields: {
                    'UserNameOrEmailAddress': {
                        validators: {
                            notEmpty: {
                                message: 'This field is required'
                            }
                        }
                    },
                    'Password': {
                        validators: {
                            notEmpty: {
                                message: 'This field is required'
                            }
                        }
                    }
                },
                plugins: {
                    trigger: new FormValidation.plugins.Trigger(),
                    submitButton: new FormValidation.plugins.SubmitButton(),
                    bootstrap: new FormValidation.plugins.Bootstrap5({
                        rowSelector: '.fv-row'
                    })
                }
            }
        );

        const socialLoginButtons = document.getElementsByClassName('social-login-link');
        console.log(socialLoginButtons);
        const submitButton = document.querySelector('#kt_sign_in_submit');
        submitButton.addEventListener('click', function (e) {
            // Disable button to avoid multiple click 
            submitButton.disabled = true;
            // Prevent default button action
            e.preventDefault();

            // Validate form before submit
            if (validator) {
                validator.validate().then(function (status) {
                    if (status === 'Valid') {
                        // Show loading indication
                        submitButton.setAttribute('data-kt-indicator', 'on');

                        for (let i = 0; i < socialLoginButtons.length; i++) {
                            socialLoginButtons[i].classList.add('disabled-link');
                        }

                        const actionUrl = $(form).attr('action');

                        $.ajax({
                            url: actionUrl,
                            type: 'POST',
                            data: $(form).serialize()
                        }).done(function (response) {
                            window.location.href = response?.redirectUrl ?? "/";
                        })
                        .fail(function (err) {
                            displayError(err);
                        })
                        .always(function () {
                            submitButton.disabled = false;
                            submitButton.removeAttribute('data-kt-indicator');
                            for (let i = 0; i < socialLoginButtons.length; i++) {
                                socialLoginButtons[i].classList.remove('disabled-link');
                            }
                        });
                    }
                    else {
                        submitButton.disabled = false;
                    }
                });
            }
        });
    }

    return {
        // Public functions
        init: function () {
            initSignInForm();
        }
    };
}();

KTUtil.onDOMContentLoaded(function () {
    KTLoginForm.init();
});