const medications = [
    {
        name: "Amoxicillin",
        doctor: "Dr. Moyo",
        totalRepeats: 3,
        repeatsLeft: 2,
        lastDispensed: "2025-05-10",
        nextEligible: "2025-06-10"
    },
    {
        name: "Lisinopril",
        doctor: "Dr. Ndlovu",
        totalRepeats: 5,
        repeatsLeft: 0,
        lastDispensed: "2025-05-01",
        nextEligible: "-"
    },
    {
        name: "Metformin",
        doctor: "Dr. Khumalo",
        totalRepeats: 4,
        repeatsLeft: 1,
        lastDispensed: "2025-05-05",
        nextEligible: "2025-06-05"
    }
];

let selectedIndex = null;

function renderTable() {
    const tbody = document.querySelector("#repeatsTable tbody");
    tbody.innerHTML = "";

    medications.forEach((med, index) => {
        const row = document.createElement("tr");
        row.innerHTML = `
      <td>${med.name}</td>
      <td>${med.doctor}</td>
      <td>${med.totalRepeats}</td>
      <td>${med.repeatsLeft}</td>
      <td>${med.lastDispensed}</td>
      <td>${med.nextEligible}</td>
      <td>
        <button onclick="openModal(${index})" ${med.repeatsLeft <= 0 ? "disabled" : ""}>
          Request Repeat
        </button>
      </td>
    `;
        tbody.appendChild(row);
    });
}

function openModal(index) {
    selectedIndex = index;
    document.getElementById("modalText").textContent =
        `Do you want to request a repeat for ${medications[index].name}?`;
    document.getElementById("modal").style.display = "flex";
}

function closeModal() {
    document.getElementById("modal").style.display = "none";
}

function confirmRequest() {
    alert(`Repeat requested for ${medications[selectedIndex].name}`);
    if (medications[selectedIndex].repeatsLeft > 0) {
        medications[selectedIndex].repeatsLeft--;
    }
    closeModal();
    renderTable();
}

window.onload = renderTable;
