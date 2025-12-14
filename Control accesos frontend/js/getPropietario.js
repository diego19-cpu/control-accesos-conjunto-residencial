document.addEventListener("DOMContentLoaded", () => {
    // La tabla donde se inyectarán los datos
    const tabla = document.getElementById("cuerpoTablaPropietarios");
    const contenedorPaginacion = document.getElementById("paginacionPropietarios");
    const API_URL = "https://localhost:44331/api/Propietario";

    // ----- variables para paginación -----
    const registrosPorPagina = 5; 
    let paginaActual = 1;
    let propietarios = []; // aquí se guardan todos los datos

    // --- FUNCIÓN PARA DIBUJAR LA TABLA SEGÚN LA PÁGINA ---
    function mostrarPagina() {
        tabla.innerHTML = "";

        const inicio = (paginaActual - 1) * registrosPorPagina;
        const fin = inicio + registrosPorPagina;

        const datosPagina = propietarios.slice(inicio, fin);

        datosPagina.forEach((p) => {
            const row = document.createElement("tr");
            row.innerHTML = `
                <td class="text-center">${p.IdPropietario}</td>
                <td class="text-center">${p.Nombre}</td>
                <td class="text-center">${p.Apellido}</td>
                <td class="text-center">${p.Correo}</td>
                <td class="text-center">${p.Telefono}</td>

                <td class="text-center">
                    <button data-id="${p.IdPropietario}" class="btn btn-warning btn-editar">
                        Editar
                    </button>
                </td>

                <td class="text-center">
                    <button data-id="${p.IdPropietario}" class="btn btn-danger btn-borrar">
                        Eliminar
                    </button>
                </td>
            `;
            tabla.appendChild(row);
        });
    }

    // --- FUNCIÓN PARA MOSTRAR PAGINACIÓN ---
    function mostrarPaginacion() {
        const totalPaginas = Math.max(1, Math.ceil(propietarios.length / registrosPorPagina));

        contenedorPaginacion.innerHTML = `
            <button id="btnAnterior" class="btn btn-outline-primary me-2" ${paginaActual === 1 ? "disabled" : ""}>
                « Anterior
            </button>
            <span> Página ${paginaActual} de ${totalPaginas} </span>
            <button id="btnSiguiente" class="btn btn-outline-primary ms-2" ${paginaActual === totalPaginas ? "disabled" : ""}>
                Siguiente »
            </button>
        `;

        document.getElementById("btnAnterior").addEventListener("click", () => {
            if (paginaActual > 1) {
                paginaActual--;
                mostrarPagina();
                mostrarPaginacion();
            }
        });

        document.getElementById("btnSiguiente").addEventListener("click", () => {
            const totalPaginas = Math.ceil(propietarios.length / registrosPorPagina);
            if (paginaActual < totalPaginas) {
                paginaActual++;
                mostrarPagina();
                mostrarPaginacion();
            }
        });
    }

    // --- FUNCIÓN PARA OBTENER LOS PROPIETARIOS (GET) ---
    function obtenerPropietarios() {
        fetch(API_URL)
            .then((response) => response.json())
            .then((data) => {
                propietarios = data; 
                paginaActual = 1;
                mostrarPagina();
                mostrarPaginacion();
                console.log("Propietarios cargados:", data);
            })
            .catch((error) =>
                console.error("Error al obtener datos de la API:", error)
            );
    }

    obtenerPropietarios();

    // --- EVENTOS PARA EDITAR Y ELIMINAR ---
    tabla.addEventListener("click", (event) => {
        const target = event.target;
        const id = target.dataset.id;

        // 🔹 BOTÓN EDITAR
        if (target.classList.contains("btn-editar")) {
            window.location.href = `editar-propietario.html?id=${id}`;
            return;
        }

        // 🔹 BOTÓN ELIMINAR
        if (target.classList.contains("btn-borrar")) {
            const confirmacion = confirm(
                "¿Estás seguro de eliminar este propietario?"
            );

            if (confirmacion) {
                fetch(`${API_URL}/${id}`, {
                    method: "DELETE",
                })
                    .then((response) => {
                        if (response.status === 204) {
                            alert("Propietario eliminado correctamente.");
                            obtenerPropietarios();
                        } else if (response.status === 404) {
                            alert("Error: Propietario no encontrado.");
                        } else {
                            throw new Error(`Error al eliminar. Código: ${response.status}`);
                        }
                    })
                    .catch((error) => console.error("Error al eliminar propietario:", error));
            }
        }
    });
});
