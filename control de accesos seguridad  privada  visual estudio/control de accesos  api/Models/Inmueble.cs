using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace control_de_accesos__api.Models
{
    public class Inmueble
    {
        // Propiedades que corresponden a las columnas de la tabla Inmueble
        public int IdInmueble { get; set; }
        public string NumeroTorre { get; set; }
        public int Piso { get; set; }
        public string Apartamento { get; set; }
        // Se incluye la clave foránea para la relación con la tabla Propietario
        public int IdPropietario { get; set; }
    }
}
