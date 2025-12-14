using control_de_accesos__api.data2;
using control_de_accesos__api.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace control_de_accesos__api.Controllers
{
    [RoutePrefix("api/personal")]
    public class PersonalController : ApiController
    {
        // GET api/personal
        // Este método devolverá la lista completa del personal.
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAllPersonal()
        {
            try
            {
                // Llamada al método estático que trae la lista de personal
                List<Personal> personal = PersonalData.ListarPersonal();

                // Si la lista está vacía o nula, devolvemos una respuesta vacía con estado 200 OK
                if (personal == null || !personal.Any())
                {
                    return Ok(new List<Personal>());
                }

                // Si se encuentra personal, devolverlo con estado 200 OK
                return Ok(personal);
            }
            catch (System.Exception ex)
            {
                // Si ocurre un error, devolvemos un 500 con el mensaje de error
                return InternalServerError(ex);
            }
        }

        // GET api/personal/filtrar/{tipo}
        // Este nuevo método devolverá solo los registros de personal que coincidan con el tipo especificado.
        [HttpGet]
        [Route("filtrar/{tipo}")]
        public IHttpActionResult GetPersonalPorTipo(string tipo)
        {
            try
            {
                // Llamada al método estático que trae la lista de personal completa
                List<Personal> personal = PersonalData.ListarPersonal();

                // Filtramos la lista para obtener solo los registros con el TipoPersona especificado
                List<Personal> personalFiltrado = personal.Where(p => p.TipoPersona.Equals(tipo, System.StringComparison.OrdinalIgnoreCase)).ToList();

                // Si la lista filtrada está vacía, devolvemos una respuesta vacía
                if (personalFiltrado == null || !personalFiltrado.Any())
                {
                    return Ok(new List<Personal>());
                }

                // Devolvemos la lista de personal filtrado con un 200 OK
                return Ok(personalFiltrado);
            }
            catch (System.Exception ex)
            {
                // En caso de error, devolver un 500 con el mensaje de error
                return InternalServerError(ex);
            }
        }

        // GET api/personal/{id}
        // Este método devolverá un registro de personal específico por su ID.
        [HttpGet]
        [Route("{id}", Name = "GetPersonalById")]
        public IHttpActionResult GetPersonalById(int id)
        {
            try
            {
                // Llamada al método estático para consultar un registro de personal por su ID
                Personal persona = PersonalData.ConsultarPersonalPorId(id);

                // Si no se encuentra el registro, devolver un 404 Not Found
                if (persona == null)
                {
                    return NotFound();
                }

                // Si el registro es encontrado, devolverlo con estado 200 OK
                return Ok(persona);
            }
            catch (System.Exception ex)
            {
                // En caso de error, devolver un 500 con el mensaje de error
                return InternalServerError(ex);
            }
        }

        // POST api/personal
        // Este método se usa para registrar un nuevo registro de personal.
        [HttpPost]
        [Route("")]
        public IHttpActionResult CreatePersonal([FromBody] Personal nuevoPersonal)
        {
            try
            {
                if (nuevoPersonal == null)
                {
                    return BadRequest("El registro de personal proporcionado no es válido.");
                }

                // Llamada al método estático para registrar un nuevo registro de personal
                bool resultado = PersonalData.RegistrarPersonal(nuevoPersonal);

                if (!resultado)
                {
                    return InternalServerError(); // Si ocurre un error, devolvemos un 500
                }

                // Si el registro es creado correctamente, devolvemos un 201 Created
                return CreatedAtRoute("GetPersonalById", new { id = nuevoPersonal.IdPersonal }, nuevoPersonal);
            }
            catch (System.Exception ex)
            {
                // En caso de error, devolver un 500 con el mensaje de error
                return InternalServerError(ex);
            }
        }

        // PUT api/personal/{id}
        // Este método actualiza la información de un registro de personal existente.
        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult UpdatePersonal(int id, [FromBody] Personal personalActualizado)
        {
            try
            {
                if (personalActualizado == null || personalActualizado.IdPersonal != id)
                {
                    return BadRequest("Los datos proporcionados no son válidos.");
                }

                // Llamada al método estático para actualizar el registro de personal
                bool resultado = PersonalData.ActualizarPersonal(personalActualizado);

                if (!resultado)
                {
                    return InternalServerError(); // Si hay un error en la actualización
                }

                // Si la actualización fue exitosa, devolver el registro actualizado con estado 200 OK
                return Ok(personalActualizado);
            }
            catch (System.Exception ex)
            {
                // Si ocurre un error, devolver un 500 con el mensaje de error
                return InternalServerError(ex);
            }
        }

        // DELETE api/personal/{id}
        // Este método elimina un registro de personal por su ID.
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeletePersonal(int id)
        {
            try
            {
                // Llamada al método estático para eliminar un registro de personal
                bool resultado = PersonalData.EliminarPersonal(id);

                if (!resultado)
                {
                    return NotFound(); // Si no se encuentra el registro, devolver un 404
                }

                // Si el registro fue eliminado correctamente, devolver un 204 No Content
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
