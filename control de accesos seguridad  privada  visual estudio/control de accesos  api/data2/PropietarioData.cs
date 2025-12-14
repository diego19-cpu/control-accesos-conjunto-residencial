using System;
using System.Collections.Generic;
using System.Data; // Necesario para DataTable o DataSet
using System.Linq;
using System.Web;
using control_de_accesos__api.Models;
using control_de_accesos__api.data2;

namespace control_de_accesos__api.data2
{
    // Esta clase maneja las operaciones CRUD para la entidad Propietario,
    // utilizando procedimientos almacenados, al igual que AdministradorData.
    public class PropietarioData
    {
        // 1. Método para Registrar un nuevo propietario
        public static bool RegistrarPropietario(Propietario oPropietario)
        {
            ConeccionbdClass1 objCnx = new ConeccionbdClass1();
            string sentencia;

            sentencia = "EXECUTE RegistrarPropietario " +
                        "'" + oPropietario.IdPropietario + "'," +
                        "'" + oPropietario.Nombre + "'," +
                        "'" + oPropietario.Apellido + "'," +
                        "'" + oPropietario.Correo + "'," +
                        "'" + oPropietario.Telefono + "'";

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

        // 2. Método para Listar todos los propietarios
        public static List<Propietario> ListarPropietarios()
        {
            List<Propietario> listaPropietarios = new List<Propietario>();
            ConeccionbdClass1 objCnx = new ConeccionbdClass1();
            string sentencia = "EXECUTE ListarPropietarios";

            if (objCnx.Consultar(sentencia, false))
            {
                while (objCnx.Reader.Read())
                {
                    Propietario oPropietario = new Propietario();
                    oPropietario.IdPropietario = Convert.ToInt32(objCnx.Reader["id_propietario"]);
                    oPropietario.Nombre = objCnx.Reader["nombre"].ToString();
                    oPropietario.Apellido = objCnx.Reader["apellido"].ToString();
                    oPropietario.Correo = objCnx.Reader["correo"].ToString();
                    oPropietario.Telefono = objCnx.Reader["telefono"].ToString();
                    listaPropietarios.Add(oPropietario);
                }
            }

            if (objCnx.Reader != null && !objCnx.Reader.IsClosed)
            {
                objCnx.Reader.Close();
            }
            objCnx.CerrarConexion();
            return listaPropietarios;
        }

        // 3. Método para Actualizar un propietario existente
        public static bool ActualizarPropietario(Propietario oPropietario)
        {
            ConeccionbdClass1 objCnx = new ConeccionbdClass1();
            string sentencia;

            sentencia = "EXECUTE ActualizarPropietario " +
                        "'" + oPropietario.IdPropietario + "'," +
                        "'" + oPropietario.Nombre + "'," +
                        "'" + oPropietario.Apellido + "'," +
                        "'" + oPropietario.Correo + "'," +
                        "'" + oPropietario.Telefono + "'";

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

        // 4. Método para Eliminar un propietario por su ID
        public static bool EliminarPropietario(int idPropietario)
        {
            ConeccionbdClass1 objCnx = new ConeccionbdClass1();
            string sentencia;

            sentencia = "EXECUTE EliminarPropietario '" + idPropietario + "'";

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

        // 5. Método para Consultar (obtener) un propietario por su ID
        public static Propietario ConsultarPropietarioPorId(int idPropietario)
        {
            Propietario oPropietario = null;
            ConeccionbdClass1 objCnx = new ConeccionbdClass1();
            string sentencia;

            sentencia = "EXECUTE ConsultarPropietario '" + idPropietario + "'";

            if (objCnx.Consultar(sentencia, false))
            {
                if (objCnx.Reader.Read())
                {
                    oPropietario = new Propietario();
                    oPropietario.IdPropietario = Convert.ToInt32(objCnx.Reader["id_propietario"]);
                    oPropietario.Nombre = objCnx.Reader["nombre"].ToString();
                    oPropietario.Apellido = objCnx.Reader["apellido"].ToString();
                    oPropietario.Correo = objCnx.Reader["correo"].ToString();
                    oPropietario.Telefono = objCnx.Reader["telefono"].ToString();
                }
            }

            if (objCnx.Reader != null && !objCnx.Reader.IsClosed)
            {
                objCnx.Reader.Close();
            }
            objCnx.CerrarConexion();
            return oPropietario;
        }
    }
}