using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using control_de_accesos__api.Models;
using control_de_accesos__api.data2;

namespace control_de_accesos__api.data2
{
    public class AccesoData
    {
        // 1. Método para Registrar un nuevo acceso
        public static bool RegistrarAcceso(Acceso oAcceso) 
        {
            try
            {
                ConeccionbdClass1 objCnx = new ConeccionbdClass1();
                string sentencia;

                sentencia = "EXECUTE RegistrarAcceso " +
                            "'" + oAcceso.IdAcceso + "'," + // <-- El ID ha sido agregado aquí
                            "'" + oAcceso.IdVigilante + "'," +
                            "'" + oAcceso.IdPersonal + "'," +
                            "'" + oAcceso.MomentoIngreso.ToString("yyyy-MM-ddTHH:mm:ss") + "'," +
                            "'" + (oAcceso.MomentoSalida.HasValue ? oAcceso.MomentoSalida.Value.ToString("yyyy-MM-ddTHH:mm:ss") : "NULL") + "'," +
                            "'" + (oAcceso.NumeroVisitas.HasValue ? oAcceso.NumeroVisitas.Value.ToString() : "NULL") + "'," +
                            "'" + (oAcceso.AutorizadoPor ?? "NULL") + "'," +
                            "'" + (oAcceso.NumeroPlacaVehiculo ?? "NULL") + "'";

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
            catch(Exception ex)
            {
                return false;

            }
    
        }

        // 2. Método para Listar todos los accesos
        public static List<Acceso> ListarAccesos()
        {
            List<Acceso> listaAccesos = new List<Acceso>();
            ConeccionbdClass1 objCnx = new ConeccionbdClass1();
            string sentencia = "EXECUTE ListarAccesos";

            if (objCnx.Consultar(sentencia, false))
            {
                while (objCnx.Reader.Read())
                {
                    Acceso oAcceso = new Acceso();
                    oAcceso.IdAcceso = Convert.ToInt32(objCnx.Reader["id_acceso"]);
                    oAcceso.IdVigilante = Convert.ToInt32(objCnx.Reader["id_vigilante"]);
                    oAcceso.IdPersonal = Convert.ToInt32(objCnx.Reader["id_personal"]);
                    oAcceso.MomentoIngreso = Convert.ToDateTime(objCnx.Reader["momento_ingreso"]);
                    oAcceso.MomentoSalida = objCnx.Reader["momento_salida"] as DateTime?;
                    oAcceso.NumeroVisitas = objCnx.Reader["numero_visitas"] as int?;
                    oAcceso.AutorizadoPor = objCnx.Reader["autorizado_por"].ToString();
                    oAcceso.NumeroPlacaVehiculo = objCnx.Reader["numero_placa_vehiculo"].ToString();
                    listaAccesos.Add(oAcceso);
                }
            }

            if (objCnx.Reader != null && !objCnx.Reader.IsClosed)
            {
                objCnx.Reader.Close();
            }
            objCnx.CerrarConexion();
            return listaAccesos;
        }

        // 3. Método para Actualizar un acceso existente
        public static bool ActualizarAcceso(Acceso oAcceso)
        {
            ConeccionbdClass1 objCnx = new ConeccionbdClass1();
            string sentencia;

            sentencia = "EXECUTE ActualizarAcceso " +
                        "'" + oAcceso.IdAcceso + "'," +
                        "'" + oAcceso.IdVigilante + "'," +
                        "'" + oAcceso.IdPersonal + "'," +
                        "'" + oAcceso.MomentoIngreso.ToString("yyyy-MM-ddTHH:mm:ss") + "'," +
                        "'" + (oAcceso.MomentoSalida.HasValue ? oAcceso.MomentoSalida.Value.ToString("yyyy-MM-ddTHH:mm:ss") : "NULL") + "'," +
                        "'" + (oAcceso.NumeroVisitas.HasValue ? oAcceso.NumeroVisitas.Value.ToString() : "NULL") + "'," +
                        "'" + (oAcceso.AutorizadoPor ?? "NULL") + "'," +
                        "'" + (oAcceso.NumeroPlacaVehiculo ?? "NULL") + "'";

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

        // 4. Método para Eliminar un acceso por su ID
        public static bool EliminarAcceso(int idAcceso)
        {
            ConeccionbdClass1 objCnx = new ConeccionbdClass1();
            string sentencia;

            sentencia = "EXECUTE EliminarAcceso '" + idAcceso + "'";

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

        // 5. Método para Consultar (obtener) un acceso por su ID
        public static Acceso ConsultarAccesoPorId(int idAcceso)
        {
            Acceso oAcceso = null;
            ConeccionbdClass1 objCnx = new ConeccionbdClass1();
            string sentencia;

            sentencia = "EXECUTE ConsultarAcceso '" + idAcceso + "'";

            if (objCnx.Consultar(sentencia, false))
            {
                if (objCnx.Reader.Read())
                {
                    oAcceso = new Acceso();
                    oAcceso.IdAcceso = Convert.ToInt32(objCnx.Reader["id_acceso"]);
                    oAcceso.IdVigilante = Convert.ToInt32(objCnx.Reader["id_vigilante"]);
                    oAcceso.IdPersonal = Convert.ToInt32(objCnx.Reader["id_personal"]);
                    oAcceso.MomentoIngreso = Convert.ToDateTime(objCnx.Reader["momento_ingreso"]);
                    oAcceso.MomentoSalida = objCnx.Reader["momento_salida"] as DateTime?;
                    oAcceso.NumeroVisitas = objCnx.Reader["numero_visitas"] as int?;
                    oAcceso.AutorizadoPor = objCnx.Reader["autorizado_por"].ToString();
                    oAcceso.NumeroPlacaVehiculo = objCnx.Reader["numero_placa_vehiculo"].ToString();
                }
            }

            if (objCnx.Reader != null && !objCnx.Reader.IsClosed)
            {
                objCnx.Reader.Close();
            }
            objCnx.CerrarConexion();
            return oAcceso;
        }
    }
}