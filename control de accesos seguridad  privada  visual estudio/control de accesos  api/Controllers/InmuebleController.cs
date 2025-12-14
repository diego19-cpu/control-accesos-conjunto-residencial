using control_de_accesos__api.data2;
using control_de_accesos__api.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace control_de_accesos__api.Controllers
{
    // Define el prefijo de ruta para todos los métodos en este controlador.
    [RoutePrefix("api/inmueble")]
    public class InmuebleController : ApiController
    {
        // GET api/inmueble
        // Este método devolverá la lista completa de inmuebles.
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAllInmuebles()
        {
            try
            {
                // Llama al método estático que trae la lista de inmuebles de la capa de datos.
                List<Inmueble> inmuebles = InmuebleData.ListarInmuebles();

                // Si la lista está vacía o es nula, devuelve una respuesta vacía con estado 200 OK.
                if (inmuebles == null || !inmuebles.Any())
                {
                    return Ok(new List<Inmueble>());
                }

                // Si se encuentran inmuebles, devuélvelos con estado 200 OK.
                return Ok(inmuebles);
            }
            catch (System.Exception ex)
            {
                // Si ocurre un error, devuelve un 500 con el mensaje de error.
                return InternalServerError(ex);
            }
        }

        // GET api/inmueble/{id}
        // Este método devolverá un inmueble específico por su ID.
        [HttpGet]
        [Route("{id}", Name = "GetInmuebleById")]
        public IHttpActionResult GetInmuebleById(int id)
        {
            try
            {
                // Llama al método estático para consultar un inmueble por su ID.
                Inmueble inmueble = InmuebleData.ConsultarInmueblePorId(id);

                // Si no se encuentra el inmueble, devuelve un 404 Not Found.
                if (inmueble == null)
                {
                    return NotFound();
                }

                // Si el inmueble es encontrado, devuélvelo con estado 200 OK.
                return Ok(inmueble);
            }
            catch (System.Exception ex)
            {
                // En caso de error, devuelve un 500 con el mensaje de error.
                return InternalServerError(ex);
            }
        }

        // POST api/inmueble
        // Este método se usa para registrar un nuevo inmueble.
        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateInmueble([FromBody] Inmueble nuevoInmueble)
        {
            try
            {
                if (nuevoInmueble == null)
                {
                    return BadRequest("El inmueble proporcionado no es válido.");
                }

                // Llama al método estático para registrar un nuevo inmueble.
                bool resultado = InmuebleData.RegistrarInmueble(nuevoInmueble);

                if (!resultado)
                {
                    return InternalServerError(); // Si ocurre un error, devolvemos un 500.
                }

                // Si el inmueble se registra correctamente, devuelve un 201 Created.
                return CreatedAtRoute("GetInmuebleById", new { id = nuevoInmueble.IdInmueble }, nuevoInmueble);
            }
            catch (System.Exception ex)
            {
                // En caso de error, devuelve un 500 con el mensaje de error.
                return InternalServerError(ex);
            }
        }

        // PUT api/inmueble/{id}
        // Este método actualiza la información de un inmueble existente.
        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult UpdateInmueble(int id, [FromBody] Inmueble inmuebleActualizado)
        {
            try
            {
                if (inmuebleActualizado == null || inmuebleActualizado.IdInmueble != id)
                {
                    return BadRequest("Los datos proporcionados no son válidos.");
                }

                // Llama al método estático para actualizar el inmueble.
                bool resultado = InmuebleData.ActualizarInmueble(inmuebleActualizado);

                if (!resultado)
                {
                    return InternalServerError(); // Si hay un error en la actualización.
                }

                // Si la actualización fue exitosa, devuelve el inmueble actualizado con estado 200 OK.
                return Ok(inmuebleActualizado);
            }
            catch (System.Exception ex)
            {
                // Si ocurre un error, devuelve un 500 con el mensaje de error.
                return InternalServerError(ex);
            }
        }

        // DELETE api/inmueble/{id}
        // Este método elimina un inmueble por su ID.
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteInmueble(int id)
        {
            try
            {
                // Llama al método estático para eliminar un inmueble.
                bool resultado = InmuebleData.EliminarInmueble(id);

                if (!resultado)
                {
                    return NotFound(); // Si no se encuentra el inmueble, devuelve un 404.
                }

                // Si el inmueble fue eliminado correctamente, devuelve un 204 No Content.
                return StatusCode(System.Net.HttpStatusCode.NoContent);
            }
            catch (System.Exception ex)
            {
                // Si ocurre un error, devuelve un 500 con el mensaje de error.
                return InternalServerError(ex);
            }
        }
    }
}