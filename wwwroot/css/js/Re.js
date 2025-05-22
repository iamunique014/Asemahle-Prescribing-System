const prescriptions = [
    {
        medication: "Amoxicillin",
        date: "2025-04-15",
        doctor: "Dr. Moyo",
        totalRepeats: 3,
        repeatsLeft: 2,
        lastDispensed: "2025-05-10",
        nextEligible: "2025-06-10"
    },
    {
        medication: "Lisinopril",
        date: "2025-03-01",
        doctor: "Dr. Ndlovu",
        totalRepeats: 5,
        repeatsLeft: 0,
        lastDispensed: "2025-05-01",
        nextEligible: "-"
    },
    {
        medication: "Metformin",
        date: "2025-02-20",
        doctor: "Dr. Khumalo",
        totalRepeats: 4,
        repeatsLeft: 1,
        lastDispensed: "2025-04-20",
        nextEligible: "2025-05-20"
    }
];

const tableBody = document.querySelector("#repeatsTable tbody");
const modal = document.getElementById("confirmModal");
const modalText = document.getElementById("modalText");
let selectedMedication = null;

function renderTable(filter = "") {
    const tableBody = document.querySelector("#repeatsTable tbody");
    tableBody.innerHTML = ""; // Clear existing rows

    prescriptions
        .filter(p => p.medication.toLowerCase().includes(filter.toLowerCase()))
        .forEach((prescription, index) => {
            const row = document.createElement("tr");
            row.innerHTML = `
        <td>${prescription.medication}</td>
        <td>${prescription.date}</td>
        <td>${prescription.doctor}</td>
        <td>${prescription.totalRepeats}</td>
        <td>${prescription.repeatsLeft}</td>
        <td>${prescription.lastDispensed}</td>
        <td>${prescription.nextEligible}</td>
        <td>
          <button onclick="openModal(${index})" ${prescription.repeatsLeft === 0 ? 'disabled' : ''}>
            Request Repeat
          </button>
        </td>
      `;
            tableBody.appendChild(row);
        });
}

function searchRepeats() {
    const input = document.getElementById("searchInput").value;
    renderTable(input);
}


function openModal(index) {
    selectedMedication = prescriptions[index];
    modalText.textContent = `Request a repeat for ${selectedMedication.medication}?`;
    modal.style.display = "block";
}

function closeModal() {
    modal.style.display = "none";
}

function confirmRequest() {
    alert(`Repeat requested for ${selectedMedication.medication}`);
    closeModal();
}

window.onload = renderTable;
