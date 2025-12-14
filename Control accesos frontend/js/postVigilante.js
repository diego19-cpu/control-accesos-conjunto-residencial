document.addEventListener("DOMContentLoaded", () => {
    const API_URL = "https://localhost:44331/api/Vigilante"; 
    const btnRegistrar = document.getElementById("btnRegistrarVigilante");

    btnRegistrar.addEventListener("click", async () => {
        const idVigilante    = document.getElementById("idVigilante").value.trim();
        const nombre         = document.getElementById("nombre").value.trim();
        const apellido       = document.getElementById("apellido").value.trim();
        const idTurno        = document.getElementById("idTurno").value.trim();
        const idAdministrador = document.getElementById("idAdministrador").value.trim();

        if (!idVigilante || !nombre || !apellido || !idTurno || !idAdministrador) {
            alert("Por favor, complete todos los campos.");
            return;
        }

        const data = {
            IdVigilante: parseInt(idVigilante, 10),
            Nombre: nombre,
            Apellido: apellido,
            IdTurno: parseInt(idTurno, 10),
            IdAdministrador: parseInt(idAdministrador, 10)
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
                alert("Vigilante registrado correctamente.");
                window.location.href = "vigilantes.html";
            } else {
                const errorText = await response.text();
                console.error(`Error al registrar vigilante. Código: ${response.status}`, errorText);
                alert(`Error al registrar vigilante. Código: ${response.status}`);
            }
        } catch (error) {
            console.error("Error de conexión o red al registrar vigilante:", error);
            alert("Fallo la conexión con la API de Vigilantes.");
        }
    });
});
