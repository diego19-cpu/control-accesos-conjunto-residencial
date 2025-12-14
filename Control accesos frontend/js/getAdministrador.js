document.addEventListener("DOMContentLoaded", () => {
    // La tabla donde inyectaremos los datos
    const tabla = document.getElementById("cuerpoTablaAdministradores");
    const contenedorPaginacion = document.getElementById("paginacionAdministradores");
    const API_URL = "https://localhost:44331/api/Administrador";

    // ----- variables para paginación -----
    const registrosPorPagina = 5; // cantidad de filas por página
    let paginaActual = 1;
    let administradores = []; // aquí guardamos todos los datos que vienen de la API

    // --- FUNCIÓN PARA DIBUJAR LA TABLA SEGÚN LA PÁGINA ACTUAL ---
    function mostrarPagina() {
        tabla.innerHTML = "";

        const inicio = (paginaActual - 1) * registrosPorPagina;
        const fin = inicio + registrosPorPagina;

        const datosPagina = administradores.slice(inicio, fin);

        datosPagina.forEach((admin) => {
            const row = document.createElement("tr");
            row.innerHTML = `
                <td class="text-center">${admin.IdAdministrador}</td>
                <td class="text-center">${admin.Nombre}</td>
                <td class="text-center">${admin.Correo}</td>
                <td class="text-center">${admin.Cargo}</td>
                <td class="text-center">
                    <!-- botón EDITAR -->
                    <button data-id="${admin.IdAdministrador}" class="btn btn-warning btn-editar">
                        Editar
                    </button>
                </td>
                <td class="text-center">
                    <!-- botón ELIMINAR -->
                    <button data-id="${admin.IdAdministrador}" class="btn btn-danger btn-borrar">
                        Eliminar
                    </button>
                </td>
            `;
            tabla.appendChild(row);
        });
    }

    // --- FUNCIÓN PARA DIBUJAR LOS BOTONES DE PAGINACIÓN ---
    function mostrarPaginacion() {
        const totalPaginas = Math.max(1, Math.ceil(administradores.length / registrosPorPagina));

        contenedorPaginacion.innerHTML = `
            <button id="btnAnterior" class="btn btn-outline-primary me-2" ${paginaActual === 1 ? "disabled" : ""}>
                « Anterior
            </button>
            <span> Página ${paginaActual} de ${totalPaginas} </span>
            <button id="btnSiguiente" class="btn btn-outline-primary ms-2" ${paginaActual === totalPaginas ? "disabled" : ""}>
                Siguiente »
            </button>
        `;

        const btnAnterior = document.getElementById("btnAnterior");
        const btnSiguiente = document.getElementById("btnSiguiente");

        btnAnterior.addEventListener("click", () => {
            if (paginaActual > 1) {
                paginaActual--;
                mostrarPagina();
                mostrarPaginacion();
            }
        });

        btnSiguiente.addEventListener("click", () => {
            const totalPaginas = Math.ceil(administradores.length / registrosPorPagina);
            if (paginaActual < totalPaginas) {
                paginaActual++;
                mostrarPagina();
                mostrarPaginacion();
            }
        });
    }

    // --- FUNCIÓN PARA OBTENER Y GUARDAR LOS ADMINISTRADORES (GET) ---
    function obtenerAdministradores() {
        fetch(API_URL)
            .then((response) => response.json())
            .then((data) => {
                administradores = data;    // guardamos todos los registros
                paginaActual = 1;          // siempre empezamos en la página 1
                mostrarPagina();           // mostramos la tabla
                mostrarPaginacion();       // mostramos los botones
                console.log(data);
            })
            .catch((error) =>
                console.error("Error al obtener datos de la API:", error)
            );
    }

    obtenerAdministradores();

    // --- MANEJO DE EVENTOS (EDITAR y ELIMINAR) ---
    tabla.addEventListener("click", (event) => {
        const target = event.target;
        const id = target.dataset.id; // Usamos data-id para obtener el ID

        // 🔹 BOTÓN EDITAR
        if (target.classList.contains("btn-editar")) {
            // Redirigimos a la página de edición pasando el ID por la URL
            window.location.href = `editar-administrador.html?id=${id}`;
            return;
        }

        // 🔹 BOTÓN ELIMINAR
        if (target.classList.contains("btn-borrar")) {
            const confirmacion = confirm(
                "¿Estás seguro de que deseas eliminar este registro de administrador?"
            );

            if (confirmacion) {
                fetch(`${API_URL}/${id}`, {
                    method: "DELETE",
                })
                    .then((response) => {
                        if (response.status === 204) {
                            alert("Administrador eliminado correctamente.");
                            obtenerAdministradores();
                        } else if (response.status === 404) {
                            alert("Error: Administrador no encontrado.");
                        } else {
                            throw new Error(`Error al eliminar el administrador. Código: ${response.status}`);
                        }
                    })
                    .catch((error) => console.error("Error al eliminar administrador:", error));
            }
        }
    });
});




