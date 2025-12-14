using control_de_accesos__api.data2;
using control_de_accesos__api.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace control_de_accesos__api.Controllers
{
    [RoutePrefix("api/administrador")]
    public class AdministradorController : ApiController
    {
        // GET api/administrador
        // Este método devolverá la lista completa de administradores.
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAllAdministradores()
        {
            try
            {
                // Llamada al método estático que trae la lista de administradores
                List<Administrador> administradores = AdministradorData.ListarAdministradores();

                // Si la lista está vacía o nula, devolvemos una respuesta vacía con estado 200 OK
                if (administradores == null || !administradores.Any())
                {
                    return Ok(new List<Administrador>());
                }

                // Si se encuentran administradores, devolverlos con estado 200 OK
                return Ok(administradores);
            }
            catch (System.Exception ex)
            {
                // Si ocurre un error, devolvemos un 500 con el mensaje de error
                return InternalServerError(ex);
            }
        }

        // GET api/administrador/{id}
        // Este método devolverá un administrador específico por su ID.
        [HttpGet]
        [Route("{id}", Name = "GetAdministradorById")]
        public IHttpActionResult GetAdministradorById(int id)
        {
            try
            {
                // Llamada al método estático para consultar un administrador por su ID
                Administrador administrador = AdministradorData.ConsultarAdministradorPorId(id);

                // Si no se encuentra el administrador, devolver un 404 Not Found
                if (administrador == null)
                {
                    return NotFound();
                }

                // Si el administrador es encontrado, devolverlo con estado 200 OK
                return Ok(administrador);
            }
            catch (System.Exception ex)
            {
                // En caso de error, devolver un 500 con el mensaje de error
                return InternalServerError(ex);
            }
        }

        // POST api/administrador
        // Este método se usa para registrar un nuevo administrador.
        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateAdministrador([FromBody] Administrador nuevoAdministrador)
        {
            try
            {
                if (nuevoAdministrador == null)
                {
                    return BadRequest("El administrador proporcionado no es válido.");
                }

                // Llamada al método estático para registrar un nuevo administrador
                bool resultado = AdministradorData.RegistrarAdministrador(nuevoAdministrador);

                if (!resultado)
                {
                    return InternalServerError(); // Si ocurre un error, devolvemos un 500
                }

                // Si el administrador es registrado correctamente, devolvemos un 201 Created
                return CreatedAtRoute("GetAdministradorById", new { id = nuevoAdministrador.IdAdministrador }, nuevoAdministrador);
            }
            catch (System.Exception ex)
            {
                // En caso de error, devolver un 500 con el mensaje de error
                return InternalServerError(ex);
            }
        }

        // PUT api/administrador/{id}
        // Este método actualiza la información de un administrador existente.
        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult UpdateAdministrador(int id, [FromBody] Administrador administradorActualizado)
        {
            try
            {
                if (administradorActualizado == null || administradorActualizado.IdAdministrador != id)
                {
                    return BadRequest("Los datos proporcionados no son válidos.");
                }

                // Llamada al método estático para actualizar el administrador
                bool resultado = AdministradorData.ActualizarAdministrador(administradorActualizado);

                if (!resultado)
                {
                    return InternalServerError(); // Si hay un error en la actualización
                }

                // Si la actualización fue exitosa, devolver el administrador actualizado con estado 200 OK
                return Ok(administradorActualizado);
            }
            catch (System.Exception ex)
            {
                // Si ocurre un error, devolver un 500 con el mensaje de error
                return InternalServerError(ex);
            }
        }

        // DELETE api/administrador/{id}
        // Este método elimina un administrador por su ID.
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteAdministrador(int id)
        {
            try
            {
                // Llamada al método estático para eliminar un administrador
                bool resultado = AdministradorData.EliminarAdministrador(id);

                if (!resultado)
                {
                    return NotFound(); // Si no se encuentra el administrador, devolver un 404
                }

                // Si el administrador fue eliminado correctamente, devolver un 204 No Content
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