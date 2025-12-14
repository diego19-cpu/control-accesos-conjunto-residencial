using System;

namespace control_de_accesos__api.Models
{
    // Modelo que representa la tabla 'Acceso' de la base de datos
    public class Acceso
    {
        // Llave primaria de la tabla
        public int IdAcceso { get; set; }

        // Llave foránea para el vigilante que registra el acceso
        public int IdVigilante { get; set; }

        // Llave foránea para la persona a la que se le concede el acceso
        public int IdPersonal { get; set; }

        // Fecha y hora de ingreso. No se hace nullable ya que la base de datos tiene un valor por defecto.
        public DateTime MomentoIngreso { get; set; }

        // Fecha y hora de salida. Es nullable ya que puede no estar registrada al momento del ingreso.
        public DateTime? MomentoSalida { get; set; }

        // Número de visitas. Es nullable, ya que no es un campo obligatorio en la base de datos.
        public int? NumeroVisitas { get; set; }

        // Nombre de la persona que autoriza el acceso
        public string AutorizadoPor { get; set; }

        // Número de la placa del vehículo, si aplica
        public string NumeroPlacaVehiculo { get; set; }
    }
}
