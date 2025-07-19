// site.js - Common JavaScript functions for the store application

// Wait for the DOM to be fully loaded
document.addEventListener('DOMContentLoaded', function () {
    console.log('Store application initialized');
    
    // Initialize any interactive elements
    initializeTooltips();
    setupFormValidation();
});

// Initialize tooltip elements if Bootstrap is being used
function initializeTooltips() {
    // Check if Bootstrap's tooltip function exists
    if (typeof bootstrap !== 'undefined' && bootstrap.Tooltip) {
        var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
        tooltipTriggerList.map(function (tooltipTriggerEl) {
            return new bootstrap.Tooltip(tooltipTriggerEl);
        });
    }
}

// Setup basic form validation
function setupFormValidation() {
    const forms = document.querySelectorAll('.needs-validation');
    
    if (forms.length > 0) {
        Array.from(forms).forEach(form => {
            form.addEventListener('submit', event => {
                if (!form.checkValidity()) {
                    event.preventDefault();
                    event.stopPropagation();
                }
                
                form.classList.add('was-validated');
            }, false);
        });
    }
}