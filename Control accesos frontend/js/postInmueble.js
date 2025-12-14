document.addEventListener("DOMContentLoaded", () => {

    const API_URL = "https://localhost:44331/api/Inmueble";
    const registrar = document.getElementById("registrarInmueble");

    if (!registrar) {
        console.error("No se encontró el botón #registrarInmueble");
        return;
    }

    registrar.addEventListener("click", async () => {

        // Obtener valores del formulario
        const id          = document.getElementById("idInmueble").value.trim();
        const numeroTorre = document.getElementById("numeroTorre").value.trim();
        const pisoTexto   = document.getElementById("piso").value.trim();
        const apartamento = document.getElementById("apartamento").value.trim();
        const idPropTexto = document.getElementById("idPropietario").value.trim();

        // Validar campos vacíos
        if (!id || !numeroTorre || !pisoTexto || !apartamento || !idPropTexto) {
            alert("Por favor, complete todos los campos.");
            return;
        }

        // Validar ID numérico
        if (isNaN(id)) {
            alert("El ID del inmueble debe ser un número válido.");
            return;
        }

        // Convertir a número piso e ID propietario
        const piso = parseInt(pisoTexto, 10);
        const idPropietario = parseInt(idPropTexto, 10);

        if (isNaN(piso) || isNaN(idPropietario)) {
            alert("Piso e ID Propietario deben ser números válidos.");
            return;
        }

        // Objeto para enviar al API
        const data = {
            IdInmueble: parseInt(id, 10),
            NumeroTorre: numeroTorre,
            Piso: piso,
            Apartamento: apartamento,
            IdPropietario: idPropietario
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
                alert("Inmueble registrado correctamente.");
                
                // 👇 MUY IMPORTANTE: aquí debe ir inmueble.html
                window.location.href = "inmueble.html";
            } else {
                const errorText = await response.text();
                console.error("Error:", errorText);
                alert(`Error al registrar inmueble. Código: ${response.status}`);
            }

        } catch (error) {
            console.error("Error de red:", error);
            alert("No se pudo conectar con la API.");
        }
    });

});


