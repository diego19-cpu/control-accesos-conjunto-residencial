// getNovedad.js
document.addEventListener("DOMContentLoaded", () => {

    // Tabla y paginación del módulo NOVEDAD
    const tabla = document.getElementById("cuerpoTablaNovedad");
    const contenedorPaginacion = document.getElementById("paginacionNovedad");

    // URL de la API (usa tu backend local para evitar CORS)
    // Igual que usas para Administrador, pero con Novedad
    const API_URL = "https://localhost:44331/api/Novedad";   // o /novedad, según tu Web API

    // Evitar errores si se carga este JS en otra página
    if (!tabla || !contenedorPaginacion) {
        console.error("No se encontró el cuerpo de la tabla o el contenedor de paginación en NOVEDAD.");
        return;
    }

    // Paginación
    const registrosPorPagina = 5;
    let paginaActual = 1;
    let novedades = [];

    // --- Formatear fecha ---
    function formatearFecha(f) {
        if (!f) return "-";
        const fecha = new Date(f);
        return isNaN(fecha) ? f : fecha.toLocaleString();
    }

    // --- Dibujar tabla ---
    function mostrarPagina() {
        tabla.innerHTML = "";

        const inicio = (paginaActual - 1) * registrosPorPagina;
        const fin = inicio + registrosPorPagina;

        const datosPagina = novedades.slice(inicio, fin);

        datosPagina.forEach((nov) => {
            const row = document.createElement("tr");

            row.innerHTML = `
                <td class="text-center">${nov.IdNovedad}</td>
                <td class="text-center">${nov.IdVigilante}</td>
                <td class="text-center">${nov.IdAdministrador ?? "-"}</td>
                <td class="text-center">${nov.IdPersonal ?? "-"}</td>
                <td class="text-center">${nov.Descripcion}</td>
                <td class="text-center">${formatearFecha(nov.MomentoNovedad)}</td>

                <td class="text-center">
                    <button data-id="${nov.IdNovedad}" class="btn btn-warning btn-editar">
                        Editar
                    </button>
                </td>

                <td class="text-center">
                    <button data-id="${nov.IdNovedad}" class="btn btn-danger btn-borrar">
                        Eliminar
                    </button>
                </td>
            `;

            tabla.appendChild(row);
        });
    }

    // --- Dibujar paginación ---
    function mostrarPaginacion() {
        const totalPaginas = Math.max(1, Math.ceil(novedades.length / registrosPorPagina));

        contenedorPaginacion.innerHTML = `
            <button id="btnAnteriorNovedad" 
                    class="btn btn-outline-primary me-2"
                    ${paginaActual === 1 ? "disabled" : ""}>
                « Anterior
            </button>

            <span> Página ${paginaActual} de ${totalPaginas} </span>

            <button id="btnSiguienteNovedad" 
                    class="btn btn-outline-primary ms-2"
                    ${paginaActual === totalPaginas ? "disabled" : ""}>
                Siguiente »
            </button>
        `;

        const btnAnterior = document.getElementById("btnAnteriorNovedad");
        const btnSiguiente = document.getElementById("btnSiguienteNovedad");

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
                const totalPaginasActual = Math.ceil(novedades.length / registrosPorPagina);
                if (paginaActual < totalPaginasActual) {
                    paginaActual++;
                    mostrarPagina();
                    mostrarPaginacion();
                }
            });
        }
    }

    // --- Obtener datos desde la API (GET) ---
    function obtenerNovedades() {
        fetch(API_URL)
            .then(resp => {
                if (!resp.ok) {
                    throw new Error("HTTP " + resp.status);
                }
                return resp.json();
            })
            .then(data => {
                // Por si tu API devuelve array directo o envuelto en $values
                if (Array.isArray(data)) {
                    novedades = data;
                } else if (data && Array.isArray(data.$values)) {
                    novedades = data.$values;
                } else {
                    console.warn("Formato de datos inesperado:", data);
                    novedades = [];
                }

                paginaActual = 1;
                mostrarPagina();
                mostrarPaginacion();

                console.log("Novedades cargadas:", novedades);
            })
            .catch(err => {
                console.error("Error al obtener novedades:", err);
            });
    }

    obtenerNovedades();

    // --- Eventos de Editar y Eliminar ---
    tabla.addEventListener("click", (e) => {
        const target = e.target;
        const id = target.dataset.id;

        if (!id) return;

        // EDITAR
        if (target.classList.contains("btn-editar")) {
            window.location.href = `editar-novedad.html?id=${id}`;
        }

        // ELIMINAR
        if (target.classList.contains("btn-borrar")) {
            if (confirm("¿Deseas eliminar esta novedad?")) {
                fetch(`${API_URL}/${id}`, {
                    method: "DELETE"
                })
                .then(res => {
                    if (res.ok) {
                        alert("Novedad eliminada.");
                        obtenerNovedades();
                    } else {
                        alert("Error al eliminar. Código: " + res.status);
                    }
                })
                .catch(err => console.error(err));
            }
        }
    });
});




