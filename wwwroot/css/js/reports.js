
  // Sample data for demonstration (replace with actual data from backend)
  const prescriptions = [
    { id: 'RX001', patient: 'John Doe', doctor: 'Dr. Smith', medication: 'Amoxicillin', status: 'Dispensed', amount: 'R150', date: '2025-05-10' },
    { id: 'RX002', patient: 'Jane Smith', doctor: 'Dr. Lee', medication: 'Ibuprofen', status: 'Dispensed', amount: 'R120', date: '2025-05-11' },
    { id: 'RX003', patient: 'Emily Davis', doctor: 'Dr. Brown', medication: 'Paracetamol', status: 'Dispensed', amount: 'R80', date: '2025-05-12' },
    { id: 'RX004', patient: 'Michael Johnson', doctor: 'Dr. Green', medication: 'Lisinopril', status: 'Pending', amount: 'R200', date: '2025-05-13' },
  ];

  function renderTable() {
    const tableBody = document.querySelector("#prescriptionTable tbody");
    tableBody.innerHTML = ''; // Clear the table before adding new data

    prescriptions.forEach(prescription => {
      const row = document.createElement("tr");

      row.innerHTML = `
        <td>${prescription.id}</td>
        <td>${prescription.patient}</td>
        <td>${prescription.doctor}</td>
        <td>${prescription.medication}</td>
        <td>${prescription.status}</td>
        <td>${prescription.amount}</td>
        <td>${prescription.status === 'Dispensed' ? `<button class="action-btn" onclick="removePrescription('${prescription.id}')">Remove</button>` : ''}</td>
      `;

      tableBody.appendChild(row);
    });
  }

  function generateReport() {
    const { jsPDF } = window.jspdf;
    const doc = new jsPDF();

    const start = document.getElementById('startDate').value;
    const end = document.getElementById('endDate').value;
    const groupBy = document.getElementById('groupBy').value;

    if (!start || !end) {
      alert('Please select a valid date range.');
      return;
    }

    // Filter by date range
    const filteredPrescriptions = prescriptions.filter(p => p.date >= start && p.date <= end);

    // Grouping data based on user choice
    const groupedData = {};
    filteredPrescriptions.forEach(prescription => {
      const groupKey = prescription[groupBy];
      if (!groupedData[groupKey]) groupedData[groupKey] = [];
      groupedData[groupKey].push(prescription);
    });

    // Set up the report title
    doc.text(`Prescription Report from ${start} to ${end}`, 10, 10);
    doc.text(`Grouped by: ${groupBy.charAt(0).toUpperCase() + groupBy.slice(1)}`, 10, 20);

    let yPosition = 30;
    for (const group in groupedData) {
      doc.text(`${group}:`, 10, yPosition);
      yPosition += 6;

      groupedData[group].forEach((prescription, idx) => {
        doc.text(`${prescription.patient} - ${prescription.medication} (R${prescription.amount})`, 10, yPosition);
        yPosition += 6;
      });
      yPosition += 4;
    }

    doc.save(`Prescription_Report_${start}_to_${end}.pdf`);
  }

  // Initialize table on page load
  renderTable();

  function removePrescription(id) {
    const index = prescriptions.findIndex(p => p.id === id);
    if (index > -1) {
      prescriptions.splice(index, 1);
      renderTable();
    }
  }
