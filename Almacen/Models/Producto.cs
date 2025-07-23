using System;

namespace Almacen.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public int StockMinimo { get; set; }
        public string Categoria { get; set; }
        public DateTime FechaCreacion { get; set; }

        public Producto()
        {
            FechaCreacion = DateTime.Now;
        }

        public bool TieneBajoStock => Stock <= StockMinimo;

        public override string ToString()
        {
            return $"{Codigo} - {Nombre}";
        }
    }
}