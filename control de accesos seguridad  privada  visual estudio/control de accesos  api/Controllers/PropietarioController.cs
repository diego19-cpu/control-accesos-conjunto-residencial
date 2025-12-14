using control_de_accesos__api.data2;
using control_de_accesos__api.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace control_de_accesos__api.Controllers
{
    // Esta clase expone endpoints de la API para la gestión de propietarios.
    [RoutePrefix("api/propietario")]
    public class PropietarioController : ApiController
    {
        // GET api/propietario
        // Este método devolverá la lista completa de propietarios.
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAllPropietarios()
        {
            try
            {
                // Llamada al método estático que trae la lista de propietarios.
                List<Propietario> propietarios = PropietarioData.ListarPropietarios();

                // Si la lista está vacía o nula, devolvemos una respuesta vacía con estado 200 OK.
                if (propietarios == null || !propietarios.Any())
                {
                    return Ok(new List<Propietario>());
                }

                // Si se encuentran propietarios, devolverlos con estado 200 OK.
                return Ok(propietarios);
            }
            catch (System.Exception ex)
            {
                // Si ocurre un error, devolvemos un 500 con el mensaje de error.
                return InternalServerError(ex);
            }
        }

        // GET api/propietario/{id}
        // Este método devolverá un propietario específico por su ID.
        [HttpGet]
        [Route("{id}", Name = "GetPropietarioById")]
        public IHttpActionResult GetPropietarioById(int id)
        {
            try
            {
                // Llamada al método estático para consultar un propietario por su ID.
                Propietario propietario = PropietarioData.ConsultarPropietarioPorId(id);

                // Si no se encuentra el propietario, devolver un 404 Not Found.
                if (propietario == null)
                {
                    return NotFound();
                }

                // Si el propietario es encontrado, devolverlo con estado 200 OK.
                return Ok(propietario);
            }
            catch (System.Exception ex)
            {
                // En caso de error, devolver un 500 con el mensaje de error.
                return InternalServerError(ex);
            }
        }

        // POST api/propietario
        // Este método se usa para registrar un nuevo propietario.
        [HttpPost]
        [Route("")]
        public IHttpActionResult CreatePropietario([FromBody] Propietario nuevoPropietario)
        {
            try
            {
                if (nuevoPropietario == null)
                {
                    return BadRequest("El propietario proporcionado no es válido.");
                }

                // Llamada al método estático para registrar un nuevo propietario.
                bool resultado = PropietarioData.RegistrarPropietario(nuevoPropietario);

                if (!resultado)
                {
                    return InternalServerError(); // Si ocurre un error, devolvemos un 500.
                }

                // Si el propietario es registrado correctamente, devolvemos un 201 Created.
                return CreatedAtRoute("GetPropietarioById", new { id = nuevoPropietario.IdPropietario }, nuevoPropietario);
            }
            catch (System.Exception ex)
            {
                // En caso de error, devolver un 500 con el mensaje de error.
                return InternalServerError(ex);
            }
        }

        // PUT api/propietario/{id}
        // Este método actualiza la información de un propietario existente.
        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult UpdatePropietario(int id, [FromBody] Propietario propietarioActualizado)
        {
            try
            {
                if (propietarioActualizado == null || propietarioActualizado.IdPropietario != id)
                {
                    return BadRequest("Los datos proporcionados no son válidos.");
                }

                // Llamada al método estático para actualizar el propietario.
                bool resultado = PropietarioData.ActualizarPropietario(propietarioActualizado);

                if (!resultado)
                {
                    return InternalServerError(); // Si hay un error en la actualización.
                }

                // Si la actualización fue exitosa, devolver el propietario actualizado con estado 200 OK.
                return Ok(propietarioActualizado);
            }
            catch (System.Exception ex)
            {
                // Si ocurre un error, devolver un 500 con el mensaje de error.
                return InternalServerError(ex);
            }
        }

        // DELETE api/propietario/{id}
        // Este método elimina un propietario por su ID.
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeletePropietario(int id)
        {
            try
            {
                // Llamada al método estático para eliminar un propietario.
                bool resultado = PropietarioData.EliminarPropietario(id);

                if (!resultado)
                {
                    return NotFound(); // Si no se encuentra el propietario, devolver un 404.
                }

                // Si el propietario fue eliminado correctamente, devolver un 204 No Content.
                return StatusCode(System.Net.HttpStatusCode.NoContent);
            }
            catch (System.Exception ex)
            {
                // Si ocurre un error, devolver un 500 con el mensaje de error.
                return InternalServerError(ex);
            }
        }
    }
}