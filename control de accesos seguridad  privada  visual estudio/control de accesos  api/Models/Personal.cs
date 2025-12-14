using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace control_de_accesos__api.Models
{
    public class Personal
    {
        // Propiedades que corresponden a las columnas de la tabla Personal
        public int IdPersonal { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string DocumentoIdentidad { get; set; }
        public string TipoPersona { get; set; }
        // Se incluye la clave foránea para la relación con la tabla Inmueble
        public int IdInmueble { get; set; }
    }
}