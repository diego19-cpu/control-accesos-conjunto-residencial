document.addEventListener("DOMContentLoaded", () => {
    // La tabla donde se inyectarán los datos
    const tabla = document.getElementById("cuerpoTablaInmuebles");
    const contenedorPaginacion = document.getElementById("paginacionInmuebles");
    const API_URL = "https://localhost:44331/api/Inmueble";

    // ----- variables para paginación -----
    const registrosPorPagina = 5; 
    let paginaActual = 1;
    let inmuebles = []; // aquí se guardan todos los datos

    // --- FUNCIÓN PARA DIBUJAR LA TABLA SEGÚN LA PÁGINA ---
    function mostrarPagina() {
        tabla.innerHTML = "";

        const inicio = (paginaActual - 1) * registrosPorPagina;
        const fin = inicio + registrosPorPagina;

        const datosPagina = inmuebles.slice(inicio, fin);

        datosPagina.forEach((inm) => {
            const row = document.createElement("tr");
            row.innerHTML = `
                <td class="text-center">${inm.IdInmueble}</td>
                <td class="text-center">${inm.NumeroTorre}</td>
                <td class="text-center">${inm.Piso}</td>
                <td class="text-center">${inm.Apartamento}</td>
                <td class="text-center">${inm.IdPropietario}</td>

                <td class="text-center">
                    <button data-id="${inm.IdInmueble}" class="btn btn-warning btn-editar">
                        Editar
                    </button>
                </td>

                <td class="text-center">
                    <button data-id="${inm.IdInmueble}" class="btn btn-danger btn-borrar">
                        Eliminar
                    </button>
                </td>
            `;
            tabla.appendChild(row);
        });
    }

    // --- FUNCIÓN PARA MOSTRAR PAGINACIÓN ---
    function mostrarPaginacion() {
        const totalPaginas = Math.max(1, Math.ceil(inmuebles.length / registrosPorPagina));

        contenedorPaginacion.innerHTML = `
            <button id="btnAnteriorInm" class="btn btn-outline-primary me-2" ${paginaActual === 1 ? "disabled" : ""}>
                « Anterior
            </button>
            <span> Página ${paginaActual} de ${totalPaginas} </span>
            <button id="btnSiguienteInm" class="btn btn-outline-primary ms-2" ${paginaActual === totalPaginas ? "disabled" : ""}>
                Siguiente »
            </button>
        `;

        document.getElementById("btnAnteriorInm").addEventListener("click", () => {
            if (paginaActual > 1) {
                paginaActual--;
                mostrarPagina();
                mostrarPaginacion();
            }
        });

        document.getElementById("btnSiguienteInm").addEventListener("click", () => {
            const totalPaginas = Math.ceil(inmuebles.length / registrosPorPagina);
            if (paginaActual < totalPaginas) {
                paginaActual++;
                mostrarPagina();
                mostrarPaginacion();
            }
        });
    }

    // --- FUNCIÓN PARA OBTENER LOS INMUEBLES (GET) ---
    function obtenerInmuebles() {
        fetch(API_URL)
            .then((response) => response.json())
            .then((data) => {
                inmuebles = data; 
                paginaActual = 1;
                mostrarPagina();
                mostrarPaginacion();
                console.log("Inmuebles cargados:", data);
            })
            .catch((error) =>
                console.error("Error al obtener datos de la API:", error)
            );
    }

    obtenerInmuebles();

    // --- EVENTOS PARA EDITAR Y ELIMINAR ---
    tabla.addEventListener("click", (event) => {
        const target = event.target;
        const id = target.dataset.id;

        // 🔹 BOTÓN EDITAR
        if (target.classList.contains("btn-editar")) {
            // ANTES: window.location.href = `inmueble.html?id=${id}`;
            // AHORA: igual que propietarios, apunta a la página de edición
            window.location.href = `editar-inmueble.html?id=${id}`;
            return;
        }

        // 🔹 BOTÓN ELIMINAR
        if (target.classList.contains("btn-borrar")) {
            const confirmacion = confirm(
                "¿Estás seguro de eliminar este inmueble?"
            );

            if (confirmacion) {
                fetch(`${API_URL}/${id}`, {
                    method: "DELETE",
                })
                    .then((response) => {
                        if (response.status === 204) {
                            alert("Inmueble eliminado correctamente.");
                            obtenerInmuebles();
                        } else if (response.status === 404) {
                            alert("Error: Inmueble no encontrado.");
                        } else {
                            throw new Error(`Error al eliminar. Código: ${response.status}`);
                        }
                    })
                    .catch((error) => console.error("Error al eliminar inmueble:", error));
            }
        }
    });
});



