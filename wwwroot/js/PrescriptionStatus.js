
  // Sample data
    const prescriptions = [
    {id: 'RX001', name: 'Amoxicillin', status: 'Submitted', amount: 'R150' },
    {id: 'RX002', name: 'Ibuprofen', status: 'Processing', amount: 'R90' },
    {id: 'RX003', name: 'Paracetamol', status: 'Ready', amount: 'R100' },
    {id: 'RX004', name: 'Lisinopril', status: 'Collected', amount: 'R200' }
    ];

    const tableBody = document.querySelector("#prescriptionTable tbody");

    function renderTable() {
        tableBody.innerHTML = ''; // Clear table
    prescriptions.forEach((prescription, index) => {
      const row = document.createElement("tr");

    row.innerHTML = `
    <td>${prescription.id}</td>
    <td>${prescription.name}</td>
    <td>${prescription.status}</td>
    <td>${prescription.amount}</td>
    <td>
        ${prescription.status === "Collected" ?
            `<button class="btn-remove" onclick="removePrescription(${index})">Remove</button>` :
            ''}
    </td>
    `;

    tableBody.appendChild(row);
    });
  }

    function removePrescription(index) {
    if (confirm("Are you sure you want to remove this collected prescription?")) {
        prescriptions.splice(index, 1);
    renderTable();
    }
  }

    // Initialize table
    renderTable();