using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace control_de_accesos__api.Models
{
    public class Administrador
    {
        public int IdAdministrador { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Cargo { get; set; }
    }
}