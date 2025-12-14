using System;
using System.Collections.Generic;
using System.Data; // Necesario para DataTable o DataSet
using System.Linq;
using System.Web;
using control_de_accesos__api.Models; // Asegúrate de que este namespace sea correcto para tu clase AdministradorClass1
using control_de_accesos__api.data2; // Asegúrate de que este namespace sea correcto para tu clase ConeccionbdClass1

namespace control_de_accesos__api.data2
{
    public class AdministradorData
    {
        // 1. Método para Registrar un nuevo administrador
        public static bool RegistrarAdministrador(Administrador oAdministrador)
        {
            ConeccionbdClass1 objCnx = new ConeccionbdClass1();
            string sentencia;

            sentencia = "EXECUTE RegistrarAdministrador " +
                        "'" + oAdministrador.IdAdministrador + "'," +
                        "'" + oAdministrador.Nombre + "'," +
                        "'" + oAdministrador.Correo + "'," +
                        "'" + oAdministrador.Cargo + "'";

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


        // 2. Método para Listar todos los administradores
        public static List<Administrador> ListarAdministradores()
        {
            List<Administrador> listaAdministradores = new List<Administrador>();
            ConeccionbdClass1 objCnx = new ConeccionbdClass1();
            // Llama al procedimiento almacenado 'ListarAdministradores' directamente en SQL Server
            string sentencia = "EXECUTE ListarAdministradores";

            if (objCnx.Consultar(sentencia, false)) // 'false' porque es un SP sin parámetros ADO.NET
            {
                while (objCnx.Reader.Read())
                {
                    // Mapea los datos del SqlDataReader a un objeto AdministradorClass1
                    Administrador oAdministrador = new Administrador();
                    oAdministrador.IdAdministrador = Convert.ToInt32(objCnx.Reader["id_administrador"]);
                    oAdministrador.Nombre = objCnx.Reader["nombre"].ToString();
                    oAdministrador.Correo = objCnx.Reader["correo"].ToString();
                    oAdministrador.Cargo = objCnx.Reader["cargo"].ToString();
                    listaAdministradores.Add(oAdministrador);
                }
            }
            // Importante: Cerrar la conexión y el lector
            if (objCnx.Reader != null && !objCnx.Reader.IsClosed)
            {
                objCnx.Reader.Close();
            }
            objCnx.CerrarConexion();
            return listaAdministradores;
        }

        // 3. Método para Actualizar un administrador existente
        // Asume que tienes un procedimiento almacenado llamado 'ActualizarAdministrador' en SQL Server
        public static bool ActualizarAdministrador(Administrador oAdministrador)
        {
            ConeccionbdClass1 objCnx = new ConeccionbdClass1();
            string sentencia;

            sentencia = "EXECUTE ActualizarAdministrador " +
                        "'" + oAdministrador.IdAdministrador + "'," +
                        "'" + oAdministrador.Nombre + "'," +
                        "'" + oAdministrador.Correo + "'," +
                        "'" + oAdministrador.Cargo + "'";

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

        // 4. Método para Eliminar un administrador por su ID
        // Asume que tienes un procedimiento almacenado llamado 'EliminarAdministrador' en SQL Server
        public static bool EliminarAdministrador(int idAdministrador)
        {
            ConeccionbdClass1 objCnx = new ConeccionbdClass1();
            string sentencia;

            sentencia = "EXECUTE EliminarAdministrador '" + idAdministrador + "'";

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

        // 5. Método para Consultar (obtener) un administrador por su ID
        // Ahora llama a 'ConsultarAdministrador' en SQL Server para que coincida con tu SP existente.
        public static Administrador ConsultarAdministradorPorId(int idAdministrador)
        {
            Administrador oAdministrador = null; // Inicializa como null
            ConeccionbdClass1 objCnx = new ConeccionbdClass1();
            string sentencia;

            // *** CORRECCIÓN APLICADA AQUÍ: Llamando a 'ConsultarAdministrador' ***
            sentencia = "EXECUTE ConsultarAdministrador '" + idAdministrador + "'";

            if (objCnx.Consultar(sentencia, false)) // 'false' porque es un SP sin parámetros ADO.NET
            {
                if (objCnx.Reader.Read()) // Si encuentra un registro
                {
                    oAdministrador = new Administrador();
                    oAdministrador.IdAdministrador = Convert.ToInt32(objCnx.Reader["id_administrador"]);
                    oAdministrador.Nombre = objCnx.Reader["nombre"].ToString();
                    oAdministrador.Correo = objCnx.Reader["correo"].ToString();
                    oAdministrador.Cargo = objCnx.Reader["cargo"].ToString();
                }
            }
            // Importante: Cerrar la conexión y el lector
            if (objCnx.Reader != null && !objCnx.Reader.IsClosed)
            {
                objCnx.Reader.Close();
            }
            objCnx.CerrarConexion();
            return oAdministrador; // Devolverá null si no se encontró el administrador
        }
    }
}
