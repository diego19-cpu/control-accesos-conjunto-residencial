using control_de_accesos__api.data2;
using control_de_accesos__api.Models;
using control_de_accesos_api.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace control_de_accesos__api.Controllers
{
    [RoutePrefix("api/vigilante")]
    public class VigilanteController : ApiController
    {
        // GET api/vigilante
        // This method returns the complete list of vigilantes.
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAllVigilantes()
        {
            try
            {
                // Call the static method that retrieves the list of vigilantes
                List<Vigilante> vigilantes = VigilanteData.ListarVigilantes();

                // If the list is empty or null, we return an empty response with status 200 OK
                if (vigilantes == null || !vigilantes.Any())
                {
                    return Ok(new List<Vigilante>());
                }

                // If vigilantes are found, return them with status 200 OK
                return Ok(vigilantes);
            }
            catch (System.Exception ex)
            {
                // If an error occurs, return a 500 with the error message
                return InternalServerError(ex);
            }
        }

        // GET api/vigilante/{id}
        // This method will return a specific vigilante by their ID.
        [HttpGet]
        [Route("{id}", Name = "GetVigilanteById")]
        public IHttpActionResult GetVigilanteById(int id)
        {
            try
            {
                // Call the static method to get a vigilante by their ID
                Vigilante vigilante = VigilanteData.ConsultarVigilantePorId(id);

                // If the vigilante is not found, return a 404 Not Found
                if (vigilante == null)
                {
                    return NotFound();
                }

                // If the vigilante is found, return them with status 200 OK
                return Ok(vigilante);
            }
            catch (System.Exception ex)
            {
                // In case of an error, return a 500 with the error message
                return InternalServerError(ex);
            }
        }

        // POST api/vigilante
        // This method is used to register a new vigilante.
        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateVigilante([FromBody] Vigilante nuevoVigilante)
        {
            try
            {
                if (nuevoVigilante == null)
                {
                    return BadRequest("The provided vigilante is not valid.");
                }

                // Call the static method to register a new vigilante
                bool resultado = VigilanteData.RegistrarVigilante(nuevoVigilante);

                if (!resultado)
                {
                    return InternalServerError(); // If an error occurs, return a 500
                }

                // If the vigilante is registered correctly, return a 201 Created
                return CreatedAtRoute("GetVigilanteById", new { id = nuevoVigilante.IdVigilante }, nuevoVigilante);
            }
            catch (System.Exception ex)
            {
                // In case of an error, return a 500 with the error message
                return InternalServerError(ex);
            }
        }

        // PUT api/vigilante/{id}
        // This method updates the information of an existing vigilante.
        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult UpdateVigilante(int id, [FromBody] Vigilante vigilanteActualizado)
        {
            try
            {
                if (vigilanteActualizado == null || vigilanteActualizado.IdVigilante != id)
                {
                    return BadRequest("The provided data is not valid.");
                }

                // Call the static method to update the vigilante
                bool resultado = VigilanteData.ActualizarVigilante(vigilanteActualizado);

                if (!resultado)
                {
                    return InternalServerError(); // If there's an update error
                }

                // If the update was successful, return the updated vigilante with status 200 OK
                return Ok(vigilanteActualizado);
            }
            catch (System.Exception ex)
            {
                // If an error occurs, return a 500 with the error message
                return InternalServerError(ex);
            }
        }

        // DELETE api/vigilante/{id}
        // This method deletes a vigilante by their ID.
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteVigilante(int id)
        {
            try
            {
                // Call the static method to delete a vigilante
                bool resultado = VigilanteData.EliminarVigilante(id);

                if (!resultado)
                {
                    return NotFound(); // If the vigilante is not found, return a 404
                }

                // If the vigilante was successfully deleted, return a 204 No Content
                return StatusCode(System.Net.HttpStatusCode.NoContent);
            }
            catch (System.Exception ex)
            {
                // If an error occurs, return a 500 with the error message
                return InternalServerError(ex);
            }
        }
    }
}