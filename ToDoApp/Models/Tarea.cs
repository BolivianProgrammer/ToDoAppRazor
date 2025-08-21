using System;

namespace ToDoAppRazor.Models
{
    public class Tarea
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public string Estado { get; set; }

        public string FechaFormateada()
        {
            return FechaExpiracion.ToString("dd/MM/yyyy");
        }

        public string BadgeClass()
        {
            return Estado switch
            {
                "Terminada" => "badge-success",
                "Pendiente" => "badge-warning",
                "Cancelada" => "badge-danger",
                _ => "badge-secondary"
            };
        }
    }
}