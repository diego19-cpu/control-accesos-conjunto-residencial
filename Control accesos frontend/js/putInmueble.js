document.addEventListener("DOMContentLoaded", () => {

    const btnEditar = document.getElementById("btnEditarInmueble");
    const API_URL = "https://localhost:44331/api/Inmueble";

    // Campos del formulario
    const idInput       = document.getElementById("idInmueble");
    const numeroTorre   = document.getElementById("numeroTorre");
    const pisoInput     = document.getElementById("piso");
    const apartamento   = document.getElementById("apartamento");
    const idPropInput   = document.getElementById("idPropietario");

    // --- BOTÓN ACTUALIZAR ---
    btnEditar.addEventListener("click", () => {

        const id = idInput.value.trim();

        // Validación del ID
        if (!id || isNaN(id)) {
            alert("Por favor, ingrese un ID de inmueble válido.");
            return;
        }

        // Validación de campos obligatorios
        if (
            !numeroTorre.value.trim() ||
            !pisoInput.value.trim() ||
            !apartamento.value.trim() ||
            !idPropInput.value.trim()
        ) {
            alert("Por favor, complete todos los campos.");
            return;
        }

        // Convertir numéricos (opcional pero recomendable)
        const piso = parseInt(pisoInput.value.trim(), 10);
        const idPropietario = parseInt(idPropInput.value.trim(), 10);

        if (isNaN(piso) || isNaN(idPropietario)) {
            alert("Piso e ID Propietario deben ser números válidos.");
            return;
        }

        // Objeto para enviar al API
        const data = {
            IdInmueble: parseInt(id, 10),
            NumeroTorre: numeroTorre.value.trim(),
            Piso: piso,
            Apartamento: apartamento.value.trim(),
            IdPropietario: idPropietario
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
                alert("Inmueble actualizado correctamente.");
                // ⬇️ AQUÍ ESTABA EL PROBLEMA
                window.location.href = "inmueble.html"; // nombre real de tu listado
            } else {
                console.error("Error al actualizar:", response.status);
                alert(`Error al actualizar inmueble. Código: ${response.status}`);
            }
        })
        .catch((error) => {
            console.error("Error al realizar la solicitud PUT:", error);
            alert("No se pudo conectar con la API.");
        });

    });

});

