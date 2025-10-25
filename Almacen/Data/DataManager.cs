using System;
using System.Collections.Generic;
using System.Linq;
using Almacen.Models;

namespace Almacen.Data
{
    public class DataManager
    {
        private static DataManager _instance;
        public static DataManager Instance => _instance ?? (_instance = new DataManager());

        // Listas para almacenar los datos en memoria
        public List<Cliente> Clientes { get; private set; }
        public List<Producto> Productos { get; private set; }
        public List<Pedido> Pedidos { get; private set; }
        public List<Venta> Ventas { get; private set; }
        public List<Usuario> Usuarios { get; private set; }

        // Contadores para IDs
        private int _nextClienteId = 1;
        private int _nextProductoId = 1;
        private int _nextPedidoId = 1;
        private int _nextVentaId = 1;
        private int _nextDetallePedidoId = 1;
        private int _nextDetalleVentaId = 1;
        private int _nextUsuarioId = 1;

        private DataManager()
        {
            Clientes = new List<Cliente>();
            Productos = new List<Producto>();
            Pedidos = new List<Pedido>();
            Ventas = new List<Venta>();
            Usuarios = new List<Usuario>();
            
            InicializarDatosPrueba();
        }

        private void InicializarDatosPrueba()
        {
            // Agregar algunos clientes de prueba
            AgregarCliente(new Cliente
            {
                Nombre = "Juan",
                Apellido = "Pérez",
                Email = "juan.perez@email.com",
                Telefono = "555-0001",
                Direccion = "Calle 123, Ciudad"
            });

            AgregarCliente(new Cliente
            {
                Nombre = "María",
                Apellido = "González",
                Email = "maria.gonzalez@email.com",
                Telefono = "555-0002",
                Direccion = "Avenida 456, Ciudad"
            });

            // Agregar algunos productos de prueba
            AgregarProducto(new Producto
            {
                Codigo = "PROD001",
                Nombre = "Laptop HP",
                Descripcion = "Laptop HP 15.6 pulgadas",
                Precio = 850.00m,
                Stock = 10,
                StockMinimo = 2,
                Categoria = "Electrónicos"
            });

            AgregarProducto(new Producto
            {
                Codigo = "PROD002",
                Nombre = "Mouse Inalámbrico",
                Descripcion = "Mouse óptico inalámbrico",
                Precio = 25.50m,
                Stock = 50,
                StockMinimo = 10,
                Categoria = "Accesorios"
            });

            AgregarProducto(new Producto
            {
                Codigo = "PROD003",
                Nombre = "Teclado Mecánico",
                Descripcion = "Teclado mecánico RGB",
                Precio = 75.00m,
                Stock = 3,
                StockMinimo = 5,
                Categoria = "Accesorios"
            });

            // Agregar usuarios de prueba
            AgregarUsuario(new Usuario
            {
                NombreUsuario = "admin",
                Contraseña = "admin123",
                Nombre = "Administrador",
                Apellido = "Sistema",
                Email = "admin@almacen.com",
                Tipo = TipoUsuario.Administrador
            });

            AgregarUsuario(new Usuario
            {
                NombreUsuario = "empleado1",
                Contraseña = "emp123",
                Nombre = "Carlos",
                Apellido = "Rodriguez",
                Email = "carlos@almacen.com",
                Tipo = TipoUsuario.Empleado
            });

            AgregarUsuario(new Usuario
            {
                NombreUsuario = "vendedor1",
                Contraseña = "vend123",
                Nombre = "Ana",
                Apellido = "Martinez",
                Email = "ana@almacen.com",
                Tipo = TipoUsuario.Vendedor
            });
        }

        // Métodos para Clientes
        public void AgregarCliente(Cliente cliente)
        {
            cliente.Id = _nextClienteId++;
            Clientes.Add(cliente);
        }

        public void ActualizarCliente(Cliente cliente)
        {
            var existente = Clientes.FirstOrDefault(c => c.Id == cliente.Id);
            if (existente != null)
            {
                existente.Nombre = cliente.Nombre;
                existente.Apellido = cliente.Apellido;
                existente.Email = cliente.Email;
                existente.Telefono = cliente.Telefono;
                existente.Direccion = cliente.Direccion;
            }
        }

        public void EliminarCliente(int id)
        {
            var cliente = Clientes.FirstOrDefault(c => c.Id == id);
            if (cliente != null)
            {
                Clientes.Remove(cliente);
            }
        }

        public Cliente ObtenerCliente(int id)
        {
            return Clientes.FirstOrDefault(c => c.Id == id);
        }

        // Métodos para Productos
        public void AgregarProducto(Producto producto)
        {
            producto.Id = _nextProductoId++;
            Productos.Add(producto);
        }

        public void ActualizarProducto(Producto producto)
        {
            var existente = Productos.FirstOrDefault(p => p.Id == producto.Id);
            if (existente != null)
            {
                existente.Codigo = producto.Codigo;
                existente.Nombre = producto.Nombre;
                existente.Descripcion = producto.Descripcion;
                existente.Precio = producto.Precio;
                existente.Stock = producto.Stock;
                existente.StockMinimo = producto.StockMinimo;
                existente.Categoria = producto.Categoria;
            }
        }

        public void EliminarProducto(int id)
        {
            var producto = Productos.FirstOrDefault(p => p.Id == id);
            if (producto != null)
            {
                Productos.Remove(producto);
            }
        }

        public Producto ObtenerProducto(int id)
        {
            return Productos.FirstOrDefault(p => p.Id == id);
        }

        public void ActualizarStock(int productoId, int cantidad)
        {
            var producto = ObtenerProducto(productoId);
            if (producto != null)
            {
                producto.Stock += cantidad;
            }
        }

