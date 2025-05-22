let allergies = [];

function renderTable() {
    const tbody = document.querySelector("#allergyTable tbody");
    tbody.innerHTML = "";

    allergies.forEach((allergy, index) => {
        const row = document.createElement("tr");
        row.innerHTML = `
      <td>${allergy.ingredient}</td>
      <td>${allergy.severity}</td>
      <td>${allergy.notes}</td>
      <td>
        <button onclick="editAllergy(${index})">Edit</button>
        <button onclick="deleteAllergy(${index})">Delete</button>
      </td>
    `;
        tbody.appendChild(row);
    });
}

function resetForm() {
    document.getElementById("ingredient").value = "";
    document.getElementById("severity").value = "Mild";
    document.getElementById("notes").value = "";
    document.getElementById("editIndex").value = "";
}

document.getElementById("allergyForm").addEventListener("submit", function (e) {
    e.preventDefault();

    const ingredient = document.getElementById("ingredient").value.trim();
    const severity = document.getElementById("severity").value;
    const notes = document.getElementById("notes").value.trim();
    const editIndex = document.getElementById("editIndex").value;

    const allergy = { ingredient, severity, notes };

    if (editIndex === "") {
        allergies.push(allergy); // Add new
    } else {
        allergies[editIndex] = allergy; // Update existing
    }

    renderTable();
    resetForm();
});

function editAllergy(index) {
    const a = allergies[index];
    document.getElementById("ingredient").value = a.ingredient;
    document.getElementById("severity").value = a.severity;
    document.getElementById("notes").value = a.notes;
    document.getElementById("editIndex").value = index;
}

function deleteAllergy(index) {
    if (confirm("Are you sure you want to delete this allergy?")) {
        allergies.splice(index, 1);
        renderTable();
    }
}

window.onload = renderTable;
