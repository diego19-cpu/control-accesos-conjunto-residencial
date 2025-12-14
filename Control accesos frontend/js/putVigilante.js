document.addEventListener("DOMContentLoaded", () => {
    const API_URL = "https://localhost:44331/api/Vigilante";

    const idInput       = document.getElementById("idVigilante");
    const nombreInput   = document.getElementById("nombre");
    const apellidoInput = document.getElementById("apellido");
    const turnoInput    = document.getElementById("idTurno");
    const adminInput    = document.getElementById("idAdministrador");
    const btnActualizar = document.getElementById("btnEditar"); // 👈 id del botón en tu HTML

    console.log("btnEditar encontrado:", btnActualizar);

    // Si no encuentra el botón, salimos para no romper nada
    if (!btnActualizar) {
        console.error("No se encontró el botón con id 'btnEditar' en esta página.");
        return;
    }

    // 1) Obtener ID desde la URL
    const params = new URLSearchParams(window.location.search);
    const id = params.get("id");

    if (!id) {
        alert("No se proporcionó un ID de vigilante en la URL.");
        return;
    }

    idInput.value = id;

    // 2) Cargar datos actuales del vigilante
    fetch(`${API_URL}/${id}`)
        .then(res => {
            if (!res.ok) {
                throw new Error("No se pudo obtener el vigilante. Código: " + res.status);
            }
            return res.json();
        })
        .then(v => {
            nombreInput.value   = v.Nombre;
            apellidoInput.value = v.Apellido;
            turnoInput.value    = v.IdTurno;
            adminInput.value    = v.IdAdministrador;
        })
        .catch(err => {
            console.error("Error al cargar vigilante:", err);
            alert("Error al cargar los datos del vigilante.");
        });

    // 3) Enviar actualización (PUT)
    btnActualizar.addEventListener("click", () => {
        const nombre   = nombreInput.value.trim();
        const apellido = apellidoInput.value.trim();
        const idTurno  = turnoInput.value.trim();
        const idAdmin  = adminInput.value.trim();

        if (!nombre || !apellido || !idTurno || !idAdmin) {
            alert("Por favor, complete todos los campos.");
            return;
        }

        const data = {
            IdVigilante: parseInt(idInput.value, 10),
            Nombre: nombre,
            Apellido: apellido,
            IdTurno: parseInt(idTurno, 10),
            IdAdministrador: parseInt(idAdmin, 10)
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
                alert("Vigilante actualizado exitosamente.");
                window.location.href = "vigilantes.html";
            } else {
                console.error("Error al actualizar vigilante. Código:", response.status);
                alert(`Error al actualizar vigilante. Código: ${response.status}`);
            }
        })
        .catch((error) => {
            console.error("Error de red al actualizar vigilante:", error);
            alert("Error de conexión al actualizar el vigilante.");
        });
    });
});

