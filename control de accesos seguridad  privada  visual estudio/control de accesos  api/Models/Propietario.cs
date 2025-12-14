using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace control_de_accesos__api.Models
{
    // Esta es la clase Propietario, que representa la tabla con el mismo nombre en la base de datos.
    public class Propietario
    {
        // Propiedad que se mapea a la columna id_propietario
        public int IdPropietario { get; set; }

        // Propiedad que se mapea a la columna nombre
        public string Nombre { get; set; }

        // Propiedad que se mapea a la columna apellido
        public string Apellido { get; set; }

        // Propiedad que se mapea a la columna correo
        public string Correo { get; set; }

        // Propiedad que se mapea a la columna telefono
        public string Telefono { get; set; }
    }
}