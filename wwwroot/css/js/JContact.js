document.getElementById("contactForm").addEventListener("submit", function (event) {
    event.preventDefault();

    // Get input values
    const name = document.getElementById("name").value.trim();
    const email = document.getElementById("email").value.trim();
    const subject = document.getElementById("subject").value.trim();
    const message = document.getElementById("message").value.trim();

    // Validate inputs
    if (!name || !email || !subject || !message) {
        alert("Please fill out all fields.");
        return;
    }

    // Validate email format
    const emailPattern = /^[^ ]+@[^ ]+\.[a-z]{2,3}$/;
    if (!emailPattern.test(email)) {
        alert("Please enter a valid email address.");
        return;
    }

    // Show success popup
    document.getElementById("popupMessage").style.display = "block";

    // Reset the form
    document.getElementById("contactForm").reset();

    // Hide popup after 4 seconds
    setTimeout(() => {
        document.getElementById("popupMessage").style.display = "none";
    }, 4000);
});
