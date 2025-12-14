document.addEventListener("DOMContentLoaded", () => {
    const API_URL = "https://localhost:44331/api/Novedad";

    // Campos del formulario
    const idInput          = document.getElementById("idNovedad");
    const idVigilanteInput = document.getElementById("idVigilante");
    const idAdminInput     = document.getElementById("idAdministrador");
    const idPersonalInput  = document.getElementById("idPersonal");
    const descripcionInput = document.getElementById("descripcion");
    const momentoInput     = document.getElementById("momentoNovedad");
    const btnActualizar    = document.getElementById("btnActualizarNovedad");

    if (!btnActualizar) {
        console.error("No se encontró el botón btnActualizarNovedad en la página.");
        return;
    }

    // 1) Obtener el id de la URL
    const params = new URLSearchParams(window.location.search);
    const id = params.get("id");

    if (!id) {
        alert("No se proporcionó un ID de novedad en la URL.");
        return;
    }

    idInput.value = id; // lo mostramos en el input

    // helper fecha API -> input datetime-local
    function formatearFechaParaInput(fechaStr) {
        if (!fechaStr) return "";
        const fecha = new Date(fechaStr);
        if (isNaN(fecha)) return "";
        const Y = fecha.getFullYear();
        const M = String(fecha.getMonth() + 1).padStart(2, "0");
        const D = String(fecha.getDate()).padStart(2, "0");
        const h = String(fecha.getHours()).padStart(2, "0");
        const m = String(fecha.getMinutes()).padStart(2, "0");
        return `${Y}-${M}-${D}T${h}:${m}`;
    }

    // 2) Cargar los datos actuales de la novedad
    fetch(`${API_URL}/${id}`)
        .then(res => {
            if (!res.ok) {
                throw new Error("No se pudo obtener la novedad. Código: " + res.status);
            }
            return res.json();
        })
        .then(nov => {
            idVigilanteInput.value = nov.IdVigilante;
            idAdminInput.value     = nov.IdAdministrador ?? "";
            idPersonalInput.value  = nov.IdPersonal ?? "";
            descripcionInput.value = nov.Descripcion;
            momentoInput.value     = formatearFechaParaInput(nov.MomentoNovedad);
        })
        .catch(err => {
            console.error("Error al cargar novedad:", err);
            alert("Error al cargar los datos de la novedad.");
        });

    // 3) Manejar el clic para actualizar (PUT)
    btnActualizar.addEventListener("click", (event) => {
        // Evitar submit normal del formulario
        event.preventDefault();

        const idVigTexto   = idVigilanteInput.value.trim();
        const idAdminTexto = idAdminInput.value.trim();
        const idPersTexto  = idPersonalInput.value.trim();
        const descTexto    = descripcionInput.value.trim();
        const momentoTexto = momentoInput.value.trim();

        // Validación básica
        if (!idVigTexto || !descTexto || !momentoTexto) {
            alert("Por favor, complete ID Vigilante, Descripción y Momento.");
            return;
        }

        if (isNaN(idVigTexto)) {
            alert("El ID del vigilante debe ser numérico.");
            return;
        }

        // Opcionales
        let idAdministrador = null;
        if (idAdminTexto !== "") {
            if (isNaN(idAdminTexto)) {
                alert("ID de administrador inválido.");
                return;
            }
            idAdministrador = parseInt(idAdminTexto, 10);
        }

        let idPersonal = null;
        if (idPersTexto !== "") {
            if (isNaN(idPersTexto)) {
                alert("ID de personal inválido.");
                return;
            }
            idPersonal = parseInt(idPersTexto, 10);
        }

        // Formato datetime-local -> yyyy-MM-ddTHH:mm:ss
        let momento = momentoTexto;
        if (momento.length === 16) {
            momento += ":00";
        }

        const data = {
            IdNovedad:       parseInt(idInput.value, 10),
            IdVigilante:     parseInt(idVigTexto, 10),
            IdAdministrador: idAdministrador,
            IdPersonal:      idPersonal,
            Descripcion:     descTexto,
            MomentoNovedad:  momento
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
                alert("Novedad actualizada exitosamente.");
                // 🔙 Volver al módulo de Novedad
                window.location.href = "/view/view/novedad.html";
            } else {
                console.error("Error al actualizar novedad. Código:", response.status);
                alert(`Error al actualizar novedad. Código: ${response.status}`);
            }
        })
        .catch((error) => {
            console.error("Error de red al actualizar novedad:", error);
            alert("Error de conexión al actualizar la novedad.");
        });
    });
});





