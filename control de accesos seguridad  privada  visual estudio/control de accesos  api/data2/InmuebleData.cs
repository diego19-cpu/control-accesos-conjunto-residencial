using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using control_de_accesos__api.Models;
using control_de_accesos__api.data2;

namespace control_de_accesos__api.data2
{
    public class InmuebleData
    {
        // 1. Método para Registrar un nuevo inmueble
        public static bool RegistrarInmueble(Inmueble oInmueble)
        {
            ConeccionbdClass1 objCnx = new ConeccionbdClass1();
            string sentencia;

            sentencia = "EXECUTE RegistrarInmueble " +
                        oInmueble.IdInmueble + "," +
                        "'" + oInmueble.NumeroTorre + "'," +
                        oInmueble.Piso + "," +
                        "'" + oInmueble.Apartamento + "'," +
                        oInmueble.IdPropietario;

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

        // 2. Método para Listar todos los inmuebles
        public static List<Inmueble> ListarInmuebles()
        {
            List<Inmueble> listaInmuebles = new List<Inmueble>();
            ConeccionbdClass1 objCnx = new ConeccionbdClass1();
            string sentencia = "EXECUTE ListarInmuebles";

            if (objCnx.Consultar(sentencia, false))
            {
                while (objCnx.Reader.Read())
                {
                    Inmueble oInmueble = new Inmueble();
                    oInmueble.IdInmueble = Convert.ToInt32(objCnx.Reader["id_inmueble"]);
                    oInmueble.NumeroTorre = objCnx.Reader["numero_torre"].ToString();
                    oInmueble.Piso = Convert.ToInt32(objCnx.Reader["piso"]);
                    oInmueble.Apartamento = objCnx.Reader["apartamento"].ToString();
                    oInmueble.IdPropietario = Convert.ToInt32(objCnx.Reader["id_propietario"]);
                    listaInmuebles.Add(oInmueble);
                }
            }

            if (objCnx.Reader != null && !objCnx.Reader.IsClosed)
            {
                objCnx.Reader.Close();
            }
            objCnx.CerrarConexion();
            return listaInmuebles;
        }

        // 3. Método para Actualizar un inmueble existente
        public static bool ActualizarInmueble(Inmueble oInmueble)
        {
            ConeccionbdClass1 objCnx = new ConeccionbdClass1();
            string sentencia;

            sentencia = "EXECUTE ActualizarInmueble " +
                        oInmueble.IdInmueble + "," +
                        "'" + oInmueble.NumeroTorre + "'," +
                        oInmueble.Piso + "," +
                        "'" + oInmueble.Apartamento + "'," +
                        oInmueble.IdPropietario;

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

        // 4. Método para Eliminar un inmueble por su ID
        public static bool EliminarInmueble(int idInmueble)
        {
            ConeccionbdClass1 objCnx = new ConeccionbdClass1();
            string sentencia;

            sentencia = "EXECUTE EliminarInmueble " + idInmueble;

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

        // 5. Método para Consultar (obtener) un inmueble por su ID
        public static Inmueble ConsultarInmueblePorId(int idInmueble)
        {
            Inmueble oInmueble = null;
            ConeccionbdClass1 objCnx = new ConeccionbdClass1();
            string sentencia;

            sentencia = "EXECUTE ConsultarInmueble " + idInmueble;

            if (objCnx.Consultar(sentencia, false))
            {
                if (objCnx.Reader.Read())
                {
                    oInmueble = new Inmueble();
                    oInmueble.IdInmueble = Convert.ToInt32(objCnx.Reader["id_inmueble"]);
                    oInmueble.NumeroTorre = objCnx.Reader["numero_torre"].ToString();
                    oInmueble.Piso = Convert.ToInt32(objCnx.Reader["piso"]);
                    oInmueble.Apartamento = objCnx.Reader["apartamento"].ToString();
                    oInmueble.IdPropietario = Convert.ToInt32(objCnx.Reader["id_propietario"]);
                }
            }

            if (objCnx.Reader != null && !objCnx.Reader.IsClosed)
            {
                objCnx.Reader.Close();
            }
            objCnx.CerrarConexion();
            return oInmueble;
        }
    }
}