using System;
using System.Collections.Generic;
using System.Linq;

namespace Almacen.Models
{
    public enum TipoPago
    {
        Efectivo,
        Tarjeta,
        Transferencia,
        Cheque
    }

    public class Venta
    {
        public int Id { get; set; }
        public int? ClienteId { get; set; }
        public DateTime FechaVenta { get; set; }
        public TipoPago TipoPago { get; set; }
        public decimal Total { get; set; }
        public string Observaciones { get; set; }
        public List<DetalleVenta> Detalles { get; set; }

        // Referencias para facilitar el uso
        public Cliente Cliente { get; set; }

        public Venta()
        {
            FechaVenta = DateTime.Now;
            TipoPago = TipoPago.Efectivo;
            Detalles = new List<DetalleVenta>();
        }

        public decimal TotalCalculado => Detalles?.Sum(d => d.Subtotal) ?? 0;

        public int TotalArticulos => Detalles?.Sum(d => d.Cantidad) ?? 0;

        public string NombreCliente => Cliente?.NombreCompleto ?? "Cliente Ocasional";
    }
}