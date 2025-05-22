
  // Show/hide fields based on repeat type
  document.querySelectorAll('input[name=\"repeatType\"]').forEach(radio => {
    radio.addEventListener('change', () => {
      const type = document.querySelector('input[name=\"repeatType\"]:checked').value;
      document.getElementById('quantityGroup').style.display = type === 'quantity' ? 'block' : 'none';
      document.getElementById('dateGroup').style.display = type === 'date' ? 'block' : 'none';
    });
  });

  function submitRepeat() {
    const prescription = document.getElementById('prescriptionSelect').value;
    const repeatType = document.querySelector('input[name=\"repeatType\"]:checked').value;
    const quantity = document.getElementById('quantity').value;
    const packDate = document.getElementById('packDate').value;

    if (!prescription) {
      alert('Please select a prescription.');
      return;
    }

    if (repeatType === 'quantity' && (!quantity || quantity <= 0)) {
      alert('Please enter a valid quantity.');
      return;
    }

    if (repeatType === 'date' && !packDate) {
      alert('Please select a packing date.');
      return;
    }

    // In a real app, you'd now send this to the server...

    // Show confirmation
    document.getElementById('confirmationMessage').style.display = 'block';

    // Reset form after short delay
    setTimeout(() => {
      document.getElementById('prescriptionSelect').value = '';
      document.getElementById('quantity').value = '';
      document.getElementById('packDate').value = '';
      document.getElementById('confirmationMessage').style.display = 'none';
    }, 3000);
  }
