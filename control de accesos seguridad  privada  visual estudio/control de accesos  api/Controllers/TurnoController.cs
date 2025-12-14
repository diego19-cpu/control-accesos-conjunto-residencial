using control_de_accesos__api.data2;
using control_de_accesos__api.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace control_de_accesos__api.Controllers
{
    [RoutePrefix("api/turno")]
    public class TurnoController : ApiController
    {
        // GET api/turno
        // Este método devolverá la lista completa de turnos.
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAllTurnos()
        {
            try
            {
                // Llamada al método estático que trae la lista de turnos
                List<Turno> turnos = TurnoData.ListarTurnos();

                // Si la lista está vacía o nula, devolvemos una respuesta vacía con estado 200 OK
                if (turnos == null || !turnos.Any())
                {
                    return Ok(new List<Turno>());
                }

                // Si se encuentran turnos, devolverlos con estado 200 OK
                return Ok(turnos);
            }
            catch (System.Exception ex)
            {
                // Si ocurre un error, devolvemos un 500 con el mensaje de error
                return InternalServerError(ex);
            }
        }

        // GET api/turno/{id}
        // Este método devolverá un turno específico por su ID.
        [HttpGet]
        [Route("{id}", Name = "GetTurnoById")]
        public IHttpActionResult GetTurnoById(int id)
        {
            try
            {
                // Llamada al método estático para consultar un turno por su ID
                Turno turno = TurnoData.ConsultarTurnoPorId(id);

                // Si no se encuentra el turno, devolver un 404 Not Found
                if (turno == null)
                {
                    return NotFound();
                }

                // Si el turno es encontrado, devolverlo con estado 200 OK
                return Ok(turno);
            }
            catch (System.Exception ex)
            {
                // En caso de error, devolver un 500 con el mensaje de error
                return InternalServerError(ex);
            }
        }

        // POST api/turno
        // Este método se usa para registrar un nuevo turno.
        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateTurno([FromBody] Turno nuevoTurno)
        {
            try
            {
                if (nuevoTurno == null)
                {
                    return BadRequest("El turno proporcionado no es válido.");
                }

                // Llamada al método estático para registrar un nuevo turno
                bool resultado = TurnoData.RegistrarTurno(nuevoTurno);

                if (!resultado)
                {
                    return InternalServerError(); // Si ocurre un error, devolvemos un 500
                }

                // Si el turno es registrado correctamente, devolvemos un 201 Created
                return CreatedAtRoute("GetTurnoById", new { id = nuevoTurno.IdTurno }, nuevoTurno);
            }
            catch (System.Exception ex)
            {
                // En caso de error, devolver un 500 con el mensaje de error
                return InternalServerError(ex);
            }
        }

        // PUT api/turno/{id}
        // Este método actualiza la información de un turno existente.
        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult UpdateTurno(int id, [FromBody] Turno turnoActualizado)
        {
            try
            {
                if (turnoActualizado == null || turnoActualizado.IdTurno != id)
                {
                    return BadRequest("Los datos proporcionados no son válidos.");
                }

                // Llamada al método estático para actualizar el turno
                bool resultado = TurnoData.ActualizarTurno(turnoActualizado);

                if (!resultado)
                {
                    return InternalServerError(); // Si hay un error en la actualización
                }

                // Si la actualización fue exitosa, devolver el turno actualizado con estado 200 OK
                return Ok(turnoActualizado);
            }
            catch (System.Exception ex)
            {
                // Si ocurre un error, devolver un 500 con el mensaje de error
                return InternalServerError(ex);
            }
        }

        // DELETE api/turno/{id}
        // Este método elimina un turno por su ID.
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteTurno(int id)
        {
            try
            {
                // Llamada al método estático para eliminar un turno
                bool resultado = TurnoData.EliminarTurno(id);

                if (!resultado)
                {
                    return NotFound(); // Si no se encuentra el turno, devolver un 404
                }

                // Si el turno fue eliminado correctamente, devolver un 204 No Content
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
