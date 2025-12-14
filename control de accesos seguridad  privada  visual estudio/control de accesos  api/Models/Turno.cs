using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace control_de_accesos__api.Models
{
    // Esta es la clase Turno
    public class Turno
    {
        public int IdTurno { get; set; }
        public string AsignacionTurno { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
    }
}

