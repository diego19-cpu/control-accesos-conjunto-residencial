document.addEventListener("DOMContentLoaded", () => {
    // Leemos el rol que se guardó en el login
    const rol = localStorage.getItem("rol");   // "administrador", "vigilante" o "propietario"

    // La tabla donde inyectaremos los datos de ACCESOS
    const tabla = document.getElementById("cuerpoTablaAcceso");
    const contenedorPaginacion = document.getElementById("paginacionAcceso");
    const totalAccesosLabel = document.getElementById("totalAccesos");
    const API_URL = "https://localhost:44331/api/Acceso"; // ajusta si tu ruta es distinta

    if (!tabla || !contenedorPaginacion) {
        console.error("No se encontró la tabla o el contenedor de paginación para accesos.");
        return;
    }

    // ----- variables para paginación -----
    const registrosPorPagina = 5;
    let paginaActual = 1;
    let accesos = [];

    function formatearFechaHora(valor) {
        if (!valor) return "";
        const fecha = new Date(valor);
        if (isNaN(fecha)) return valor; // si no lo puede parsear, devuelve tal cual
        return fecha.toLocaleString();
    }

    // --- ACTUALIZAR TOTAL DE ACCESOS ---
    function actualizarTotalAccesos() {
        if (!totalAccesosLabel) return;
        const total = accesos.length;
        totalAccesosLabel.textContent = `Total de accesos: ${total}`;
    }

    // --- FUNCIÓN PARA DIBUJAR LA TABLA ---
    function mostrarPagina() {
        tabla.innerHTML = "";

        const inicio = (paginaActual - 1) * registrosPorPagina;
        const fin = inicio + registrosPorPagina;

        const datosPagina = accesos.slice(inicio, fin);

        datosPagina.forEach((acc) => {
            const row = document.createElement("tr");

            // 👇 según el rol, mostramos botones o solo guiones
            let celdasAcciones = "";

            if (rol === "propietario") {
                // El propietario NO puede editar ni eliminar
                celdasAcciones = `
                    <td class="text-center">-</td>
                    <td class="text-center">-</td>
                `;
            } else {
                // Admin/Vigilante sí pueden ver y usar los botones
                celdasAcciones = `
                    <td class="text-center">
                        <button data-id="${acc.IdAcceso}" class="btn btn-warning btn-editar solo-no-propietario">
                            Editar
                        </button>
                    </td>
                    <td class="text-center">
                        <button data-id="${acc.IdAcceso}" class="btn btn-danger btn-borrar solo-no-propietario">
                            Eliminar
                        </button>
                    </td>
                `;
            }

            row.innerHTML = `
                <td class="text-center">${acc.IdAcceso}</td>
                <td class="text-center">${acc.IdVigilante}</td>
                <td class="text-center">${acc.IdPersonal}</td>
                <td class="text-center">${formatearFechaHora(acc.MomentoIngreso)}</td>
                <td class="text-center">${acc.MomentoSalida ? formatearFechaHora(acc.MomentoSalida) : "-"}</td>
                
                <!-- VISITAS: siempre número -->
                <td class="text-center">${acc.NumeroVisitas != null ? acc.NumeroVisitas : 0}</td>
                
                <td class="text-center">${acc.AutorizadoPor || "-"}</td>
                <td class="text-center">${acc.NumeroPlacaVehiculo || "-"}</td>
                ${celdasAcciones}
            `;
            tabla.appendChild(row);
        });
    }

    // --- PAGINACIÓN ---
    function mostrarPaginacion() {
        const totalPaginas = Math.max(1, Math.ceil(accesos.length / registrosPorPagina));

        contenedorPaginacion.innerHTML = `
            <button id="btnAnteriorAcc" class="btn btn-outline-primary me-2" ${paginaActual === 1 ? "disabled" : ""}>
                « Anterior
            </button>
            <span> Página ${paginaActual} de ${totalPaginas} </span>
            <button id="btnSiguienteAcc" class="btn btn-outline-primary ms-2" ${paginaActual === totalPaginas ? "disabled" : ""}>
                Siguiente »
            </button>
        `;

        const btnAnterior = document.getElementById("btnAnteriorAcc");
        const btnSiguiente = document.getElementById("btnSiguienteAcc");

        if (btnAnterior) {
            btnAnterior.addEventListener("click", () => {
                if (paginaActual > 1) {
                    paginaActual--;
                    mostrarPagina();
                    mostrarPaginacion();
                }
            });
        }

        if (btnSiguiente) {
            btnSiguiente.addEventListener("click", () => {
                const totalPaginasActual = Math.ceil(accesos.length / registrosPorPagina);
                if (paginaActual < totalPaginasActual) {
                    paginaActual++;
                    mostrarPagina();
                    mostrarPaginacion();
                }
            });
        }
    }

    // --- GET ACCESOS ---
    function obtenerAccesos() {
        fetch(API_URL)
            .then((response) => {
                if (!response.ok) {
                    throw new Error(`Error HTTP ${response.status}`);
                }
                return response.json();
            })
            .then((data) => {
                accesos = Array.isArray(data) ? data : [];
                paginaActual = 1;
                mostrarPagina();
                mostrarPaginacion();
                actualizarTotalAccesos();
                console.log("Accesos:", data);
            })
            .catch((error) => {
                console.error("Error al obtener accesos de la API:", error);
                alert("No se pudieron obtener los accesos desde la API.");
            });
    }

    obtenerAccesos();

    // --- EDITAR y ELIMINAR ---
    tabla.addEventListener("click", (event) => {
        const target = event.target;
        if (!(target instanceof HTMLElement)) return;

        const id = target.dataset.id;
        if (!id) return;

        // 🔒 Bloque extra por seguridad: si es propietario, no se hace nada
        if (rol === "propietario") {
            alert("No tiene permisos para realizar esta acción.");
            return;
        }

        // EDITAR
        if (target.classList.contains("btn-editar")) {
            window.location.href = `editar-acceso.html?id=${id}`;
            return;
        }

        // ELIMINAR
        if (target.classList.contains("btn-borrar")) {
            const confirmacion = confirm(
                "¿Estás seguro de que deseas eliminar este registro de acceso?"
            );

            if (confirmacion) {
                fetch(`${API_URL}/${id}`, {
                    method: "DELETE",
                })
                    .then((response) => {
                        if (response.status === 204) {
                            alert("Acceso eliminado correctamente.");
                            obtenerAccesos(); // vuelve a cargar y actualiza total
                        } else if (response.status === 404) {
                            alert("Error: Acceso no encontrado.");
                        } else {
                            throw new Error(`Error al eliminar el acceso. Código: ${response.status}`);
                        }
                    })
                    .catch((error) => {
                        console.error("Error al eliminar acceso:", error);
                        alert("No se pudo eliminar el acceso.");
                    });
            }
        }
    });
});


