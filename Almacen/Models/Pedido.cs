using System;
using System.Collections.Generic;
using System.Linq;

namespace Almacen.Models
{
    public enum EstadoPedido
    {
        Pendiente,
        Procesando,
        Completado,
        Cancelado
    }

    public class Pedido
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public DateTime FechaPedido { get; set; }
        public EstadoPedido Estado { get; set; }
        public string Observaciones { get; set; }
        public List<DetallePedido> Detalles { get; set; }

        // Referencias para facilitar el uso
        public Cliente Cliente { get; set; }

        public Pedido()
        {
            FechaPedido = DateTime.Now;
            Estado = EstadoPedido.Pendiente;
            Detalles = new List<DetallePedido>();
        }

        public decimal Total => Detalles?.Sum(d => d.Subtotal) ?? 0;

        public int TotalArticulos => Detalles?.Sum(d => d.Cantidad) ?? 0;
    }
}