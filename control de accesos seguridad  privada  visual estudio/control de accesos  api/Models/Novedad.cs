using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace control_de_accesos_api.Models
{
    public class Novedad
    {
        // Propiedad que representa la clave primaria de la tabla 'Novedades'.
        // Se mapea a la columna 'id_novedad' de tipo INT.
        public int IdNovedad { get; set; }

        // Propiedad para la clave foránea del vigilante.
        // Se mapea a la columna 'id_vigilante' de tipo INT.
        public int IdVigilante { get; set; }

        // Propiedad para la clave foránea del administrador.
        // El tipo es Nullable (int?) porque la columna 'id_administrador' permite valores nulos.
        public int? IdAdministrador { get; set; }

        // Propiedad para la clave foránea del personal.
        // El tipo es Nullable (int?) porque la columna 'id_personal' permite valores nulos.
        public int? IdPersonal { get; set; }

        // Propiedad para la descripción de la novedad.
        // Se mapea a la columna 'descripcion' de tipo VARCHAR(500).
        public string Descripcion { get; set; }

        // Propiedad para el momento en que se registra la novedad.
        // Se mapea a la columna 'momento_novedad' de tipo DATETIME.
        public DateTime MomentoNovedad { get; set; }
    }
}