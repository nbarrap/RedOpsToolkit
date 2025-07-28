using System;

namespace Almacen.Models
{
    public enum TipoUsuario
    {
        Administrador,
        Empleado,
        Vendedor
    }

    public class Usuario
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public string Contraseña { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public TipoUsuario Tipo { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? UltimoAcceso { get; set; }

        public Usuario()
        {
            FechaCreacion = DateTime.Now;
            Activo = true;
        }

        public string NombreCompleto => $"{Nombre} {Apellido}";
    }
}