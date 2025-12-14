document.addEventListener("DOMContentLoaded", () => {

    const API_URL = "https://localhost:44331/api/Propietario";
    const registrar = document.getElementById("registrarPropietario");

    registrar.addEventListener("click", async () => {

        // Obtener valores del formulario
        const id = document.getElementById("idPropietario").value.trim();
        const nombre = document.getElementById("nombre").value.trim();
        const apellido = document.getElementById("apellido").value.trim();
        const correo = document.getElementById("correo").value.trim();
        const telefono = document.getElementById("telefono").value.trim();

        // Validar campos vacíos
        if (!id || !nombre || !apellido || !correo || !telefono) {
            alert("Por favor, complete todos los campos.");
            return;
        }

        // Validar ID numérico
        if (isNaN(id)) {
            alert("El ID debe ser un número válido.");
            return;
        }

        // Crear objeto para enviar al API
        const data = {
            IdPropietario: parseInt(id, 10),
            Nombre: nombre,
            Apellido: apellido,
            Correo: correo,
            Telefono: telefono
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
                alert("Propietario registrado correctamente.");
                window.location.href = "propietarios.html";
            } else {
                const errorText = await response.text();
                console.error("Error:", errorText);
                alert(`Error al registrar propietario. Código: ${response.status}`);
            }

        } catch (error) {
            console.error("Error de red:", error);
            alert("No se pudo conectar con la API.");
        }
    });

});

