using System;
using System.Collections.Generic;
using System.Data; // Necesario para DataTable o DataSet
using System.Linq;
using System.Web;
using control_de_accesos__api.Models; // Asegúrate de que este namespace sea correcto
using control_de_accesos__api.data2; // Asegúrate de que este namespace sea correcto

namespace control_de_accesos__api.data2
{
    public class PersonalData
    {
        // 1. Método para Registrar un nuevo Personal
        public static bool RegistrarPersonal(Personal oPersonal)
        {
            ConeccionbdClass1 objCnx = new ConeccionbdClass1();
            string sentencia;

            sentencia = "EXECUTE RegistrarPersonal " +
                        "'" + oPersonal.IdPersonal + "'," +
                        "'" + oPersonal.Nombre + "'," +
                        "'" + oPersonal.Apellido + "'," +
                        "'" + oPersonal.DocumentoIdentidad + "'," +
                        "'" + oPersonal.TipoPersona + "'," +
                        "'" + oPersonal.IdInmueble + "'";

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

        // 2. Método para Listar todo el personal
        public static List<Personal> ListarPersonal()
        {
            List<Personal> listaPersonal = new List<Personal>();
            ConeccionbdClass1 objCnx = new ConeccionbdClass1();
            string sentencia = "EXECUTE ListarPersonal";

            if (objCnx.Consultar(sentencia, false))
            {
                while (objCnx.Reader.Read())
                {
                    Personal oPersonal = new Personal();
                    oPersonal.IdPersonal = Convert.ToInt32(objCnx.Reader["id_personal"]);
                    oPersonal.Nombre = objCnx.Reader["nombre"].ToString();
                    oPersonal.Apellido = objCnx.Reader["apellido"].ToString();
                    oPersonal.DocumentoIdentidad = objCnx.Reader["documento_identidad"].ToString();
                    
                    oPersonal.TipoPersona = objCnx.Reader["tipo_persona"].ToString();
                    oPersonal.IdInmueble = Convert.ToInt32(objCnx.Reader["id_inmueble"]);
                    listaPersonal.Add(oPersonal);
                }
            }
            if (objCnx.Reader != null && !objCnx.Reader.IsClosed)
            {
                objCnx.Reader.Close();
            }
            objCnx.CerrarConexion();
            return listaPersonal;
        }

        // 3. Método para Actualizar un registro de personal existente
        public static bool ActualizarPersonal(Personal oPersonal)
        {
            ConeccionbdClass1 objCnx = new ConeccionbdClass1();
            string sentencia;

            sentencia = "EXECUTE ActualizarPersonal " +
                        "'" + oPersonal.IdPersonal + "'," +
                        "'" + oPersonal.Nombre + "'," +
                        "'" + oPersonal.Apellido + "'," +
                        "'" + oPersonal.DocumentoIdentidad + "'," +
                        "'" + oPersonal.TipoPersona + "'," +
                        "'" + oPersonal.IdInmueble + "'";

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

        // 4. Método para Eliminar un registro de personal por su ID
        public static bool EliminarPersonal(int idPersonal)
        {
            ConeccionbdClass1 objCnx = new ConeccionbdClass1();
            string sentencia;

            sentencia = "EXECUTE EliminarPersonal '" + idPersonal + "'";

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

        // 5. Método para Consultar (obtener) un registro de personal por su ID
        public static Personal ConsultarPersonalPorId(int idPersonal)
        {
            Personal oPersonal = null;
            ConeccionbdClass1 objCnx = new ConeccionbdClass1();
            string sentencia;

            sentencia = "EXECUTE ConsultarPersonal '" + idPersonal + "'";

            if (objCnx.Consultar(sentencia, false))
            {
                if (objCnx.Reader.Read())
                {
                    oPersonal = new Personal();
                    oPersonal.IdPersonal = Convert.ToInt32(objCnx.Reader["id_personal"]);
                    oPersonal.Nombre = objCnx.Reader["nombre"].ToString();
                    oPersonal.Apellido = objCnx.Reader["apellido"].ToString();
                    oPersonal.DocumentoIdentidad = objCnx.Reader["documento_identidad"].ToString();
                    oPersonal.TipoPersona = objCnx.Reader["tipo_persona"].ToString();
                    oPersonal.IdInmueble = Convert.ToInt32(objCnx.Reader["id_inmueble"]);
                }
            }
            if (objCnx.Reader != null && !objCnx.Reader.IsClosed)
            {
                objCnx.Reader.Close();
            }
            objCnx.CerrarConexion();
            return oPersonal;
        }
    }
}