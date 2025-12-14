document.addEventListener("DOMContentLoaded", () => {
    // La tabla donde inyectaremos los datos
    const tabla = document.getElementById("cuerpoTablaTurnos");
    const contenedorPaginacion = document.getElementById("paginacionTurnos");
    const API_URL = "https://localhost:44331/api/Turno";

    // ----- variables para paginación -----
    const registrosPorPagina = 5; 
    let paginaActual = 1;
    let turnos = []; // aquí guardamos todos los datos que vienen de la API

    // --- FUNCIÓN PARA DIBUJAR LA TABLA SEGÚN LA PÁGINA ACTUAL ---
    function mostrarPagina() {
        tabla.innerHTML = "";

        const inicio = (paginaActual - 1) * registrosPorPagina;
        const fin = inicio + registrosPorPagina;

        const datosPagina = turnos.slice(inicio, fin);

        datosPagina.forEach((turno) => {
            const row = document.createElement("tr");

            // HoraInicio y HoraFin vienen como "HH:mm:ss", mostramos solo HH:mm
            const horaInicio = (turno.HoraInicio || "").toString().substring(0,5);
            const horaFin    = (turno.HoraFin    || "").toString().substring(0,5);

            row.innerHTML = `
                <td class="text-center">${turno.IdTurno}</td>
                <td class="text-center">${turno.AsignacionTurno}</td>
                <td class="text-center">${horaInicio}</td>
                <td class="text-center">${horaFin}</td>
                <td class="text-center">
                    <button data-id="${turno.IdTurno}" class="btn btn-warning btn-editar">
                        Editar
                    </button>
                </td>
                <td class="text-center">
                    <button data-id="${turno.IdTurno}" class="btn btn-danger btn-borrar">
                        Eliminar
                    </button>
                </td>
            `;
            tabla.appendChild(row);
        });
    }

    // --- FUNCIÓN PARA DIBUJAR LOS BOTONES DE PAGINACIÓN ---
    function mostrarPaginacion() {
        const totalPaginas = Math.max(1, Math.ceil(turnos.length / registrosPorPagina));

        contenedorPaginacion.innerHTML = `
            <button id="btnAnteriorTurno" class="btn btn-outline-primary me-2" ${paginaActual === 1 ? "disabled" : ""}>
                « Anterior
            </button>
            <span> Página ${paginaActual} de ${totalPaginas} </span>
            <button id="btnSiguienteTurno" class="btn btn-outline-primary ms-2" ${paginaActual === totalPaginas ? "disabled" : ""}>
                Siguiente »
            </button>
        `;

        const btnAnterior = document.getElementById("btnAnteriorTurno");
        const btnSiguiente = document.getElementById("btnSiguienteTurno");

        btnAnterior.addEventListener("click", () => {
            if (paginaActual > 1) {
                paginaActual--;
                mostrarPagina();
                mostrarPaginacion();
            }
        });

        btnSiguiente.addEventListener("click", () => {
            const totalPaginas = Math.ceil(turnos.length / registrosPorPagina);
            if (paginaActual < totalPaginas) {
                paginaActual++;
                mostrarPagina();
                mostrarPaginacion();
            }
        });
    }

    // --- FUNCIÓN PARA OBTENER Y GUARDAR LOS TURNOS (GET) ---
    function obtenerTurnos() {
        fetch(API_URL)
            .then((response) => response.json())
            .then((data) => {
                turnos = data;
                paginaActual = 1;
                mostrarPagina();
                mostrarPaginacion();
                console.log("Turnos:", data);
            })
            .catch((error) =>
                console.error("Error al obtener datos de la API de Turnos:", error)
            );
    }

    obtenerTurnos();

    // --- MANEJO DE EVENTOS (EDITAR y ELIMINAR) ---
    tabla.addEventListener("click", (event) => {
        const target = event.target;
        const id = target.dataset.id;

        // 🔹 BOTÓN EDITAR
        if (target.classList.contains("btn-editar")) {
            window.location.href = `editar-turno.html?id=${id}`;
            return;
        }

        // 🔹 BOTÓN ELIMINAR
        if (target.classList.contains("btn-borrar")) {
            const confirmacion = confirm(
                "¿Estás seguro de que deseas eliminar este turno?"
            );

            if (confirmacion) {
                fetch(`${API_URL}/${id}`, {
                    method: "DELETE",
                })
                    .then((response) => {
                        if (response.status === 204) {
                            alert("Turno eliminado correctamente.");
                            obtenerTurnos();
                        } else if (response.status === 404) {
                            alert("Error: Turno no encontrado.");
                        } else {
                            throw new Error(`Error al eliminar el turno. Código: ${response.status}`);
                        }
                    })
                    .catch((error) => console.error("Error al eliminar turno:", error));
            }
        }
    });
});
