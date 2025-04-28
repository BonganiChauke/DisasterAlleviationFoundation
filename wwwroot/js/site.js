// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Ensure the DOM is fully loaded before attaching event listeners
'use strict';
document.addEventListener('DOMContentLoaded', function () {

    //sign up form
    //form submit event
    document.getElementById("signupForm").addEventListener("submit", function (event) {

        // Prevent form submission
        //event.preventDefault();

        // Clear previous error messages
        clearErrorMessages();

        // Get form values
        const firstName = document.getElementById("firstName").value.trim();
        const lastName = document.getElementById("lastName").value.trim();
        const email = document.getElementById("email").value.trim();
        const password = document.getElementById("password").value.trim();
        const confirmPassword = document.getElementById("confirmPassword").value.trim();

        let passwordError = validatePasswordComplexity(password);
        let nameError = validateUserNames(firstName);
        let lastNameError = validateUserNames(lastName);

        // Validation flags
        let isValid = true;

        // First Name Validation
        if (nameError !== "") {
            isValid = false;
            displayErrorMessage("firstNameError", nameError);
        }

        // Last Name Validation
        if (lastNameError !== "") {
            isValid = false;
            displayErrorMessage("lastNameError", lastNameError);
        }

        // Email Validation
        if (email === "") {
            isValid = false;
            displayErrorMessage("emailError", "Email is required.");
        } else if (!validateEmail(email)) {
            isValid = false;
            displayErrorMessage("emailError", "Invalid email format.");
        }

        // Password Validation
        if (passwordError !== "") {
            isValid = false;
            displayErrorMessage("passwordError", passwordError);
        } else if (password.length < 8) {
            isValid = false;
            displayErrorMessage("passwordError", "Password must be at least 8 characters long.");
        }

        // Confirm Password Validation
        if (confirmPassword === "") {
            isValid = false;
            displayErrorMessage("confirmPasswordError", "Confirm password is required.");
        } else if (password !== confirmPassword) {
            isValid = false;
            displayErrorMessage("confirmPasswordError", "Passwords do not match.");
        }

        // If form is valid, proceed with submission (currently just logs success)
        if (isValid) {
            alert("Form submitted successfully!"); // Placeholder for actual form submission
        } else {
            //prevent default
            event.preventDefault();
        }
    });//end of sign up


    //function for login 
    document.getElementById("signInForm").addEventListener("submit", function (event) {

        //get values from the form
        const email = document.getElementById("email").value.trim();
        const password = document.getElementById("password").value.trim();

        // Validation flags
        let isValid = true;

        //
        if (email === "") {
            isValid = false;
            email.focus();
        }
        if (password === "") {

            isValid = false;
            password.focus();
        }

        // If form is valid, proceed with submission (currently just logs success)
        if (!isValid) {
            //prevent default
            event.preventDefault();
        }

    });//end


    //---------------------------------------------
    // Names validation function
    function validateUserNames(name) {
        const hasNumber = /[0-9]/.test(name); // Number
        const hasSpecialChar = /[!@@#\$%\^\&*\)\(+=._-]/.test(name); // Special character

        let errorMessage = "";
        if (name === "") errorMessage += "Name is required.\n";
        if (name.length < 3) errorMessage += "Name cannot be less than 3 letters.\n";
        if (hasNumber) errorMessage += "Name cannot contain numbers.\n";
        if (hasSpecialChar) errorMessage += "Name cannot contain special characters.\n";

        return errorMessage;
    }

    // Password validation function
    function validatePasswordComplexity(password) {
        const hasUppercase = /[A-Z]/.test(password); // Uppercase letter
        const hasLowercase = /[a-z]/.test(password); // Lowercase letter
        const hasNumber = /[0-9]/.test(password); // Number
        const hasSpecialChar = /[!@#\$%\^\&*\)\(+=._-]/.test(password); // Special character

        let errorMessage = "";
        if (!password === "") errorMessage += "Password Required.\n";
        if (password.lenght < 8) errorMessage += "Password must be 8 characters.\n";
        if (!hasUppercase) errorMessage += "Password must contain at least one uppercase letter.\n";
        if (!hasLowercase) errorMessage += "Password must contain at least one lowercase letter.\n";
        if (!hasNumber) errorMessage += "Password must contain at least one number.\n";
        if (!hasSpecialChar) errorMessage += "Password must contain at least one special character.\n";

        return errorMessage;
    }

    // Function to validate email format using regex
    function validateEmail(email) {
        const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return re.test(email);
    }

    // Function to display error message
    function displayErrorMessage(elementId, message) {
        document.getElementById(elementId).textContent = message;
    }

    // Function to clear all error messages
    function clearErrorMessages() {
        let errorMessages = document.querySelectorAll(".error-message");
        errorMessages.forEach(function (errorMessage) {
            errorMessage.textContent = "";
        });
    }


});//end of DOMContentLoaded

// Sidebar toggle functionality for small screens
document
    .getElementById("sidebarCollapse")
    .addEventListener("click", function () {
        const sidebar = document.getElementById("sidebar");
        sidebar.classList.toggle("active");
    });

// Close button functionality inside the sidebar
document
    .getElementById("closeSidebar")
    .addEventListener("click", function () {
        const sidebar = document.getElementById("sidebar");
        sidebar.classList.toggle("active"); // Toggle the active class to close the sidebar
    });

// Ensure the sidebar is visible on large screens and hidden on small screens
window.addEventListener("resize", function () {
    const sidebar = document.getElementById("sidebar");
    const toggleButton = document.getElementById("sidebarCollapse");
    if (window.innerWidth > 768) {
        sidebar.classList.remove("active"); // Show sidebar on large screens
        toggleButton.style.display = "none"; // Hide toggle button on large screens
    } else {
        sidebar.classList.add("active"); // Hide sidebar on small screens
        toggleButton.style.display = "inline-block"; // Show toggle button on small screens
    }
});

// Initial check to set the right state when the page loads
window.addEventListener("load", function () {
    const sidebar = document.getElementById("sidebar");
    const toggleButton = document.getElementById("sidebarCollapse");
    if (window.innerWidth > 768) {
        sidebar.classList.remove("active"); // Show sidebar on large screens
        toggleButton.style.display = "none"; // Hide toggle button on large screens
    } else {
        sidebar.classList.add("active"); // Hide sidebar on small screens
        toggleButton.style.display = "inline-block"; // Show toggle button on small screens
    }
});

function handleDonationTypeChange() {
    const donationType = document.getElementById('donationType').value;

    // Hide all fields initially
    document.getElementById('moneyFields').style.display = 'none';
    document.getElementById('foodFields').style.display = 'none';
    document.getElementById('clothesFields').style.display = 'none';

    // Show relevant fields based on selection
    if (donationType === 'Money') {
        document.getElementById('moneyFields').style.display = 'block';
    } else if (donationType === 'Food') {
        document.getElementById('foodFields').style.display = 'block';
    } else if (donationType === 'Clothes') {
        document.getElementById('clothesFields').style.display = 'block';
    }
}