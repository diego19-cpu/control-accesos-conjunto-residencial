document.addEventListener("DOMContentLoaded", () => { 

    const API_URL = "https://localhost:44331/api/Personal";
    const btnEditar = document.getElementById("btnEditarPersonal"); // usa este id en el HTML

    // Campos del formulario
    const idInput           = document.getElementById("idPersonal");
    const nombreInput       = document.getElementById("nombre");
    const apellidoInput     = document.getElementById("apellido");
    const docIdentidadInput = document.getElementById("documentoIdentidad");
    const tipoPersonaInput  = document.getElementById("tipoPersona");
    const idInmuebleInput   = document.getElementById("idInmueble");

    if (!btnEditar) {
        console.error("No se encontró el botón #btnEditarPersonal");
        return;
    }

    // ================== OBTENER ID DE LA URL Y CARGAR DATOS ==================
    const params = new URLSearchParams(window.location.search);
    const idUrl = params.get("id");   // ej: editar-personal.html?id=3  -> "3"

    if (!idUrl) {
        alert("No se recibió un ID en la URL (ej: editar-personal.html?id=3)");
        return;
    }

    // Hacer GET a la API para rellenar el formulario
    fetch(`${API_URL}/${idUrl}`)
        .then(response => {
            if (!response.ok) {
                throw new Error(`Error al obtener el personal. Código: ${response.status}`);
            }
            return response.json();
        })
        .then(data => {
            // Rellenar los campos
            idInput.value           = data.IdPersonal;
            nombreInput.value       = data.Nombre;
            apellidoInput.value     = data.Apellido;
            docIdentidadInput.value = data.DocumentoIdentidad;
            tipoPersonaInput.value  = data.TipoPersona;
            idInmuebleInput.value   = data.IdInmueble;

            // si quieres que el ID no se edite:
            idInput.readOnly = true;
        })
        .catch(error => {
            console.error("Error al cargar datos del personal:", error);
            alert("No se pudieron cargar los datos del personal.");
        });
    // ================== FIN BLOQUE CARGA ==================


    // --- BOTÓN ACTUALIZAR ---
    btnEditar.addEventListener("click", () => {

        const id                = idInput.value.trim();
        const nombre            = nombreInput.value.trim();
        const apellido          = apellidoInput.value.trim();
        const documento         = docIdentidadInput.value.trim();
        const tipoPersona       = tipoPersonaInput.value.trim();
        const idInmuebleTexto   = idInmuebleInput.value.trim();

        // Validación del ID
        if (!id || isNaN(id)) {
            alert("Por favor, ingrese un ID de personal válido.");
            return;
        }

        // Validación de campos obligatorios
        if (!nombre || !apellido || !documento || !tipoPersona || !idInmuebleTexto) {
            alert("Por favor, complete todos los campos.");
            return;
        }

        // Convertir campos numéricos
        const idInmueble = parseInt(idInmuebleTexto, 10);
        if (isNaN(idInmueble)) {
            alert("El ID del inmueble debe ser un número válido.");
            return;
        }

        // Objeto para enviar al API
        const data = {
            IdPersonal: parseInt(id, 10),
            Nombre: nombre,
            Apellido: apellido,
            DocumentoIdentidad: documento,
            TipoPersona: tipoPersona,
            IdInmueble: idInmueble
        };

        // Llamada PUT a la API
        fetch(`${API_URL}/${id}`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(data),
        })
        .then((response) => {
            if (response.ok) {
                alert("Personal actualizado correctamente.");
                // aquí la corrección: volver al listado correcto
                window.location.href = "personal.html";
            } else {
                console.error("Error al actualizar:", response.status);
                alert(`Error al actualizar personal. Código: ${response.status}`);
            }
        })
        .catch((error) => {
            console.error("Error al realizar la solicitud PUT:", error);
            alert("No se pudo conectar con la API.");
        });

    }); 
});





