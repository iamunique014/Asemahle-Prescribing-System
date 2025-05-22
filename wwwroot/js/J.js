
//localStorage.removeItem('customerProfile'); // Clears saved data on every page load
    const form = document.getElementById('profileForm');
    const nameInput = document.getElementById('name');
    const emailInput = document.getElementById('email');
    const phoneInput = document.getElementById('phone');
    const addressInput = document.getElementById('address');
const notesInput = document.getElementById('allergy-select');
    const title = document.getElementById('profileTitle');
    const button = document.getElementById('submitButton');

    // Load saved profile if it exists
    const savedProfile = JSON.parse(localStorage.getItem('customerProfile'));
    if (savedProfile) {
        nameInput.value = savedProfile.name;
    emailInput.value = savedProfile.email;
    phoneInput.value = savedProfile.phone;
    addressInput.value = savedProfile.address;
    notesInput.value = savedProfile.notes;
    title.textContent = 'Update Your Profile';
    button.textContent = 'Update Profile';
        }

    form.addEventListener('submit', function (e) {
        e.preventDefault();
    const profileData = {
        name: nameInput.value,
    email: emailInput.value,
    phone: phoneInput.value,
    address: addressInput.value,
    notes: notesInput.value
          };

    localStorage.setItem('customerProfile', JSON.stringify(profileData));
    alert('Profile saved!');
    title.textContent = 'Update Your Profile';
    button.textContent = 'Update Profile';
        });

