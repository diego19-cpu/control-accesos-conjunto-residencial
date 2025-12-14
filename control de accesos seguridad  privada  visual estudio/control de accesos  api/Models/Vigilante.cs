using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace control_de_accesos_api.Models
{
    // Esta es la clase Vigilante, que se relaciona con Turno y Administrador
    public class Vigilante
    {
        public int IdVigilante { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int IdTurno { get; set; }
        public int IdAdministrador { get; set; }
    }
}
