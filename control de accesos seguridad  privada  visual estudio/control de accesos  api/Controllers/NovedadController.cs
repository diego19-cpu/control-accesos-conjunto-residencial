using control_de_accesos_api.data2;
using control_de_accesos_api.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace control_de_accesos_api.Controllers
{
    [RoutePrefix("api/novedad")]
    public class NovedadController : ApiController
    {
        // GET api/novedad
        // Este método devolverá la lista completa de novedades.
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAllNovedades()
        {
            try
            {
                // Llamada al método estático que trae la lista de novedades
                List<Novedad> novedades = NovedadData.ListarNovedades();

                // Si la lista está vacía o nula, devolvemos una respuesta vacía con estado 200 OK
                if (novedades == null || !novedades.Any())
                {
                    return Ok(new List<Novedad>());
                }

                // Si se encuentran novedades, devolverlas con estado 200 OK
                return Ok(novedades);
            }
            catch (System.Exception ex)
            {
                // Si ocurre un error, devolvemos un 500 con el mensaje de error
                return InternalServerError(ex);
            }
        }

        // GET api/novedad/{id}
        // Este método devolverá una novedad específica por su ID.
        [HttpGet]
        [Route("{id}", Name = "GetNovedadById")]
        public IHttpActionResult GetNovedadById(int id)
        {
            try
            {
                // Llamada al método estático para consultar una novedad por su ID
                Novedad novedad = NovedadData.ConsultarNovedadPorId(id);

                // Si no se encuentra la novedad, devolver un 404 Not Found
                if (novedad == null)
                {
                    return NotFound();
                }

                // Si la novedad es encontrada, devolverla con estado 200 OK
                return Ok(novedad);
            }
            catch (System.Exception ex)
            {
                // En caso de error, devolver un 500 con el mensaje de error
                return InternalServerError(ex);
            }
        }

        // POST api/novedad
        // Este método se usa para registrar una nueva novedad.
        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateNovedad([FromBody] Novedad nuevaNovedad)
        {
            try
            {
                if (nuevaNovedad == null)
                {
                    return BadRequest("La novedad proporcionada no es válida.");
                }

                // Llamada al método estático para registrar una nueva novedad
                bool resultado = NovedadData.RegistrarNovedad(nuevaNovedad);

                if (!resultado)
                {
                    return InternalServerError(); // Si ocurre un error, devolvemos un 500
                }

                // Si la novedad es registrada correctamente, devolvemos un 201 Created
                return CreatedAtRoute("GetNovedadById", new { id = nuevaNovedad.IdNovedad }, nuevaNovedad);
            }
            catch (System.Exception ex)
            {
                // En caso de error, devolver un 500 con el mensaje de error
                return InternalServerError(ex);
            }
        }

        // PUT api/novedad/{id}
        // Este método actualiza la información de una novedad existente.
        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult UpdateNovedad(int id, [FromBody] Novedad novedadActualizada)
        {
            try
            {
                if (novedadActualizada == null || novedadActualizada.IdNovedad != id)
                {
                    return BadRequest("Los datos proporcionados no son válidos.");
                }

                // Llamada al método estático para actualizar la novedad
                bool resultado = NovedadData.ActualizarNovedad(novedadActualizada);

                if (!resultado)
                {
                    return InternalServerError(); // Si hay un error en la actualización
                }

                // Si la actualización fue exitosa, devolver la novedad actualizada con estado 200 OK
                return Ok(novedadActualizada);
            }
            catch (System.Exception ex)
            {
                // Si ocurre un error, devolver un 500 con el mensaje de error
                return InternalServerError(ex);
            }
        }

        // DELETE api/novedad/{id}
        // Este método elimina una novedad por su ID.
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteNovedad(int id)
        {
            try
            {
                // Llamada al método estático para eliminar una novedad
                bool resultado = NovedadData.EliminarNovedad(id);

                if (!resultado)
                {
                    return NotFound(); // Si no se encuentra la novedad, devolver un 404
                }

                // Si la novedad fue eliminada correctamente, devolver un 204 No Content
                return StatusCode(System.Net.HttpStatusCode.NoContent);
            }
            catch (System.Exception ex)
            {
                // Si ocurre un error, devolver un 500 con el mensaje de error
                return InternalServerError(ex);
            }
        }
    }
}
