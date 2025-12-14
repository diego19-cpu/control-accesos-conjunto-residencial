document.addEventListener("DOMContentLoaded", () => {
    const API_URL = "https://localhost:44331/api/Turno"; 
    const btnRegistrar = document.getElementById("btnRegistrarTurno");

    btnRegistrar.addEventListener("click", async () => {
        const idTurno = document.getElementById("idTurno").value.trim();
        const asignacion = document.getElementById("asignacionTurno").value.trim();
        const horaInicio = document.getElementById("horaInicio").value.trim(); // "HH:mm"
        const horaFin = document.getElementById("horaFin").value.trim();       // "HH:mm"

        if (!idTurno || !asignacion || !horaInicio || !horaFin) {
            alert("Por favor, complete todos los campos.");
            return;
        }

        const data = {
            IdTurno: parseInt(idTurno, 10),
            AsignacionTurno: asignacion,
            HoraInicio: horaInicio + ":00", // lo mandamos como HH:mm:ss
            HoraFin: horaFin + ":00"
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
                alert("Turno registrado correctamente.");
                window.location.href = "turnos.html";
            } else {
                const errorText = await response.text();
                console.error(`Error al registrar turno. Código: ${response.status}`, errorText);
                alert(`Error al registrar turno. Código: ${response.status}`);
            }
        } catch (error) {
            console.error("Error de conexión o red al registrar turno:", error);
            alert("Fallo la conexión con la API de Turnos.");
        }
    });
});
