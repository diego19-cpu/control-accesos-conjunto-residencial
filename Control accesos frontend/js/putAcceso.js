document.addEventListener("DOMContentLoaded", () => { 

    const API_URL = "https://localhost:44331/api/Acceso";
    const btnEditar = document.getElementById("btnEditarAcceso");

    const idAccesoInput      = document.getElementById("idAcceso");
    const idVigilanteInput   = document.getElementById("idVigilante");
    const idPersonalInput    = document.getElementById("idPersonal");
    const ingresoInput       = document.getElementById("momentoIngreso");
    const salidaInput        = document.getElementById("momentoSalida");
    const visitasInput       = document.getElementById("numeroVisitas");
    const autorizadoInput    = document.getElementById("autorizadoPor");
    // 🔧 CORREGIDO: este id debe coincidir con tu HTML
    const placaInput         = document.getElementById("numeroPlacaVehiculo");

    if (!btnEditar) {
        console.error("No se encontró el botón #btnEditarAcceso");
        return;
    }

    // 1. ID de la URL
    const params = new URLSearchParams(window.location.search);
    const idUrl = params.get("id");

    if (!idUrl) {
        alert("No se recibió un ID en la URL (ej: editar-acceso.html?id=1)");
        return;
    }

    // 2. Cargar datos
    fetch(`${API_URL}/${idUrl}`)
        .then(r => {
            if (!r.ok) throw new Error(`Error GET: ${r.status}`);
            return r.json();
        })
        .then(data => {
            console.log("Acceso cargado:", data);

            idAccesoInput.value    = data.IdAcceso;
            idVigilanteInput.value = data.IdVigilante;
            idPersonalInput.value  = data.IdPersonal;

            ingresoInput.value = convertirADatetimeLocal(data.MomentoIngreso);
            salidaInput.value  = data.MomentoSalida ? convertirADatetimeLocal(data.MomentoSalida) : "";

            visitasInput.value    = data.NumeroVisitas ?? "";
            autorizadoInput.value = data.AutorizadoPor ?? "";
            placaInput.value      = data.NumeroPlacaVehiculo ?? "";

            idAccesoInput.readOnly = true;
        })
        .catch(err => {
            console.error("Error al cargar acceso:", err);
            alert("No se pudieron cargar los datos del acceso.");
        });

    function convertirADatetimeLocal(valor) {
        if (!valor) return "";
        const d = new Date(valor);
        if (isNaN(d)) return "";
        const pad = n => n.toString().padStart(2, "0");
        const yyyy = d.getFullYear();
        const MM   = pad(d.getMonth() + 1);
        const dd   = pad(d.getDate());
        const hh   = pad(d.getHours());
        const mm   = pad(d.getMinutes());
        return `${yyyy}-${MM}-${dd}T${hh}:${mm}`;
    }

    // 👉 convertir a ISO para mandar a la API
    function convertirAISOorNull(valor) {
        if (!valor) return null;
        const d = new Date(valor);
        if (isNaN(d)) return null;
        return d.toISOString();
    }

    // 3. Actualizar
    btnEditar.addEventListener("click", () => {
        const idAcceso     = idAccesoInput.value.trim();
        const idVigilante  = idVigilanteInput.value.trim();
        const idPersonal   = idPersonalInput.value.trim();
        const ingreso      = ingresoInput.value.trim();
        const salida       = salidaInput.value.trim();
        const visitasTexto = visitasInput.value.trim();
        const autorizado   = autorizadoInput.value.trim();
        const placa        = placaInput.value.trim();

        if (!idAcceso || isNaN(idAcceso)) {
            alert("IdAcceso inválido.");
            return;
        }
        if (!idVigilante || !idPersonal || !ingreso) {
            alert("IdVigilante, IdPersonal y MomentoIngreso son obligatorios.");
            return;
        }

        const numeroVisitas = visitasTexto ? parseInt(visitasTexto, 10) : null;

        const data = {
            IdAcceso: parseInt(idAcceso, 10),
            IdVigilante: parseInt(idVigilante, 10),
            IdPersonal: parseInt(idPersonal, 10),
            // 🔧 ahora las fechas van en ISO
            MomentoIngreso: convertirAISOorNull(ingreso),
            MomentoSalida: convertirAISOorNull(salida),
            NumeroVisitas: numeroVisitas,
            AutorizadoPor: autorizado || null,
            NumeroPlacaVehiculo: placa || null
        };

        console.log("JSON que se envía al PUT:", data);

        fetch(`${API_URL}/${idAcceso}`, {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(data),
        })
        .then(r => {
            if (r.ok) {
                alert("Acceso actualizado correctamente.");
                // volver a la página anterior
                history.back();
            } else {
                return r.text().then(txt => {
                    console.error("Respuesta de error:", txt);
                    alert(`Error al actualizar acceso. Código: ${r.status}`);
                });
            }
        })
        .catch(err => {
            console.error("Error PUT acceso:", err);
            alert("No se pudo conectar con la API.");
        });
    });
});