        public bool TieneStockSuficiente(int productoId, int cantidad)
        {
            var producto = ObtenerProducto(productoId);
            return producto != null && producto.Stock >= cantidad;
        }

        public List<Producto> ObtenerProductosBajoStock()
        {
            return Productos.Where(p => p.TieneBajoStock).ToList();
        }

        // Métodos para Pedidos
        public void AgregarPedido(Pedido pedido)
        {
            pedido.Id = _nextPedidoId++;
            
            foreach (var detalle in pedido.Detalles)
            {
                detalle.Id = _nextDetallePedidoId++;
                detalle.PedidoId = pedido.Id;
                detalle.Producto = ObtenerProducto(detalle.ProductoId);
            }
            
            pedido.Cliente = ObtenerCliente(pedido.ClienteId);
            Pedidos.Add(pedido);
        }

        public void ActualizarEstadoPedido(int pedidoId, EstadoPedido nuevoEstado)
        {
            var pedido = Pedidos.FirstOrDefault(p => p.Id == pedidoId);
            if (pedido != null)
            {
                pedido.Estado = nuevoEstado;
                
                // Si el pedido se completa, actualizar stock
                if (nuevoEstado == EstadoPedido.Completado)
                {
                    foreach (var detalle in pedido.Detalles)
                    {
                        ActualizarStock(detalle.ProductoId, -detalle.Cantidad);
                    }
                }
            }
        }

        public Pedido ObtenerPedido(int id)
        {
            return Pedidos.FirstOrDefault(p => p.Id == id);
        }

        // Métodos para Ventas
        public void AgregarVenta(Venta venta)
        {
            venta.Id = _nextVentaId++;
            
            foreach (var detalle in venta.Detalles)
            {
                detalle.Id = _nextDetalleVentaId++;
                detalle.VentaId = venta.Id;
                detalle.Producto = ObtenerProducto(detalle.ProductoId);
                
                // Actualizar stock inmediatamente
                ActualizarStock(detalle.ProductoId, -detalle.Cantidad);
            }
            
            if (venta.ClienteId.HasValue)
            {
                venta.Cliente = ObtenerCliente(venta.ClienteId.Value);
            }
            
            venta.Total = venta.TotalCalculado;
            Ventas.Add(venta);
        }

        public Venta ObtenerVenta(int id)
        {
            return Ventas.FirstOrDefault(v => v.Id == id);
        }

        // Métodos de consulta
        public decimal ObtenerTotalVentasDelDia(DateTime fecha)
        {
            return Ventas
                .Where(v => v.FechaVenta.Date == fecha.Date)
                .Sum(v => v.Total);
        }

        public List<Venta> ObtenerVentasDelPeriodo(DateTime fechaInicio, DateTime fechaFin)
        {
            return Ventas
                .Where(v => v.FechaVenta.Date >= fechaInicio.Date && v.FechaVenta.Date <= fechaFin.Date)
                .OrderByDescending(v => v.FechaVenta)
                .ToList();
        }

        public List<Producto> BuscarProductos(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return Productos.ToList();

            texto = texto.ToLower();
            return Productos
                .Where(p => p.Codigo.ToLower().Contains(texto) || 
                           p.Nombre.ToLower().Contains(texto) || 
                           p.Descripcion.ToLower().Contains(texto))
                .ToList();
        }

        public List<Cliente> BuscarClientes(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return Clientes.ToList();

            texto = texto.ToLower();
            return Clientes
                .Where(c => c.Nombre.ToLower().Contains(texto) || 
                           c.Apellido.ToLower().Contains(texto) || 
                           c.Email.ToLower().Contains(texto))
                .ToList();
        }

        // Métodos para Usuarios
        public void AgregarUsuario(Usuario usuario)
        {
            usuario.Id = _nextUsuarioId++;
            Usuarios.Add(usuario);
        }

        public void ActualizarUsuario(Usuario usuario)
        {
            var existente = Usuarios.FirstOrDefault(u => u.Id == usuario.Id);
            if (existente != null)
            {
                existente.NombreUsuario = usuario.NombreUsuario;
                existente.Contraseña = usuario.Contraseña;
                existente.Nombre = usuario.Nombre;
                existente.Apellido = usuario.Apellido;
                existente.Email = usuario.Email;
                existente.Tipo = usuario.Tipo;
                existente.Activo = usuario.Activo;
                existente.UltimoAcceso = usuario.UltimoAcceso;
            }
        }

        public void EliminarUsuario(int id)
        {
            var usuario = Usuarios.FirstOrDefault(u => u.Id == id);
            if (usuario != null)
            {
                Usuarios.Remove(usuario);
            }
        }

        public Usuario ObtenerUsuario(int id)
        {
            return Usuarios.FirstOrDefault(u => u.Id == id);
        }

        public Usuario AutenticarUsuario(string nombreUsuario, string contraseña)
        {
            return Usuarios.FirstOrDefault(u => 
                u.NombreUsuario.Equals(nombreUsuario, StringComparison.OrdinalIgnoreCase) && 
                u.Contraseña == contraseña && 
                u.Activo);
        }

        public bool ExisteNombreUsuario(string nombreUsuario, int? idExcluir = null)
        {
            return Usuarios.Any(u => 
                u.NombreUsuario.Equals(nombreUsuario, StringComparison.OrdinalIgnoreCase) && 
                (!idExcluir.HasValue || u.Id != idExcluir.Value));
        }

        public List<Usuario> BuscarUsuarios(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return Usuarios.ToList();

            texto = texto.ToLower();
            return Usuarios
                .Where(u => u.NombreUsuario.ToLower().Contains(texto) || 
                           u.Nombre.ToLower().Contains(texto) || 
                           u.Apellido.ToLower().Contains(texto) || 
                           u.Email.ToLower().Contains(texto))
                .ToList();
        }
    }
}