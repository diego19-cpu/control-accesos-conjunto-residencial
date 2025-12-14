document.addEventListener("DOMContentLoaded", () => {

    const API_URL = "https://localhost:44331/api/Acceso";
    const registrar = document.getElementById("registrarAcceso");

    if (!registrar) {
        console.error("No se encontró el botón registrarAcceso");
        return;
    }

    registrar.addEventListener("click", async () => {

        // Función para obtener valores de inputs con verificación
        const getValue = (id) => {
            const el = document.getElementById(id);
            if (!el) {
                console.error("No se encontró el elemento con id:", id);
                throw new Error("Elemento no encontrado: " + id);
            }
            return el.value.trim();
        };

        // Obtener datos del formulario
        const idAccesoTexto    = getValue("idAcceso");
        const idVigilanteTexto = getValue("idVigilante");
        const idPersonalTexto  = getValue("idPersonal");
        const ingreso          = getValue("momentoIngreso");     // datetime-local
        const salida           = getValue("momentoSalida");      // opcional
        const visitasTexto     = getValue("numeroVisitas");      // opcional
        const autorizadoPor    = getValue("autorizadoPor");      // opcional
        const placa            = getValue("numeroPlacaVehiculo");// opcional

        // Validar campos obligatorios
        if (!idAccesoTexto || !idVigilanteTexto || !idPersonalTexto || !ingreso) {
            alert("IdAcceso, IdVigilante, IdPersonal y MomentoIngreso son obligatorios.");
            return;
        }

        // Validar IDs numéricos
        if (isNaN(idAccesoTexto) || isNaN(idVigilanteTexto) || isNaN(idPersonalTexto)) {
            alert("Los IDs deben ser números válidos.");
            return;
        }

        const numeroVisitas = visitasTexto ? parseInt(visitasTexto, 10) : null;

        // Construcción del objeto final
        const data = {
            IdAcceso:            parseInt(idAccesoTexto, 10),
            IdVigilante:         parseInt(idVigilanteTexto, 10),
            IdPersonal:          parseInt(idPersonalTexto, 10),
            MomentoIngreso:      ingreso,
            MomentoSalida:       salida || null,
            NumeroVisitas:       numeroVisitas,
            AutorizadoPor:       autorizadoPor || null,
            NumeroPlacaVehiculo: placa || null
        };

        try {
            const response = await fetch(API_URL, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(data)
            });

            if (response.ok) {
                alert("Acceso registrado correctamente.");
                history.back(); // o redirigir a accesos.html
            } else {
                const errorText = await response.text();
                console.error("Error:", errorText);
                alert(`Error al registrar acceso. Código: ${response.status}`);
            }

        } catch (error) {
            console.error("Error de red:", error);
            alert("No se pudo conectar con la API.");
        }
    });

});


