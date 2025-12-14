document.addEventListener("DOMContentLoaded", () => {

    const btnEditar = document.getElementById("btnEditarPropietario");
    const API_URL = "https://localhost:44331/api/Propietario";

    // Campos del formulario
    const idInput  = document.getElementById("idPropietario");
    const nombre   = document.getElementById("nombre");
    const apellido = document.getElementById("apellido");
    const correo   = document.getElementById("correo");
    const telefono = document.getElementById("telefono");

    // --- BOTÓN ACTUALIZAR ---
    btnEditar.addEventListener("click", () => {

        const id = idInput.value.trim();

        // Validación del ID
        if (!id || isNaN(id)) {
            alert("Por favor, ingrese un ID de propietario válido.");
            return;
        }

        // Validación de campos obligatorios
        if (!nombre.value.trim() || !apellido.value.trim() || !correo.value.trim() || !telefono.value.trim()) {
            alert("Por favor, complete todos los campos.");
            return;
        }

        // Objeto para enviar al API
        const data = {
            IdPropietario: parseInt(id, 10),
            Nombre: nombre.value.trim(),
            Apellido: apellido.value.trim(),
            Correo: correo.value.trim(),
            Telefono: telefono.value.trim()
        };

        // Llamada PUT a la API
        fetch(`${API_URL}/${id}`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(data),
        })
        .then((response) => {
            if (response.ok) {
                alert("Propietario actualizado correctamente.");
                window.location.href = "propietarios.html";
            } else {
                console.error("Error al actualizar:", response.status);
                alert(`Error al actualizar propietario. Código: ${response.status}`);
            }
        })
        .catch((error) => {
            console.error("Error al realizar la solicitud PUT:", error);
            alert("No se pudo conectar con la API.");
        });

    });

});
