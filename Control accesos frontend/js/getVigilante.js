document.addEventListener("DOMContentLoaded", () => {
    const tabla = document.getElementById("cuerpoTablaVigilantes");
    const contenedorPaginacion = document.getElementById("paginacionVigilantes");
    const API_URL = "https://localhost:44331/api/Vigilante";

    const registrosPorPagina = 5;
    let paginaActual = 1;
    let vigilantes = [];

    function mostrarPagina() {
        tabla.innerHTML = "";

        const inicio = (paginaActual - 1) * registrosPorPagina;
        const fin = inicio + registrosPorPagina;

        const datosPagina = vigilantes.slice(inicio, fin);

        datosPagina.forEach((v) => {
            const row = document.createElement("tr");
            row.innerHTML = `
                <td class="text-center">${v.IdVigilante}</td>
                <td class="text-center">${v.Nombre}</td>
                <td class="text-center">${v.Apellido}</td>
                <td class="text-center">${v.IdTurno}</td>
                <td class="text-center">${v.IdAdministrador}</td>
                <td class="text-center">
                    <button data-id="${v.IdVigilante}" class="btn btn-warning btn-editar">
                        Editar
                    </button>
                </td>
                <td class="text-center">
                    <button data-id="${v.IdVigilante}" class="btn btn-danger btn-borrar">
                        Eliminar
                    </button>
                </td>
            `;
            tabla.appendChild(row);
        });
    }

    function mostrarPaginacion() {
        const totalPaginas = Math.max(1, Math.ceil(vigilantes.length / registrosPorPagina));

        contenedorPaginacion.innerHTML = `
            <button id="btnAnteriorVig" class="btn btn-outline-primary me-2" ${paginaActual === 1 ? "disabled" : ""}>
                « Anterior
            </button>
            <span> Página ${paginaActual} de ${totalPaginas} </span>
            <button id="btnSiguienteVig" class="btn btn-outline-primary ms-2" ${paginaActual === totalPaginas ? "disabled" : ""}>
                Siguiente »
            </button>
        `;

        const btnAnterior = document.getElementById("btnAnteriorVig");
        const btnSiguiente = document.getElementById("btnSiguienteVig");

        btnAnterior.addEventListener("click", () => {
            if (paginaActual > 1) {
                paginaActual--;
                mostrarPagina();
                mostrarPaginacion();
            }
        });

        btnSiguiente.addEventListener("click", () => {
            const totalPaginas = Math.ceil(vigilantes.length / registrosPorPagina);
            if (paginaActual < totalPaginas) {
                paginaActual++;
                mostrarPagina();
                mostrarPaginacion();
            }
        });
    }

    function obtenerVigilantes() {
        fetch(API_URL)
            .then((response) => response.json())
            .then((data) => {
                vigilantes = data;
                paginaActual = 1;
                mostrarPagina();
                mostrarPaginacion();
                console.log("Vigilantes:", data);
            })
            .catch((error) =>
                console.error("Error al obtener datos de la API de Vigilantes:", error)
            );
    }

    obtenerVigilantes();

    // Eventos Editar / Eliminar
    tabla.addEventListener("click", (event) => {
        const target = event.target;
        const id = target.dataset.id;

        if (target.classList.contains("btn-editar")) {
            window.location.href = `editar-vigilante.html?id=${id}`;
            return;
        }

        if (target.classList.contains("btn-borrar")) {
            const confirmacion = confirm(
                "¿Estás seguro de que deseas eliminar este vigilante?"
            );

            if (confirmacion) {
                fetch(`${API_URL}/${id}`, {
                    method: "DELETE",
                })
                    .then((response) => {
                        if (response.status === 204) {
                            alert("Vigilante eliminado correctamente.");
                            obtenerVigilantes();
                        } else if (response.status === 404) {
                            alert("Error: Vigilante no encontrado.");
                        } else {
                            throw new Error(`Error al eliminar el vigilante. Código: ${response.status}`);
                        }
                    })
                    .catch((error) => console.error("Error al eliminar vigilante:", error));
            }
        }
    });
});
