document.addEventListener("DOMContentLoaded", () => {
    const API_URL = "https://localhost:44331/api/Turno";

    // Campos del formulario
    const idInput       = document.getElementById("idTurno");
    const asignacionInp = document.getElementById("asignacionTurno");
    const horaInicioInp = document.getElementById("horaInicio");
    const horaFinInp    = document.getElementById("horaFin");
    const btnActualizar = document.getElementById("btnActualizarTurno");

    // 1) Obtener el id de la URL
    const params = new URLSearchParams(window.location.search);
    const id = params.get("id");

    if (!id) {
        alert("No se proporcionó un ID de turno en la URL.");
        return;
    }

    idInput.value = id;

    // 2) Cargar los datos actuales del turno
    fetch(`${API_URL}/${id}`)
        .then(res => {
            if (!res.ok) {
                throw new Error("No se pudo obtener el turno. Código: " + res.status);
            }
            return res.json();
        })
        .then(turno => {
            asignacionInp.value = turno.AsignacionTurno;
            // Horas vienen como "HH:mm:ss" → value del input es "HH:mm"
            if (turno.HoraInicio) {
                horaInicioInp.value = turno.HoraInicio.toString().substring(0,5);
            }
            if (turno.HoraFin) {
                horaFinInp.value = turno.HoraFin.toString().substring(0,5);
            }
        })
        .catch(err => {
            console.error("Error al cargar turno:", err);
            alert("Error al cargar los datos del turno.");
        });

    // 3) Manejar el clic para actualizar (PUT)
    btnActualizar.addEventListener("click", () => {
        const asignacion = asignacionInp.value.trim();
        const horaIni    = horaInicioInp.value.trim();
        const horaFin    = horaFinInp.value.trim();

        if (!asignacion || !horaIni || !horaFin) {
            alert("Por favor, complete todos los campos.");
            return;
        }

        const data = {
            IdTurno: parseInt(idInput.value, 10),
            AsignacionTurno: asignacion,
            HoraInicio: horaIni + ":00",
            HoraFin: horaFin + ":00"
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
                alert("Turno actualizado exitosamente.");
                window.location.href = "turnos.html";
            } else {
                console.error("Error al actualizar turno. Código:", response.status);
                alert(`Error al actualizar turno. Código: ${response.status}`);
            }
        })
        .catch((error) => {
            console.error("Error de red al actualizar turno:", error);
            alert("Error de conexión al actualizar el turno.");
        });
    });
});
