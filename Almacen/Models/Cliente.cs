using System;

namespace Almacen.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public DateTime FechaRegistro { get; set; }

        public Cliente()
        {
            FechaRegistro = DateTime.Now;
        }

        public string NombreCompleto => $"{Nombre} {Apellido}";

        public override string ToString()
        {
            return NombreCompleto;
        }
    }
}