document.addEventListener("DOMContentLoaded", () => {
    // La tabla donde inyectaremos los datos de PERSONAL
    const tabla = document.getElementById("cuerpoTablaPersonal");
    const contenedorPaginacion = document.getElementById("paginacionPersonal");
    const API_URL = "https://localhost:44331/api/Personal"; // Ajusta si tu ruta es distinta

    // 👉 CAMBIO MÍNIMO:
    // Si este script se carga en una página sin tabla (por ejemplo registro-personal.html),
    // salimos sin hacer nada.
    if (!tabla || !contenedorPaginacion) {
        return;
    }

    // ----- variables para paginación -----
    const registrosPorPagina = 5; // cantidad de filas por página
    let paginaActual = 1;
    let personal = []; // aquí guardamos todos los datos que vienen de la API

    // --- FUNCIÓN PARA DIBUJAR LA TABLA SEGÚN LA PÁGINA ACTUAL ---
    function mostrarPagina() {
        tabla.innerHTML = "";

        const inicio = (paginaActual - 1) * registrosPorPagina;
        const fin = inicio + registrosPorPagina;

        const datosPagina = personal.slice(inicio, fin);

        datosPagina.forEach((per) => {
            const row = document.createElement("tr");
            row.innerHTML = `
                <td class="text-center">${per.IdPersonal}</td>
                <td class="text-center">${per.Nombre}</td>
                <td class="text-center">${per.Apellido}</td>
                <td class="text-center">${per.DocumentoIdentidad}</td>
                <td class="text-center">${per.TipoPersona}</td>
                <td class="text-center">${per.IdInmueble}</td>
                <td class="text-center">
                    <!-- botón EDITAR -->
                    <button data-id="${per.IdPersonal}" class="btn btn-warning btn-editar">
                        Editar
                    </button>
                </td>
                <td class="text-center">
                    <!-- botón ELIMINAR -->
                    <button data-id="${per.IdPersonal}" class="btn btn-danger btn-borrar">
                        Eliminar
                    </button>
                </td>
            `;
            tabla.appendChild(row);
        });
    }

    // --- FUNCIÓN PARA DIBUJAR LOS BOTONES DE PAGINACIÓN ---
    function mostrarPaginacion() {
        const totalPaginas = Math.max(1, Math.ceil(personal.length / registrosPorPagina));

        contenedorPaginacion.innerHTML = `
            <button id="btnAnteriorPer" class="btn btn-outline-primary me-2" ${paginaActual === 1 ? "disabled" : ""}>
                « Anterior
            </button>
            <span> Página ${paginaActual} de ${totalPaginas} </span>
            <button id="btnSiguientePer" class="btn btn-outline-primary ms-2" ${paginaActual === totalPaginas ? "disabled" : ""}>
                Siguiente »
            </button>
        `;

        const btnAnterior = document.getElementById("btnAnteriorPer");
        const btnSiguiente = document.getElementById("btnSiguientePer");

        btnAnterior.addEventListener("click", () => {
            if (paginaActual > 1) {
                paginaActual--;
                mostrarPagina();
                mostrarPaginacion();
            }
        });

        btnSiguiente.addEventListener("click", () => {
            const totalPaginas = Math.ceil(personal.length / registrosPorPagina);
            if (paginaActual < totalPaginas) {
                paginaActual++;
                mostrarPagina();
                mostrarPaginacion();
            }
        });
    }

    // --- FUNCIÓN PARA OBTENER Y GUARDAR EL PERSONAL (GET) ---
    function obtenerPersonal() {
        fetch(API_URL)
            .then((response) => response.json())
            .then((data) => {
                personal = data;   // guardamos todos los registros
                paginaActual = 1;  // siempre empezamos en la página 1
                mostrarPagina();   // mostramos la tabla
                mostrarPaginacion(); // mostramos los botones
                console.log(data);
            })
            .catch((error) =>
                console.error("Error al obtener personal de la API:", error)
            );
    }

    obtenerPersonal();

    // --- MANEJO DE EVENTOS (EDITAR y ELIMINAR) ---
    tabla.addEventListener("click", (event) => {
        const target = event.target;
        const id = target.dataset.id; // Usamos data-id para obtener el ID

        // 🔹 BOTÓN EDITAR
        if (target.classList.contains("btn-editar")) {
            // Ir a la página de EDICIÓN (editar-personal.html)
            window.location.href = `editar-personal.html?id=${id}`;
            return;
        }

        // 🔹 BOTÓN ELIMINAR
        if (target.classList.contains("btn-borrar")) {
            const confirmacion = confirm(
                "¿Estás seguro de que deseas eliminar este registro de personal?"
            );

            if (confirmacion) {
                fetch(`${API_URL}/${id}`, {
                    method: "DELETE",
                })
                    .then((response) => {
                        if (response.status === 204) {
                            alert("Registro de personal eliminado correctamente.");
                            obtenerPersonal();
                        } else if (response.status === 404) {
                            alert("Error: Personal no encontrado.");
                        } else {
                            throw new Error(`Error al eliminar el personal. Código: ${response.status}`);
                        }
                    })
                    .catch((error) => console.error("Error al eliminar personal:", error));
            }
        }
    });
});

