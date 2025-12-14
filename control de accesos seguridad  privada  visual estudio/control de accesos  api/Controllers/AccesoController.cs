using control_de_accesos__api.data2;
using control_de_accesos__api.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Net;

namespace control_de_accesos__api.Controllers
{
    [RoutePrefix("api/acceso")]
    public class AccesoController : ApiController
    {
        // GET api/acceso
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAllAccesos()
        {
            try
            {
                // Llama al método estático para obtener la lista de accesos
                List<Acceso> accesos = AccesoData.ListarAccesos();

                // Devuelve Ok con la lista de accesos (puede ser una lista vacía si no hay registros)
                return Ok(accesos);
            }
            catch (System.Exception ex)
            {
                // Maneja cualquier excepción interna y devuelve un Error 500
                return InternalServerError(ex);
            }
        }

        // GET api/acceso/{id}
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetAccesoById(int id)
        {
            try
            {
                // Llama al método estático para buscar un acceso por su ID
                Acceso acceso = AccesoData.ConsultarAccesoPorId(id);

                if (acceso == null)
                {
                    // Si no se encuentra el acceso, devuelve un 404 Not Found
                    return NotFound();
                }

                // Si el acceso se encuentra, devuelve un 200 Ok con el objeto
                return Ok(acceso);
            }
            catch (System.Exception ex)
            {
                // Maneja cualquier excepción interna y devuelve un Error 500
                return InternalServerError(ex);
            }
        }

        // POST api/acceso
        // Este método se usa para registrar un nuevo acceso.
        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateAcceso([FromBody] Acceso nuevoAcceso)
        {
            try
            {
                if (nuevoAcceso == null)
                {
                    // Si el objeto es nulo, devuelve un 400 Bad Request
                    return BadRequest("El registro de acceso proporcionado no es válido.");
                }

                // Llama al método de la clase de datos. El método ahora devuelve 'bool'.
                bool resultado = AccesoData.RegistrarAcceso(nuevoAcceso);

                if (!resultado)
                {
                    // Si el registro falla, devuelve un 500 Internal Server Error
                    return InternalServerError();
                }

                // Si el acceso es registrado correctamente, devuelve un 200 OK
                // y una copia del objeto que se acaba de crear
                return Ok(nuevoAcceso);
            }
            catch (System.Exception ex)
            {
                // En caso de error, devolver un 500 con el mensaje de error
                return InternalServerError(ex);
            }
        }

        // PUT api/acceso/{id}
        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult UpdateAcceso(int id, [FromBody] Acceso accesoActualizado)
        {
            try
            {
                if (accesoActualizado == null || accesoActualizado.IdAcceso != id)
                {
                    // Si los datos no son válidos o el ID no coincide, devuelve un 400
                    return BadRequest("Los datos proporcionados no son válidos.");
                }

                // Llama al método de la clase de datos para actualizar
                bool resultado = AccesoData.ActualizarAcceso(accesoActualizado);

                if (!resultado)
                {
                    // Si la actualización falla, devuelve un 500 Internal Server Error
                    return InternalServerError();
                }

                // Si la actualización es exitosa, devuelve un 200 OK con el objeto actualizado
                return Ok(accesoActualizado);
            }
            catch (System.Exception ex)
            {
                // Maneja cualquier excepción interna y devuelve un Error 500
                return InternalServerError(ex);
            }
        }

        // DELETE api/acceso/{id}
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteAcceso(int id)
        {
            try
            {
                // Llama al método de la clase de datos para eliminar
                bool resultado = AccesoData.EliminarAcceso(id);

                if (!resultado)
                {
                    // Si el registro no existe o la eliminación falla, devuelve un 404 Not Found
                    return NotFound();
                }

                // Si la eliminación es exitosa, devuelve un 204 No Content
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (System.Exception ex)
            {
                // Maneja cualquier excepción interna y devuelve un Error 500
                return InternalServerError(ex);
            }
        }
    }
}