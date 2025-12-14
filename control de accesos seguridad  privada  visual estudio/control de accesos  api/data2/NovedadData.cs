using control_de_accesos__api.data2;
using control_de_accesos_api.data2;
using control_de_accesos_api.Models;
using System;
using System.Collections.Generic;
using System.Data; // Necesario para DataTable o DataSet
using System.Linq;
using System.Web;

namespace control_de_accesos_api.data2
{
    // Esta clase maneja las operaciones CRUD para la entidad Novedad,
    // utilizando procedimientos almacenados, al igual que PropietarioData y AdministradorData.
    public class NovedadData
    {
        // 1. Método para Registrar una nueva novedad
        public static bool RegistrarNovedad(Novedad oNovedad)
        {
            ConeccionbdClass1 objCnx = new ConeccionbdClass1();
            string sentencia;

            // Manejar campos opcionales (que pueden ser nulos)
            string idAdminStr = oNovedad.IdAdministrador.HasValue ? oNovedad.IdAdministrador.Value.ToString() : "NULL";
            string idPersonalStr = oNovedad.IdPersonal.HasValue ? oNovedad.IdPersonal.Value.ToString() : "NULL";

            sentencia = "EXECUTE RegistrarNovedad " +
                        oNovedad.IdNovedad + ", " +
                        oNovedad.IdVigilante + ", " +
                        idAdminStr + ", " +
                        idPersonalStr + ", " +
                        "'" + oNovedad.Descripcion + "', " +
                        "'" + oNovedad.MomentoNovedad.ToString("yyyy-MM-ddTHH:mm:ss") + "'";

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

        // 2. Método para Listar todas las novedades
        public static List<Novedad> ListarNovedades()
        {
            List<Novedad> listaNovedades = new List<Novedad>();
            ConeccionbdClass1 objCnx = new ConeccionbdClass1();
            string sentencia = "EXECUTE ListarNovedades";

            if (objCnx.Consultar(sentencia, false))
            {
                while (objCnx.Reader.Read())
                {
                    Novedad oNovedad = new Novedad();
                    oNovedad.IdNovedad = Convert.ToInt32(objCnx.Reader["id_novedad"]);
                    oNovedad.IdVigilante = Convert.ToInt32(objCnx.Reader["id_vigilante"]);

                    // Manejar campos nulos al leer de la base de datos
                    oNovedad.IdAdministrador = objCnx.Reader["id_administrador"] != DBNull.Value ? (int?)Convert.ToInt32(objCnx.Reader["id_administrador"]) : null;
                    oNovedad.IdPersonal = objCnx.Reader["id_personal"] != DBNull.Value ? (int?)Convert.ToInt32(objCnx.Reader["id_personal"]) : null;

                    oNovedad.Descripcion = objCnx.Reader["descripcion"].ToString();
                    oNovedad.MomentoNovedad = Convert.ToDateTime(objCnx.Reader["momento_novedad"]);
                    listaNovedades.Add(oNovedad);
                }
            }
            if (objCnx.Reader != null && !objCnx.Reader.IsClosed)
            {
                objCnx.Reader.Close();
            }
            objCnx.CerrarConexion();
            return listaNovedades;
        }

        public static bool ActualizarNovedad(Novedad oNovedad)
        {
            ConeccionbdClass1 objCnx = new ConeccionbdClass1();
            string sentencia;

            string idAdminStr = oNovedad.IdAdministrador.HasValue ? oNovedad.IdAdministrador.Value.ToString() : "NULL";
            string idPersonalStr = oNovedad.IdPersonal.HasValue ? oNovedad.IdPersonal.Value.ToString() : "NULL";

            sentencia = "EXECUTE ActualizarNovedad " +
                        oNovedad.IdNovedad + ", " +
                        oNovedad.IdVigilante + ", " +
                        idAdminStr + ", " +
                        idPersonalStr + ", " +
                        "'" + oNovedad.Descripcion + "'";

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

        // 4. Método para Eliminar una novedad por su ID
        public static bool EliminarNovedad(int idNovedad)
        {
            ConeccionbdClass1 objCnx = new ConeccionbdClass1();
            string sentencia;

            sentencia = "EXECUTE EliminarNovedad '" + idNovedad + "'";

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

        // 5. Método para Consultar (obtener) una novedad por su ID
        public static Novedad ConsultarNovedadPorId(int idNovedad)
        {
            Novedad oNovedad = null;
            ConeccionbdClass1 objCnx = new ConeccionbdClass1();
            string sentencia;

            sentencia = "EXECUTE ConsultarNovedad '" + idNovedad + "'";

            if (objCnx.Consultar(sentencia, false))
            {
                if (objCnx.Reader.Read())
                {
                    oNovedad = new Novedad();
                    oNovedad.IdNovedad = Convert.ToInt32(objCnx.Reader["id_novedad"]);
                    oNovedad.IdVigilante = Convert.ToInt32(objCnx.Reader["id_vigilante"]);

                    oNovedad.IdAdministrador = objCnx.Reader["id_administrador"] != DBNull.Value ? (int?)Convert.ToInt32(objCnx.Reader["id_administrador"]) : null;
                    oNovedad.IdPersonal = objCnx.Reader["id_personal"] != DBNull.Value ? (int?)Convert.ToInt32(objCnx.Reader["id_personal"]) : null;

                    oNovedad.Descripcion = objCnx.Reader["descripcion"].ToString();
                    oNovedad.MomentoNovedad = Convert.ToDateTime(objCnx.Reader["momento_novedad"]);
                }
            }

            if (objCnx.Reader != null && !objCnx.Reader.IsClosed)
            {
                objCnx.Reader.Close();
            }
            objCnx.CerrarConexion();
            return oNovedad;
        }
    }
}
