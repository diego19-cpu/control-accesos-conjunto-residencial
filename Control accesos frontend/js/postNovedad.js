document.addEventListener("DOMContentLoaded", () => {
    // URL correcta (usa singular y minúscula)
    const API_URL = "https://localhost:44331/api/Novedad";
    
    // Botón registrar
    const registrar = document.getElementById("registrarNovedad");

    if (!registrar) {
        console.error("No se encontró el botón registrarNovedad");
        return;
    }

    registrar.addEventListener("click", async () => {

        // Obtener valores
        const idVigilante     = document.getElementById("idVigilante").value.trim();
        const idAdministrador = document.getElementById("idAdministrador").value.trim();
        const idPersonal      = document.getElementById("idPersonal").value.trim();
        const descripcion     = document.getElementById("descripcion").value.trim();
        const momento         = document.getElementById("momentoNovedad").value.trim();

        // Validación de obligatorios
        if (!idVigilante || !descripcion || !momento) {
            alert("Complete los campos obligatorios: Vigilante, Descripción y Momento.");
            return;
        }

        // Validación de numéricos obligatorios
        if (isNaN(idVigilante)) {
            alert("El ID del vigilante debe ser numérico.");
            return;
        }

        // Validación opcionales (nulos)
        let idAdminNum = null;
        if (idAdministrador !== "") {
            if (isNaN(idAdministrador)) {
                alert("El ID del administrador debe ser numérico.");
                return;
            }
            idAdminNum = parseInt(idAdministrador, 10);
        }

        let idPersonalNum = null;
        if (idPersonal !== "") {
            if (isNaN(idPersonal)) {
                alert("El ID del personal debe ser numérico.");
                return;
            }
            idPersonalNum = parseInt(idPersonal, 10);
        }

        // Corregir formato "yyyy-MM-ddTHH:mm" → agregar segundos
        let momentoFinal = momento;
        if (momentoFinal.length === 16) {
            momentoFinal += ":00";
        }

        try {
            // 1. Obtener lista actual de novedades
            const respLista = await fetch(API_URL);
            if (!respLista.ok) {
                alert("No se pudo obtener la lista de novedades para calcular el ID.");
                return;
            }

            const lista = await respLista.json();

            // 2. Calcular nuevo ID
            let nuevoId = 1;
            if (Array.isArray(lista) && lista.length > 0) {
                const maxId = Math.max(...lista.map(n => n.IdNovedad));
                nuevoId = maxId + 1;
            }

            // 3. Crear objeto datos
            const data = {
                IdNovedad: nuevoId,
                IdVigilante: parseInt(idVigilante, 10),
                IdAdministrador: idAdminNum,
                IdPersonal: idPersonalNum,
                Descripcion: descripcion,
                MomentoNovedad: momentoFinal
            };

            // 4. POST a la API
            const response = await fetch(API_URL, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(data)
            });

            if (response.ok) {
                alert("Novedad registrada correctamente.");

                // ⬅⬅⬅ AQUÍ ESTABA EL PROBLEMA
                // Antes: window.location.href = "novedades.html";
                // Ahora volvemos al módulo correcto:
                window.location.href = "novedad.html"; 
                // o si quieres absoluto: window.location.href = "/view/view/novedad.html";

            } else {
                const err = await response.text();
                console.error("Error al registrar:", err);
                alert(`Error al registrar la novedad. Código: ${response.status}`);
            }

        } catch (error) {
            console.error("Error de red:", error);
            alert("No se pudo conectar con la API.");
        }
    });
});





