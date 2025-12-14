document.addEventListener("DOMContentLoaded", () => {
    // 1. Definición de la URL de la API (verificada con Postman)
    const API_URL = "https://localhost:44331/api/Administrador"; 
    
    // 2. Elemento del botón que inicia la acción
    const registrar = document.getElementById("registrar");

    // 3. Adjuntar la función al evento click
    registrar.addEventListener("click", async () => {
        // Obtener valores de los campos del formulario
        const nombre = document.getElementById("nombre").value.trim();
        const correo = document.getElementById("correo").value.trim();
        const cargo  = document.getElementById("cargo").value.trim();

        // Verificar si algún campo está vacío
        if (!nombre || !correo || !cargo) {
            alert("Por favor, complete todos los campos.");
            return;
        }

        try {
            // 🔹 1) Pedir la lista actual de administradores para calcular un ID nuevo
            const respLista = await fetch(API_URL);
            if (!respLista.ok) {
                console.error("No se pudo obtener la lista de administradores. Código:", respLista.status);
                alert("No se pudo obtener la lista de administradores para calcular el ID.");
                return;
            }

            const lista = await respLista.json();

            // 🔹 2) Calcular el siguiente ID disponible
            let nuevoId = 1;
            if (Array.isArray(lista) && lista.length > 0) {
                const maxId = Math.max(...lista.map(a => a.IdAdministrador));
                nuevoId = maxId + 1;
            }

            // 🔹 3) Crear el objeto de datos que se enviará a la API
            const data = {
                IdAdministrador: nuevoId,  // ID calculado automáticamente
                Nombre: nombre,
                Correo: correo,
                Cargo: cargo
            };

            // 🔹 4) Realizar la solicitud POST a la API
            const response = await fetch(API_URL, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(data)
            });

            if (response.ok) {
                console.log("Administrador registrado correctamente");
                alert("Registro exitoso!");
                window.location.href = "index.html"; 
            } else {
                const errorText = await response.text();
                console.error(`Error al registrar. Código: ${response.status}`, errorText);
                alert(`Error al registrar. El servidor devolvió el código: ${response.status}. Revisa la consola del servidor (.NET) para el error detallado.`);
            }
        } catch (error) {
            // Este catch atrapará errores de red, DNS o CORS
            console.error("Error de conexión, CORS, o red. La solicitud falló:", error);
            alert("Fallo la conexión con la API. Revisa la consola para errores de red o CORS.");
        }
    });
});
