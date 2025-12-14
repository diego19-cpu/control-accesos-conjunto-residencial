document.addEventListener("DOMContentLoaded", () => {
    const btnEditar = document.getElementById("btnEditar");
    const API_URL = "https://localhost:44331/api/Administrador";

    // Campos del formulario
    const idInput  = document.getElementById("idAdministrador");
    const nombre   = document.getElementById("nombre");
    const correo   = document.getElementById("correo");
    const cargo    = document.getElementById("cargo");

    // --- Manejar el clic para actualizar (PUT) ---
    btnEditar.addEventListener("click", () => {

        const id = idInput.value.trim();

        if (!id || isNaN(id)) {
            alert("Por favor, ingrese un ID de administrador válido.");
            return;
        }

        if (!nombre.value.trim() || !correo.value.trim() || !cargo.value.trim()) {
            alert("Por favor, complete todos los campos.");
            return;
        }

        const data = {
            IdAdministrador: parseInt(id, 10),
            Nombre: nombre.value.trim(),
            Correo: correo.value.trim(),
            Cargo: cargo.value.trim()
        };

        fetch(`${API_URL}/${id}`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(data),
        })
          .then((response) => {
            if (response.ok) {
              console.log("Datos actualizados correctamente");
              alert("Administrador actualizado exitosamente!");
              window.location.href = "index.html";
            } else {
              console.error("Error al enviar la solicitud:", response.status);
              alert(`Error al actualizar. Código: ${response.status}`);
            }
          })
          .catch((error) => {
            console.error("Error al enviar la solicitud:", error);
          });
    }); 
});

