document.addEventListener("DOMContentLoaded", () => {

    const API_URL = "https://localhost:44331/api/Personal";
    const registrar = document.getElementById("btnRegistrarPersonal"); // id del botón en el HTML

    console.log("Botón registrar:", registrar);

    if (!registrar) {
        console.error("No se encontró el botón de registrar personal");
        return;
    }

    registrar.addEventListener("click", async () => {

        // Obtener valores del formulario
        const idPersonalTexto    = document.getElementById("idPersonal").value.trim();
        const nombre             = document.getElementById("nombre").value.trim();
        const apellido           = document.getElementById("apellido").value.trim();
        const documentoIdentidad = document.getElementById("documentoIdentidad").value.trim();
        const tipoPersona        = document.getElementById("tipoPersona").value.trim();
        const idInmuebleTexto    = document.getElementById("idInmueble").value.trim();

        // Validar campos vacíos
        if (
            !idPersonalTexto ||
            !nombre ||
            !apellido ||
            !documentoIdentidad ||
            !tipoPersona ||
            !idInmuebleTexto
        ) {
            alert("Por favor, complete todos los campos.");
            return;
        }

        // Validar ID Personal numérico
        if (isNaN(idPersonalTexto)) {
            alert("El ID del personal debe ser un número válido.");
            return;
        }

        // Validar ID Inmueble numérico
        const idInmueble = parseInt(idInmuebleTexto, 10);
        if (isNaN(idInmueble)) {
            alert("El ID Inmueble debe ser un número válido.");
            return;
        }

        // Crear objeto para enviar al API
        const data = {
            IdPersonal: parseInt(idPersonalTexto, 10),
            Nombre: nombre,
            Apellido: apellido,
            DocumentoIdentidad: documentoIdentidad,
            TipoPersona: tipoPersona,
            IdInmueble: idInmueble
        };

        try {
            const response = await fetch(API_URL, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(data)
            });

            if (response.ok) {
                alert("Personal registrado correctamente.");
                // Página de listado
                window.location.href = "personal.html";
            } else {
                const errorText = await response.text();
                console.error("Error:", errorText);
                alert(`Error al registrar personal. Código: ${response.status}`);
            }

        } catch (error) {
            console.error("Error de red:", error);
            alert("No se pudo conectar con la API.");
        }
    });

});

