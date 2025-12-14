using control_de_accesos__api.data2;
// Asegúrate de que estos namespaces sean correctos para tu proyecto
using control_de_accesos__api.Models;
using control_de_accesos_api.Models;
using System;
using System.Collections.Generic;
using System.Configuration; // Para la cadena de conexión
using System.Data; // Necesario para DataTable o DataSet
using System.Data.SqlClient; // Para la comunicación con SQL Server
using System.Linq;
using System.Web;

namespace control_de_accesos__api.data2
{
    // AVISO DE SEGURIDAD: Este enfoque de concatenación de strings es MUY VULNERABLE a la inyección SQL.
    // Para un proyecto serio, se recomienda encarecidamente usar parámetros en los SqlCommand.

    public class VigilanteData
    {
        // 1. Método para Registrar un nuevo vigilante
        public static bool RegistrarVigilante(Vigilante oVigilante)
        {
            ConeccionbdClass1 objCnx = new ConeccionbdClass1();
            string sentencia;

            sentencia = "EXECUTE RegistrarVigilante " +
                        "'" + oVigilante.IdVigilante + "'," +
                        "'" + oVigilante.Nombre + "'," +
                        "'" + oVigilante.Apellido + "'," +
                        "'" + oVigilante.IdTurno + "'," +
                        "'" + oVigilante.IdAdministrador + "'";

            if (!objCnx.EjecutarSentencia(sentencia, false))
            {
                objCnx.CerrarConexion();
                return false;
            }
            else
            {
                objCnx.CerrarConexion();
                return true;
            }
        }

        // 2. Método para Listar todos los vigilantes
        public static List<Vigilante> ListarVigilantes()
        {
            List<Vigilante> listaVigilantes = new List<Vigilante>();
            ConeccionbdClass1 objCnx = new ConeccionbdClass1();
            string sentencia = "EXECUTE ListarVigilantes";

            if (objCnx.Consultar(sentencia, false))
            {
                while (objCnx.Reader.Read())
                {
                    Vigilante oVigilante = new Vigilante();
                    oVigilante.IdVigilante = Convert.ToInt32(objCnx.Reader["id_vigilante"]);
                    oVigilante.Nombre = objCnx.Reader["nombre"].ToString();
                    oVigilante.Apellido = objCnx.Reader["apellido"].ToString();
                    oVigilante.IdTurno = Convert.ToInt32(objCnx.Reader["id_turno"]);
                    oVigilante.IdAdministrador = Convert.ToInt32(objCnx.Reader["id_administrador"]);
                    listaVigilantes.Add(oVigilante);
                }
            }
            objCnx.CerrarConexion();
            return listaVigilantes;
        }

        // 3. Método para Actualizar un vigilante existente
        public static bool ActualizarVigilante(Vigilante oVigilante)
        {
            ConeccionbdClass1 objCnx = new ConeccionbdClass1();
            string sentencia;

            sentencia = "EXECUTE ActualizarVigilante " +
                        "'" + oVigilante.IdVigilante + "'," +
                        "'" + oVigilante.Nombre + "'," +
                        "'" + oVigilante.Apellido + "'," +
                        "'" + oVigilante.IdTurno + "'," +
                        "'" + oVigilante.IdAdministrador + "'";

            if (!objCnx.EjecutarSentencia(sentencia, false))
            {
                objCnx.CerrarConexion();
                return false;
            }
            else
            {
                objCnx.CerrarConexion();
                return true;
            }
        }

        // 4. Método para Eliminar un vigilante por su ID
        public static bool EliminarVigilante(int idVigilante)
        {
            ConeccionbdClass1 objCnx = new ConeccionbdClass1();
            string sentencia;

            sentencia = "EXECUTE EliminarVigilante '" + idVigilante + "'";

            if (!objCnx.EjecutarSentencia(sentencia, false))
            {
                objCnx.CerrarConexion();
                return false;
            }
            else
            {
                objCnx.CerrarConexion();
                return true;
            }
        }

        // 5. Método para Consultar (obtener) un vigilante por su ID
        public static Vigilante ConsultarVigilantePorId(int idVigilante)
        {
            Vigilante oVigilante = null;
            ConeccionbdClass1 objCnx = new ConeccionbdClass1();
            string sentencia;

            sentencia = "EXECUTE ConsultarVigilante '" + idVigilante + "'";

            if (objCnx.Consultar(sentencia, false))
            {
                if (objCnx.Reader.Read())
                {
                    oVigilante = new Vigilante();
                    oVigilante.IdVigilante = Convert.ToInt32(objCnx.Reader["id_vigilante"]);
                    oVigilante.Nombre = objCnx.Reader["nombre"].ToString();
                    oVigilante.Apellido = objCnx.Reader["apellido"].ToString();
                    oVigilante.IdTurno = Convert.ToInt32(objCnx.Reader["id_turno"]);
                    oVigilante.IdAdministrador = Convert.ToInt32(objCnx.Reader["id_administrador"]);
                }
            }
            objCnx.CerrarConexion();
            return oVigilante;
        }
    }
}