using System;
using System.Collections.Generic;
using System.Data; // Necesario para DataTable o DataSet
using System.Linq;
using System.Web;
using control_de_accesos__api.Models; // Asegúrate de que este namespace contenga tu clase Turno
using control_de_accesos__api.data2; // Asegúrate de que este namespace contenga tu clase ConeccionbdClass1

namespace control_de_accesos__api.data2
{
    public class TurnoData
    {
        // 1. Método para Registrar un nuevo turno
        // Llama al procedimiento almacenado 'RegistrarTurno'
        public static bool RegistrarTurno(Turno oTurno)
        {
            ConeccionbdClass1 objCnx = new ConeccionbdClass1();
            string sentencia;

            // Construcción de la sentencia SQL para ejecutar el Stored Procedure
            // NOTA: Se recomienda usar SqlParameter para evitar inyección SQL.
            sentencia = "EXECUTE RegistrarTurno " +
                        "'" + oTurno.IdTurno + "'," +
                        "'" + oTurno.AsignacionTurno + "'," +
                        "'" + oTurno.HoraInicio.ToString("hh\\:mm\\:ss") + "'," +
                        "'" + oTurno.HoraFin.ToString("hh\\:mm\\:ss") + "'";

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

        // 2. Método para Listar todos los turnos
        // Llama al procedimiento almacenado 'ListarTurnos'
        public static List<Turno> ListarTurnos()
        {
            List<Turno> listaTurnos = new List<Turno>();
            ConeccionbdClass1 objCnx = new ConeccionbdClass1();
            string sentencia = "EXECUTE ListarTurnos";

            if (objCnx.Consultar(sentencia, false))
            {
                while (objCnx.Reader.Read())
                {
                    // Mapea los datos del SqlDataReader a un objeto Turno
                    Turno oTurno = new Turno();
                    oTurno.IdTurno = Convert.ToInt32(objCnx.Reader["id_turno"]);
                    oTurno.AsignacionTurno = objCnx.Reader["asignacion_turno"].ToString();
                    // Conversión de datos TIME de SQL a TimeSpan en C#
                    oTurno.HoraInicio = (TimeSpan)objCnx.Reader["hora_inicio"];
                    oTurno.HoraFin = (TimeSpan)objCnx.Reader["hora_fin"];
                    listaTurnos.Add(oTurno);
                }
            }
            // Importante: Cerrar la conexión y el lector
            if (objCnx.Reader != null && !objCnx.Reader.IsClosed)
            {
                objCnx.Reader.Close();
            }
            objCnx.CerrarConexion();
            return listaTurnos;
        }

        // 3. Método para Actualizar un turno existente
        // Llama al procedimiento almacenado 'ActualizarTurno'
        public static bool ActualizarTurno(Turno oTurno)
        {
            ConeccionbdClass1 objCnx = new ConeccionbdClass1();
            string sentencia;

            sentencia = "EXECUTE ActualizarTurno " +
                        "'" + oTurno.IdTurno + "'," +
                        "'" + oTurno.AsignacionTurno + "'," +
                        "'" + oTurno.HoraInicio.ToString("hh\\:mm\\:ss") + "'," +
                        "'" + oTurno.HoraFin.ToString("hh\\:mm\\:ss") + "'";

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

        // 4. Método para Eliminar un turno por su ID
        // Llama al procedimiento almacenado 'EliminarTurno'
        public static bool EliminarTurno(int idTurno)
        {
            ConeccionbdClass1 objCnx = new ConeccionbdClass1();
            string sentencia;

            sentencia = "EXECUTE EliminarTurno '" + idTurno + "'";

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

        // 5. Método para Consultar (obtener) un turno por su ID
        // Llama al procedimiento almacenado 'ConsultarTurno'
        public static Turno ConsultarTurnoPorId(int idTurno)
        {
            Turno oTurno = null;
            ConeccionbdClass1 objCnx = new ConeccionbdClass1();
            string sentencia;

            sentencia = "EXECUTE ConsultarTurno '" + idTurno + "'";

            if (objCnx.Consultar(sentencia, false))
            {
                if (objCnx.Reader.Read())
                {
                    oTurno = new Turno();
                    oTurno.IdTurno = Convert.ToInt32(objCnx.Reader["id_turno"]);
                    oTurno.AsignacionTurno = objCnx.Reader["asignacion_turno"].ToString();
                    // Conversión de datos TIME de SQL a TimeSpan en C#
                    oTurno.HoraInicio = (TimeSpan)objCnx.Reader["hora_inicio"];
                    oTurno.HoraFin = (TimeSpan)objCnx.Reader["hora_fin"];
                }
            }
            // Importante: Cerrar la conexión y el lector
            if (objCnx.Reader != null && !objCnx.Reader.IsClosed)
            {
                objCnx.Reader.Close();
            }
            objCnx.CerrarConexion();
            return oTurno;
        }
    }
}